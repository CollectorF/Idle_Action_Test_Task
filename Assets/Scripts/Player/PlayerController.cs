using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(InventoryController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float animationsSpeed;
    [SerializeField]
    [Range(0.0f, 0.3f)]
    private float rotationSmoothTime = 0.12f;
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Joystick joystick;

    private Animator animator;
    private NavMeshAgent agent;

    private int animIDSpeed;
    private int animIDIsHarvesting;
    private bool isHarvesting = false;

    private Vector3 currentVelocity;
    private float targetRotation = 0;
    private float rotationVelocity = 0;

    public delegate void HarvestEvent(bool state);

    public event HarvestEvent OnHarvest;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        AssignAnimationID();
        animator.speed = animationsSpeed;
    }


    private void Update()
    {
        CalculateRotation(joystick.Direction);
        CheckGroundType();
    }

    private void OnAnimatorMove()
    {
        currentVelocity = animator.deltaPosition;
        agent.Move(currentVelocity);
    }

    [ContextMenu("Stop")]
    private void StopAnim()
    {
        animator.Play("Harvesting", 0, 14 / (float)72);
        Debug.LogError("Stop");
    }

    private void CalculateRotation(Vector2 direction)
    {
        animator.SetFloat(animIDSpeed, joystick.Direction.magnitude, 0.05f, Time.deltaTime);
        animator.SetBool(animIDIsHarvesting, isHarvesting);

        if (direction != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
    }

    private void AssignAnimationID()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDIsHarvesting = Animator.StringToHash("IsHarvesting");
    }

    private void CheckGroundType()
    {
        if (joystick.Direction != Vector2.zero)
        {
            if (Physics.Raycast(agent.transform.position, Vector3.down, out RaycastHit hit, 5f, layerMask))
            {
                switch (hit.collider.tag)
                {
                    case "Level/Harvest":
                        isHarvesting = true;
                        OnHarvest?.Invoke(true);
                        break;
                    case "Level/Walkable":
                        isHarvesting = false;
                        OnHarvest?.Invoke(false);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

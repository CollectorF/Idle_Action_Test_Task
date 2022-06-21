using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float harvestSpeed;
    [SerializeField]
    private AnimationCurve harvestSpeedCurve;
    [SerializeField]
    [Range(0.0f, 0.3f)]
    private float rotationSmoothTime = 0.12f;

    [SerializeField]
    private Joystick joystick;

    private Animator animator;
    private NavMeshAgent agent;

    private int animIDSpeed;
    private int animIDIsHarvesting;

    [SerializeField]
    private bool isHarvesting = false;

    private Vector3 movementDirection;
    private AnimatorClipInfo[] currentClipInfo;
    private float targetRotation = 0;
    private float rotationVelocity = 0;
    private float currentSpeed = 0;
    private float currentClipLength;
    private float currentTime = 0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        AssignAnimationID();
    }


    private void Update()
    {
        Walk(joystick.Direction);
    }

    private void Walk(Vector2 direction)
    {
        animator.SetFloat(animIDSpeed, joystick.Direction.magnitude, 0.05f, Time.deltaTime);
        animator.SetBool(animIDIsHarvesting, isHarvesting);


        currentSpeed = 0;
        if (direction != Vector2.zero)
        {
            if (isHarvesting)
            {
                currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
                currentClipLength = currentClipInfo[0].clip.length;
                if (direction == Vector2.zero ^ currentTime >= currentClipLength)
                {
                    currentTime = 0f;
                }
                else
                {
                    currentTime = Mathf.Clamp(currentTime, 0, currentClipLength);
                    currentSpeed = harvestSpeedCurve.Evaluate(currentTime) * harvestSpeed;
                    Debug.Log(currentSpeed);
                    currentTime += Time.deltaTime;
                }
            }
            else
            {
                currentSpeed = walkSpeed;
            }
            targetRotation = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
        movementDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;
        agent.Move(movementDirection * currentSpeed * Time.deltaTime);
    }

    private void AssignAnimationID()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDIsHarvesting = Animator.StringToHash("IsHarvesting");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestingController : MonoBehaviour
{
    [SerializeField]
    private GameObject worktool;
    [SerializeField]
    private GameObject stack;
    [SerializeField]
    private float animationDuration;

    private PlayerController playerController;
    private BlockController currentBlockController;
    private int blocksInStack;

    private int stackSize { get; set; }

    public delegate void StackFullEvent();

    public event StackFullEvent OnStackIsFull;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerController.OnHarvest += ActivateWorktool;
        stackSize = playerController.parameters.StackSize;
        worktool.SetActive(false);
    }

    private void ActivateWorktool(bool state)
    {
        worktool.SetActive(state);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Block"))
        {
            if (blocksInStack < stackSize)
            {
                currentBlockController = collider.gameObject.GetComponent<BlockController>();
                currentBlockController.SetColliderAsTrigger(true);
                currentBlockController.AnimateBlock(stack.transform.position + (Vector3)(playerController.inputJoystick.Direction * 0.5f), animationDuration);
                currentBlockController.gameObject.transform.rotation = stack.transform.rotation;
                currentBlockController.gameObject.transform.SetParent(stack.transform);
                blocksInStack++;
            }
            else
            {
                OnStackIsFull?.Invoke();
            }
        }
    }
}

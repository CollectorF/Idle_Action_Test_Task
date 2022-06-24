using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HarvestingController : MonoBehaviour
{
    [SerializeField]
    private GameObject worktool;
    [SerializeField]
    private GameObject stack;
    [SerializeField]
    private float blockAnimationDuration = 0.3f;
    [SerializeField]
    private float stackAnimationDuration = 0.3f;
    [SerializeField]
    private float stackAnimationAmplitude = 5;

    private PlayerController playerController;
    private BlockController currentBlockController;
    private int blocksInStack;
    private Sequence stackAnimationSequence;

    private int stackSize { get; set; }

    public delegate void StackFullEvent();

    public event StackFullEvent OnStackIsFull;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerController.OnHarvest += ActivateWorktool;
        playerController.OnWalk += SetStackAnimationState;
        stackSize = playerController.parameters.StackSize;
        worktool.SetActive(false);
        stackAnimationSequence = CreateStackAnimation(stackAnimationAmplitude, stackAnimationDuration);
        SetStackAnimationState(false);
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
                currentBlockController.AnimateBlock(stack.transform.position + (Vector3)(playerController.inputJoystick.Direction * 0.5f), blockAnimationDuration);
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

    private void SetStackAnimationState(bool state)
    {
        if (state)
        {
            stackAnimationSequence.Play();
        }
        else
        {
            stackAnimationSequence.Pause();
        }
    }

    private Sequence CreateStackAnimation(float amplitude, float duration)
    {
        Sequence sequence = DOTween.Sequence()
            .Append(stack.transform.DOLocalRotate(new Vector3(0, amplitude, 0), duration, RotateMode.LocalAxisAdd))
            .Append(stack.transform.DOLocalRotate(new Vector3(0, -amplitude, 0), duration, RotateMode.LocalAxisAdd));
        sequence.SetLoops(-1);
        return sequence;
    }
}

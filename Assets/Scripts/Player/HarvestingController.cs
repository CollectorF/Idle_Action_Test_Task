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
    [SerializeField]
    private float blockPickupJumpPower = 1;

    private PlayerController playerController;
    private BlockController currentBlockController;
    private Sequence stackAnimationSequence;

    public int StackSize { get; private set; }
    [HideInInspector]
    public List<BlockController> blocks;

    public delegate void AddToStackEvent();
    public delegate void RemoveFromStackEvent();

    public event AddToStackEvent OnAddToStack;
    public event RemoveFromStackEvent OnRemoveFromStack;


    private void Awake()
    {
        blocks = new List<BlockController>();
        playerController = GetComponent<PlayerController>();
        playerController.OnHarvest += ActivateWorktool;
        playerController.OnWalk += SetStackAnimationState;
        StackSize = playerController.parameters.StackSize;
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
            if (blocks.Count < StackSize)
            {
                currentBlockController = collider.gameObject.GetComponent<BlockController>();
                currentBlockController.SetColliderAsTrigger(true);
                currentBlockController.AnimateBlock(stack.transform.position + (Vector3)(playerController.inputJoystick.Direction * 0.5f),
                    blockAnimationDuration, blockPickupJumpPower);
                currentBlockController.gameObject.transform.rotation = stack.transform.rotation;
                currentBlockController.gameObject.transform.SetParent(stack.transform);
                blocks.Add(currentBlockController);
            }
            OnAddToStack?.Invoke();
        }
    }

    public void RemoveBlockFromStack(BlockController block)
    {
        blocks.Remove(block);
        OnRemoveFromStack?.Invoke();
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

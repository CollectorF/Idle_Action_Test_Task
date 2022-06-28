using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockController : MonoBehaviour
{
    [SerializeField]
    private BlockParameters parameters;
    [SerializeField]
    private Vector3 scaleInStack;

    public int blockCost { get; private set; }

    private new Collider collider;
    private Vector3 startScale;

    private void Awake()
    {
        blockCost = parameters.Cost;
        collider = GetComponent<Collider>();
        startScale = transform.localScale;
        SetColliderEnabledState(true);
    }

    public void SetColliderEnabledState(bool state)
    {
        collider.enabled = state;
    }

    public void AnimateBlock(Vector3 position, float duration, float jumpPower)
    {
        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(scaleInStack, duration))
            .Join(transform.DOJump(position, jumpPower, 1, duration));
    }
}
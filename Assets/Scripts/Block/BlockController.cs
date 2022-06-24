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

    private float blockCost { get; set; }

    private new Collider collider;
    private Vector3 startScale;

    private void Awake()
    {
        blockCost = parameters.Cost;
        collider = GetComponent<Collider>();
        startScale = transform.localScale;
        SetColliderAsTrigger(false);
    }

    public void SetColliderAsTrigger(bool state)
    {
        collider.isTrigger = state;
    }

    public void AnimateBlock(Vector3 position, float duration)
    {
        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(scaleInStack, duration))
            .Join(transform.DOJump(position, 0.5f, 1, duration));
    }
}

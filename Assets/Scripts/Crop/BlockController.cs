using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockController : MonoBehaviour
{
    [SerializeField]
    private BlockParameters parameters;

    public int BlockCost { get; private set; }
    public Vector3 ScaleInStack { get; private set; }

    private new Collider collider;

    private void Awake()
    {
        BlockCost = parameters.Cost;
        ScaleInStack = parameters.ScaleInStack;
        collider = GetComponent<Collider>();
        SetColliderEnabledState(true);
    }

    public void SetColliderEnabledState(bool state)
    {
        collider.enabled = state;
    }

    public void AnimateBlock(Vector3 position, Vector3 scale, float duration, float jumpPower)
    {
        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(scale, duration))
            .Join(transform.DOJump(position, jumpPower, 1, duration));
    }

    public void AnimateBlock(Vector3 position, Vector3 scale, float duration, float jumpPower, Transform parent)
    {
        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(scale, duration))
            .Join(transform.DOJump(position, jumpPower, 1, duration))
            .OnComplete(() => gameObject.transform.position = parent.transform.position);
        gameObject.transform.SetParent(parent.transform);
    }
}

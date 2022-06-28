using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        Sequence stackAnimationSequence = DOTween.Sequence()
            .Append(transform.DOLocalRotate(new Vector3(0, 5, 0), 1, RotateMode.LocalAxisAdd))
            .Append(transform.DOLocalRotate(new Vector3(0, -5, 0), 1, RotateMode.LocalAxisAdd));
        stackAnimationSequence.SetLoops(-1);
    }
}

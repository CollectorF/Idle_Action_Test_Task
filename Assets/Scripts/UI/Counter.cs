using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Counter : MonoBehaviour
{
    [SerializeField]
    private Image coinImage;
    [SerializeField]
    private TextMeshProUGUI counterText;
    [SerializeField]
    private float coinShakeDuration = 0.5f;
    [SerializeField]
    private float coinShakeStrenght = 20;
    [SerializeField]
    private int coinShakeVibrato = 70;

    private int counterValue;
    private Tween tween;

    public void SetCounter(int value)
    {
        counterValue += value;
        counterText.text = counterValue.ToString();
    }

    public int GetCounter()
    {
        return counterValue;
    }

    public void AnimateCoinIcon()
    {
        if (tween == null)
        {
            tween = coinImage.rectTransform.DOShakePosition(coinShakeDuration, coinShakeStrenght, coinShakeVibrato).OnComplete(() => tween = null);
        }
    }
}

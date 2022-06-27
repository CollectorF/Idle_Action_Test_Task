using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class GameplayUI : MonoBehaviour
{
    [SerializeField]
    private Transform coinStartPoint;
    [SerializeField]
    private Transform coinEndPoint;
    [SerializeField]
    private GameObject coinPrefab;

    internal ProgressBar progressBar;
    internal Counter counter;

    private new Camera camera;
    private Vector2 position;

    public delegate void UpdateCounterEvent(BlockController block);

    public event UpdateCounterEvent OnUpdateCounter;

    private void Awake()
    {
        progressBar = GetComponentInChildren<ProgressBar>();
        counter = GetComponentInChildren<Counter>();
        camera = Camera.main;
    }

    public void UpdateProgress(int stackSize, int blocksInStack, float progress)
    {
        if (progress < 1)
        {
            progressBar.SetProgress(stackSize, blocksInStack, progress);
        }
        else
        {
            progressBar.SetProgress();
        }
    }

    public void UpdateCounter(int value)
    {
        counter.SetCounter(value);
    }

    public void AnimateCoinFromSoldBlock(BlockController block)
    {
        position = camera.WorldToScreenPoint(coinStartPoint.position);
        GameObject currentCoin = Instantiate(coinPrefab, position, Quaternion.identity);
        currentCoin.transform.SetParent(gameObject.transform, false);
        Tween tween = currentCoin.transform.DOMove(coinEndPoint.position, 1f);
        tween.OnComplete(() => RemoveAnimatedCoin(currentCoin, tween, block));
    }

    private void RemoveAnimatedCoin(GameObject coin, Tween tween, BlockController block)
    {
        Destroy(coin);
        tween.Kill();
        counter.AnimateCoinIcon();
        OnUpdateCounter?.Invoke(block);
    }
}

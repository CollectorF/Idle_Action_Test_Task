using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    internal ProgressBar progressBar;

    private void Awake()
    {
        progressBar = GetComponentInChildren<ProgressBar>();
    }

    public void UpdateProgress(int stackSize, int blocksInStack, float progress)
    {
        progressBar.SetProgress(stackSize, blocksInStack, progress);
    }
}

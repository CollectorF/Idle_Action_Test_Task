using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameplayUI gameplayUI;
    [SerializeField]
    private PlayerController playerController;

    private HarvestingController harvestingController;

    private void Awake()
    {
        harvestingController = playerController.gameObject.GetComponent<HarvestingController>();
        harvestingController.OnAddToStack += UpdateFillBar;
        harvestingController.OnRemoveFromStack += UpdateFillBar;
    }

    private void Start()
    {
        UpdateFillBar();
    }

    private float CalculateProgress(float stackSize, float blocksInStack)
    {
        float fillPercent = blocksInStack / stackSize;
        return fillPercent;
    }

    private void UpdateFillBar()
    {
        var progress = CalculateProgress(harvestingController.StackSize, harvestingController.blocks.Count);
        gameplayUI.UpdateProgress(harvestingController.StackSize, harvestingController.blocks.Count, progress);
    }
}

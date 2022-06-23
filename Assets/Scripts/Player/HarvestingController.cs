using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestingController : MonoBehaviour
{
    [SerializeField]
    private GameObject worktool;
    [SerializeField]
    private GameObject stack;

    private PlayerController playerController;

    private float stackSize { get; set; }

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerController.OnHarvest += ActivateWorktool;
        stackSize = playerController.parameters.StackSize;
        worktool.SetActive(false);
    }

    private void ActivateWorktool(bool state)
    {
        worktool.SetActive(state);
    }
}

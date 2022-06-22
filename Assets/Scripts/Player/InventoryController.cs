using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private GameObject worktool;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerController.OnHarvest += ActivateWorktool;
        worktool.SetActive(false);
    }

    private void ActivateWorktool(bool state)
    {
        worktool.SetActive(state);
    }
}

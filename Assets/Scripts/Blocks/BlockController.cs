using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    [SerializeField]
    private BlockParameters parameters;

    private float blockCost { get; set; }

    private new Collider collider;

    private void Awake()
    {
        blockCost = parameters.Cost;
        collider = GetComponent<Collider>();
        collider.isTrigger = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collider.isTrigger = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnController : MonoBehaviour
{
    [SerializeField]
    private Transform blockEndPoint;
    [SerializeField]
    private float blockAnimationDuration = 0.7f;
    [SerializeField]
    private float blockUnloadJumpPower = 1.2f;
    [SerializeField]
    private float delayBetweenEachBlock = 0.3f;
    [SerializeField]
    private Vector3 blockScaleInBarn = new Vector3(3, 3, 3);

    private HarvestingController harvestingController;
    private Coroutine dropoffCoroutine;

    public delegate void BlockSoldEvent(BlockController block);

    public event BlockSoldEvent OnBlockSold;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            harvestingController = other.gameObject.GetComponent<HarvestingController>();
            if (harvestingController.blocks.Count > 0)
            {
                foreach (var item in harvestingController.blocks)
                {
                    if (dropoffCoroutine == null)
                    {
                        dropoffCoroutine = StartCoroutine(DropoffCoroutine(item));
                    }
                    return;
                }
            }
            else
            {
                harvestingController.ActivateStack(false);
            }
        }
    }

    private IEnumerator DropoffCoroutine(BlockController item)
    {
        harvestingController.RemoveBlockFromStack(item);
        item.AnimateBlock(blockEndPoint.position, blockScaleInBarn, blockAnimationDuration, blockUnloadJumpPower);
        OnBlockSold?.Invoke(item);
        yield return new WaitForSeconds(delayBetweenEachBlock);
        Destroy(item.gameObject, 1);
        dropoffCoroutine = null;
    }
}

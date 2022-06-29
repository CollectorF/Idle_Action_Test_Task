using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WheatController : MonoBehaviour
{
    [SerializeField]
    public WheatParameters parameters;
    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private GameObject basePlane;
    [SerializeField]
    private Renderer mesh;

    private ParticleSystem particleEffect;
    private GameObject currentBlock;
    private HarvestingController harvestingController;

    public bool isGrowing { get; private set; }
    private Coroutine growCoroutine;

    private void Start()
    {
        particleEffect = GetComponentInChildren<ParticleSystem>();
        harvestingController = FindObjectOfType<HarvestingController>();
        mesh.material.SetColor("_Color", parameters.mellow);
        basePlane.tag = "Level/Harvest";
        isGrowing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blade") && !isGrowing)
        {
            currentBlock = SpawnBlock(other);
            if (harvestingController.blocks.Count < harvestingController.StackSize)
            {
                var controller = currentBlock.GetComponent<BlockController>();
                harvestingController.MoveBlockToStack(controller);
            }
            else
            {
                currentBlock.transform.DOJump(currentBlock.transform.position, 1, 1, 0.5f);
            }
            particleEffect.Play();
            mesh.transform.DOMoveY(-0.5f, 0);
            mesh.material.SetColor("_Color", parameters.growing);
            basePlane.tag = "Level/Walkable";
            isGrowing = true;
            if (growCoroutine == null)
            {
                growCoroutine = StartCoroutine(GrowCoroutine());
            }
        }
    }

    private GameObject SpawnBlock(Collider collider)
    {
        var block = Instantiate(blockPrefab, gameObject.transform.position, Quaternion.Euler(
            new Vector3
            (
                gameObject.transform.rotation.x,
                Random.Range(0, 360),
                gameObject.transform.rotation.z)
            ));
        return block;
    }

    private IEnumerator GrowCoroutine()
    {
        Sequence sequence = DOTween.Sequence()
            .Append(mesh.transform.DOMoveY(0, parameters.growDuration))
            .Join(mesh.material.DOColor(parameters.mellow, "_Color", parameters.growDuration))
            .OnComplete(() => basePlane.tag = "Level/Harvest");
        yield return new WaitForSeconds(parameters.growDuration);
        isGrowing = false;
        growCoroutine = null;
    }
}

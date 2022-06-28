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

    public bool isGrowing { get; private set; }
    private Coroutine growCoroutine;

    private void Start()
    {
        mesh.material.SetColor("_Color", parameters.mellow);
        basePlane.tag = "Level/Harvest";
        isGrowing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blade") && !isGrowing)
        {
            var currentBlock = Instantiate(blockPrefab, gameObject.transform.position,Quaternion.Euler(
                new Vector3
                (
                    gameObject.transform.rotation.x,
                    Random.Range(0, 360),
                    gameObject.transform.rotation.z)
                ));
            currentBlock.transform.DOJump(currentBlock.transform.position, 1, 1, 0.5f);
            gameObject.transform.DOMoveY(-0.5f, 0);
            mesh.material.SetColor("_Color", parameters.growing);
            basePlane.tag = "Level/Walkable";
            isGrowing = true;
            if (growCoroutine == null)
            {
                growCoroutine = StartCoroutine(GrowCoroutine());
            }
        }
    }

    private IEnumerator GrowCoroutine()
    {
        Sequence sequence = DOTween.Sequence()
            .Append(gameObject.transform.DOMoveY(0, parameters.growDuration))
            .Join(mesh.material.DOColor(parameters.mellow, "_Color", parameters.growDuration))
            .OnComplete(() => basePlane.tag = "Level/Harvest");
        yield return new WaitForSeconds(parameters.growDuration);
        isGrowing = false;
        growCoroutine = null;
    }
}

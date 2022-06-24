using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    private Image fillImage;
    [SerializeField]
    private TextMeshProUGUI progressText;

    private float progress;

    public void SetProgress(float stackSize, float blocksInStack, float progress)
    {
        this.progress = progress;
        fillImage.fillAmount = progress;
        progressText.text = $"{blocksInStack}/{stackSize}";
    }

    public float GetProgress()
    {
        return progress;
    }
}

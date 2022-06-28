using UnityEngine;

[CreateAssetMenu(fileName = "wheat", menuName = "Objects/Default Wheat")]
public class WheatParameters : ScriptableObject
{
    public Color growing;
    public Color mellow;
    public float growDuration;
}
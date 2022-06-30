using UnityEngine;

[CreateAssetMenu(fileName = "block", menuName = "Objects/Default Block")]
public class BlockParameters : ScriptableObject
{
    public int Cost;
    public Vector3 ScaleInStack;
}

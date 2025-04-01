using UnityEngine;

[CreateAssetMenu(fileName = "BoomerangNodeDataSO", menuName = "SO/Node/BoomerangNodeDataSO")]
public class BoomerangNodeDataSO : LongNodeDataSO
{
    [Header("Boomerang Number")]
    [Range(1, 10)] public int boomerangNum;
}

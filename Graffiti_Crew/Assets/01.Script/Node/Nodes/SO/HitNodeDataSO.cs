using UnityEngine;

[CreateAssetMenu(fileName = "HitNodeDataSO", menuName = "SO/Node/HitNodeDataSO")]
public class HitNodeDataSO : NodeDataSO
{
    public Vector3 pos;
    public Sprite sprite;
    [Range(2, 100)] public int hitNum;
}

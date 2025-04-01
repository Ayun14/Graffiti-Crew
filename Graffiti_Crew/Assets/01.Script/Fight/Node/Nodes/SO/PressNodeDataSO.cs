using UnityEngine;

[CreateAssetMenu(fileName = "PressNodeDataSO", menuName = "SO/Node/PressNodeDataSO")]
public class PressNodeDataSO : NodeDataSO
{
    public Vector3 pos;
    public Sprite sprite;
    [Range(0.1f, 10)] public float pressTime;
}

using UnityEngine;

public abstract class Node : MonoBehaviour
{
    private bool _isClearNode = false;

    private void Awake()
    {
        Init();
    }

    // √ ±‚»≠
    public virtual void Init()
    {
        _isClearNode = false;
    }

    public void NodeClear()
    {
        _isClearNode = true;
    }

    public abstract NodeType GetNodeType();
}

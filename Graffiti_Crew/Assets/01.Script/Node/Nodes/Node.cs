using UnityEngine;

public abstract class Node : MonoBehaviour
{
    private bool _isClearNode = false;

    private void Awake()
    {
        Init();
    }

    // 초기화
    public virtual void Init()
    {
        _isClearNode = false;
    }

    public virtual void NodeClear()
    {
        _isClearNode = true;

        // 자신이 클리어된 사실을 Judgement에게 알려줘야 한다.
        Destroy(gameObject);
    }

    public abstract NodeType GetNodeType();
}

using UnityEngine;

public abstract class Node : MonoBehaviour
{
    private bool _isClearNode = false;

    private void OnEnable()
    {
        Init();
    }

    // �ʱ�ȭ
    public virtual void Init()
    {
        _isClearNode = false;
    }

    public virtual void NodeClear()
    {
        _isClearNode = true;

        // �ڽ��� Ŭ����� ����� Judgement���� �˷���� �Ѵ�.
    }

    public abstract NodeType GetNodeType();
}

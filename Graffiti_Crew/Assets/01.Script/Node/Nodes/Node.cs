using UnityEngine;

public abstract class Node : MonoBehaviour
{
    protected bool _isClearNode = false;

    protected NodeJudgement _judgement;

    // �ʱ�ȭ
    public virtual void Init(NodeJudgement judgement)
    {
        if (_judgement == null)
            _judgement = judgement;

        _isClearNode = false;
    }

    public virtual void NodeClear()
    {
        _isClearNode = true;

        // �ڽ��� Ŭ����� ����� Judgement���� �˷���� �Ѵ�.
        _judgement?.CheckNodeClear(this);
    }

    public abstract NodeType GetNodeType();
}

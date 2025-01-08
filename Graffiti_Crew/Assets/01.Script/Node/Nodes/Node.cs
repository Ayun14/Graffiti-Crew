using UnityEngine;

public abstract class Node : MonoBehaviour
{
    protected bool _isClearNode = false;

    protected NodeJudgement _judgement;

    // 초기화
    public virtual void Init(NodeJudgement judgement)
    {
        if (_judgement == null)
            _judgement = judgement;

        _isClearNode = false;
    }

    public virtual void NodeClear()
    {
        _isClearNode = true;

        // 자신이 클리어된 사실을 Judgement에게 알려줘야 한다.
        _judgement?.CheckNodeClear(this);
    }

    public abstract NodeType GetNodeType();
}

using UnityEngine;

public abstract class NodeJudgement : MonoBehaviour
{
    protected StageGameRule _stageGameRule;

    [Header("Node")]
    [SerializeField] protected LayerMask _whatIsNode;
    [HideInInspector] public bool isNodeClick;
    [HideInInspector] public Node currentNode;
    protected int _clearNodeCnt = 0;
    public int ClearNodeCnt => _clearNodeCnt;

    public virtual void Init(StageGameRule stageGameRule)
    {
        _stageGameRule = stageGameRule;

        currentNode = null;
        _clearNodeCnt = 0;
    }

    #region Input

    protected virtual void Update()
    {
        if (GameManager.IsPause() == false && _stageGameRule.IsCanInput())
            NodeInput();
    }

    protected abstract void NodeInput();

    protected void NodeClick(Node node)
    {
        if (node is INodeAction actionNode)
        {
            actionNode.NodeStartAction();
        }
    }
    #endregion

    #region Node Clear Check

    public virtual void NodeClear(Node node)
    {
        if (node == null || currentNode == null) return;

        // NodeClear
        if (node == currentNode)
        {
            // Stage Game Rule
            _stageGameRule.NodeClear();
            currentNode = null;
        }
    }

    public virtual void NodeFalse()
    {
        _stageGameRule.NodeFalse();
    }

    public void CurrentNodeFalse() => currentNode?.NodeFalse();

    #endregion
}

using UnityEngine;

public class ClickJudgement : NodeJudgement
{
    [Header("Player")]
    [SerializeField] private SliderValueSO _playerSliderValueSO;

    private void Awake()
    {
        _playerSliderValueSO.Value = _playerSliderValueSO.min;
    }

    protected override void NodeInput()
    {
        if (Input.GetMouseButtonUp(0))
            isNodeClick = false;

        if (_stageGameRule.IsCanInput())
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _whatIsNode))
                {
                    if (hit.transform.parent.TryGetComponent(out Node node))
                    {
                        isNodeClick = true;

                        currentNode = node;
                        NodeClick(currentNode);
                    }
                }
                else // HitNode Combo ½ÇÆÐ
                {
                    if (currentNode != null && currentNode.GetNodeType() == NodeType.HitNode)
                        _stageGameRule.NodeFalse(currentNode);
                }
            }
        }
    }

    public override void NodeClear(Node node)
    {
        base.NodeClear(node);

        // NodeClear
        if (node == currentNode)
        {
            // Player Slider Update
            float percent = ++_clearNodeCnt / (float)_stageGameRule.NodeCnt;
            _playerSliderValueSO.Value = _playerSliderValueSO.max * percent;
        }
    }
}

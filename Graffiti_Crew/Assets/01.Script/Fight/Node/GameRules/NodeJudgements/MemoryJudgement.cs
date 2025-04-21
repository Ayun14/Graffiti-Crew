using DG.Tweening;
using UnityEngine;

public class MemoryJudgement : NodeJudgement
{
    [SerializeField] private SliderValueSO _playerSliderValueSO;
    private Tween _playerSliderValueChangeTween;

    private void Awake()
    {
        _playerSliderValueSO.value = _playerSliderValueSO.max;
    }

    protected override void NodeInput()
    {
        if (Input.GetMouseButtonUp(0))
            isNodeClick = false;

        if (_stageGameRule.IsCanInput() && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _whatIsNode))
            {
                if (hit.transform.parent.TryGetComponent(out Node node))
                {
                    // node 가 지금 가장 앞에 있는 노드 라면?
                    if (_stageGameRule.GetCurrentNode() == node.GetNodeDataSO())
                    {
                        isNodeClick = true;
                        currentNode = node;
                        NodeClick(currentNode);
                    }
                    else
                    {
                        // 클릭 못한 노드 부터 보여야함
                        _stageGameRule.NodeSpawn();
                    }
                }
            }
            else
            {
                // HitNode Combo 실패
                if (currentNode != null && currentNode.GetNodeType() == NodeType.HitNode)
                    NodeFalse();

                // 클릭 못한 노드 부터 보여야함
                _stageGameRule.NodeSpawn();
            }
        }
    }

    public override void NodeFalse()
    {
        base.NodeFalse();

        if (_playerSliderValueChangeTween != null && _playerSliderValueChangeTween.IsActive())
            _playerSliderValueChangeTween.Complete();

        float targetValue = _playerSliderValueSO.Value - 10;
        _playerSliderValueChangeTween = DOTween.To(() => _playerSliderValueSO.Value,
            x => _playerSliderValueSO.Value = x, targetValue, 0.1f);

        if (targetValue <= 0)
            _stageGameRule.AllNodeClear(); // 그냥 끝내버리기..
    }
}

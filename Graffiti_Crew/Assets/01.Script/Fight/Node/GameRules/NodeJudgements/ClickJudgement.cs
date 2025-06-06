using DG.Tweening;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ClickJudgement : NodeJudgement
{
    [Header("Player")]
    [SerializeField] private SliderValueSO _playerSliderValueSO;
    private Tween _playerProgressValueChangeTween;

    private void Awake()
    {
        _playerSliderValueSO.Value = _playerSliderValueSO.min;
    }

    protected override void NodeInput()
    {
        if (Input.GetMouseButtonUp(0))
            isNodeClick = false;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _whatIsNode))
            {
                #region 스프레이 안나옴 처리
                if (_stageGameRule.IsCanInput() == false)
                {
                    // 스프레이 안나오는 소리 나오기
                    GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Spray_NoneGas);
                    return;
                }
                #endregion

                if (hit.transform.parent.TryGetComponent(out Node node))
                {
                    isNodeClick = true;

                    currentNode = node;
                    NodeClick(currentNode);
                }
            }
            else // HitNode Combo 실패
            {
                if (currentNode != null && currentNode.GetNodeType() == NodeType.HitNode)
                    _stageGameRule.NodeFalse();
            }
        }
    }

    public override void NodeClear(Node node)
    {
        if (node == null || currentNode == null) return;

        if (node == currentNode)
        {
            if (_playerSliderValueSO == null) return;
            if (_playerProgressValueChangeTween != null && _playerProgressValueChangeTween.IsActive())
                _playerProgressValueChangeTween.Complete();

            float percent = ++_clearNodeCnt / (float)_stageGameRule.NodeCnt;
            float targetValue = _playerSliderValueSO.max * percent;

            _playerProgressValueChangeTween = DOTween.To(() => _playerSliderValueSO.Value,
                x => _playerSliderValueSO.Value = x, targetValue, 0.2f);
        }
        base.NodeClear(node);
    }
}

using DG.Tweening;
using UnityEngine;

public class ClickJudgement : NodeJudgement
{
    [Header("Player")]
    [SerializeField] private SliderValueSO _playerSliderValueSO;
    [SerializeField] private SliderValueSO _rivalSliderValueSO;
    private Tween _playerProgressValueChangeTween;

    private bool _isBattle = false;

    private void Awake()
    {
        _playerSliderValueSO.Value = _playerSliderValueSO.min;
    }

    protected override void Update()
    {
        base.Update();

        if (_stageGameRule.stageType == StageType.Battle)
        {
            UpdateSliderValue(true);
        }
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
            else
            {
                if (currentNode != null)
                    currentNode.NodeFalse();
                else
                    NodeFalse();
            }
        }
    }

    public override void NodeClear(Node node)
    {
        if (node == null || currentNode == null) return;

        UpdateSliderValue(false);

        if (node == currentNode)
        {
            ++_clearNodeCnt;
            if (_stageGameRule.stageType == StageType.Activity)
                UpdateSliderValue(false);
        }
        base.NodeClear(node);
    }

    private void UpdateSliderValue(bool isBattle)
    {
        if (_playerSliderValueSO == null) return;
        if (_playerProgressValueChangeTween != null && _playerProgressValueChangeTween.IsActive())
            _playerProgressValueChangeTween.Complete();

        float playerValue = _clearNodeCnt / (float)_stageGameRule.NodeCnt * _playerSliderValueSO.max;
        float targetValue = playerValue;
        if (isBattle)
        {
            // 플레이어와 라이벌의 차이
            float rivalValue = _rivalSliderValueSO.Value;
            float addValue = playerValue - rivalValue;
            targetValue = (_playerSliderValueSO.max / 2) + addValue;

        }

        _playerProgressValueChangeTween = DOTween.To(() => _playerSliderValueSO.Value,
            x => _playerSliderValueSO.Value = x, targetValue, 0.2f);
    }
}

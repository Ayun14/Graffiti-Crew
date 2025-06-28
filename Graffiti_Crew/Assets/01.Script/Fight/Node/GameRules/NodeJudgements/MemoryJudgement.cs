using DG.Tweening;
using UnityEngine;

public class MemoryJudgement : NodeJudgement
{
    [SerializeField] private SliderValueSO _playerSliderValueSO;
    private Tween _playerSliderValueChangeTween;

    private void Awake()
    {
        _playerSliderValueSO.Value = _playerSliderValueSO.max;
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
                #region �������� �ȳ��� ó��
                if (_stageGameRule.IsCanInput() == false)
                {
                    // �������� �ȳ����� �Ҹ� ������
                    GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Spray_NoneGas);
                    return;
                }
                #endregion

                if (hit.transform.parent.TryGetComponent(out Node node))
                {
                    // node �� ���� ���� �տ� �ִ� ��� ���?
                    if (_stageGameRule.GetCurrentNode() == node.GetNodeDataSO())
                    {
                        isNodeClick = true;
                        currentNode = node;
                        NodeClick(currentNode);
                    }
                    else
                    {
                        // Ŭ�� ���� ��� ���� ��������
                        _stageGameRule.NodeSpawn();
                    }
                }
            }
            else
            {
                if (currentNode != null)
                    currentNode.NodeFalse();
                else
                    NodeFalse();

                // Ŭ�� ���� ��� ���� ��������
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
            _stageGameRule.AllNodeClear(); // �׳� ����������..
    }
}

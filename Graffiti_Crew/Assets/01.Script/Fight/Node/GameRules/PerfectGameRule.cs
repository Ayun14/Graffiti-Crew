using DG.Tweening;
using UnityEngine;

public class PerfectGameRule : StageGameRule
{
    //[SerializeField] private SliderValueSO _playerSliderValueSO;
    private Tween _playerSliderValueChangeTween;

    protected override void Awake()
    {
        base.Awake();

        //_playerSliderValueSO.value = _playerSliderValueSO.max;
    }

    public override void NodeClear()
    {
        _nodeSpawner.DequeueNode();

        if (_graffitiRenderer != null && _nodeJudgement.currentNode != null)
            _graffitiRenderer.SetSprite(_nodeJudgement.currentNode.GetNodeDataSO().graffitiSprite);
    }

    #region Combo

    public override void NodeFalse(Node node)
    {
        base.NodeFalse(node);

        //if (_playerSliderValueChangeTween != null && _playerSliderValueChangeTween.IsActive())
        //    _playerSliderValueChangeTween.Complete();

        //float targetValue = _playerSliderValueSO.Value - 10;
        //_playerSliderValueChangeTween = DOTween.To(() => _playerSliderValueSO.Value,
        //    x => _playerSliderValueSO.Value = x, targetValue, 0.1f);

        //if (targetValue <= 0)
        //    mySubject.ChangeGameState(GameState.Finish);
    }

    #endregion
}
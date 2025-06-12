using UnityEngine.Events;

public class TutorialNode : Observer<GameStateController>
{
    public UnityEvent OnNodeClear;

    private StageGameRule _stageGameRule;

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            if (mySubject.GameState == GameState.Tutorial)
            {
                _stageGameRule = GetComponent<StageGameRule>();
                _stageGameRule.OnNodeClear += HandleOnNodeClear;
            }
        }
    }

    private void OnDisable()
    {
        _stageGameRule.OnNodeClear -= HandleOnNodeClear;
    }

    private void HandleOnNodeClear()
    {
        OnNodeClear?.Invoke();
    }

    private void SetInput(bool isCanInput)
    {
        _stageGameRule.isTurotial = !isCanInput;
    }
}

using UnityEngine;
using UnityEngine.Events;

public class TutorialNode : Observer<GameStateController>
{
    [SerializeField] private DialogueUIController _dialogueUIController;
    public UnityEvent OnNodeClear;

    private StageGameRule _stageGameRule;

    private void Awake()
    {
        Attach();
    }

    private void OnDestroy()
    {
        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            if (mySubject.GameState == GameState.Tutorial)
            {
                _stageGameRule = GetComponent<StageGameRule>();
                _stageGameRule.OnNodeClear += HandleOnNodeClear;
            }
            if (mySubject.GameState == GameState.Dialogue)
            {
                _dialogueUIController.OnEndTyping += SetInput;
            }
        }
    }

    private void OnDisable()
    {
        _stageGameRule.OnNodeClear -= HandleOnNodeClear;
        _dialogueUIController.OnEndTyping -= SetInput;
    }

    private void HandleOnNodeClear()
    {
        OnNodeClear?.Invoke();
    }

    private void SetInput(bool isCanInput)
    {
        if (_stageGameRule == null) return;

        _stageGameRule.isTurotial = !isCanInput;
    }
}

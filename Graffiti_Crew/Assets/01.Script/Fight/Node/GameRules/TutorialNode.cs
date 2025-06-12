using UnityEngine;
using UnityEngine.Events;

public class TutorialNode : MonoBehaviour
{
    public UnityEvent OnNodeClear;

    private StageGameRule _stageGameRule;

    private void Start()
    {
        _stageGameRule = GetComponent<StageGameRule>();
        _stageGameRule.OnNodeClear += HandleOnNodeClear;
    }

    private void OnDisable()
    {
        _stageGameRule.OnNodeClear -= HandleOnNodeClear;
    }

    private void HandleOnNodeClear()
    {
        OnNodeClear?.Invoke();
    }
}

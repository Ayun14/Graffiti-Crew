using System.Collections.Generic;
using UnityEngine;

public class ComboController : MonoBehaviour
{
    [Header("Pool")]
    [SerializeField] private PoolManagerSO _poolManager;
    [SerializeField] private List<PoolTypeSO> _textParticles;

    private int _currentCombo = 0;

    protected StageGameRule _stageGameRule;
    private StageResultSO _stageResult;

    private bool _isCombo = false;

    public void Init(StageGameRule stageGameRule)
    {
        _stageGameRule = stageGameRule;
        _stageResult = stageGameRule.stageResult;
        _currentCombo = 0;
    }

    public void SuccessCombo(Vector3 particleSpawnPos)
    {
        if (_isCombo == false) _isCombo = true;

        ++_currentCombo;
        TextParticleSpawn(particleSpawnPos);

        //if (_stageGameRule.stageRuleType == StageRuleType.PerfectRule && _currentCombo > _stageResult.value)
        //    _stageResult.value = _currentCombo;
    }

    public void FailCombo()
    {
        _isCombo = false;
        _currentCombo = 0;
    }

    private void TextParticleSpawn(Vector3 particleSpawnPos)
    {
        int index = Mathf.Clamp(_currentCombo - 1, 0, _textParticles.Count - 1);
        PoolTypeSO poolType = _textParticles[index];

        GameObject textParticle = _poolManager.Pop(poolType).GameObject;
        textParticle.transform.position = particleSpawnPos;
    }
}

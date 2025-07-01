using System.Collections.Generic;
using UnityEngine;

public class ComboController : MonoBehaviour
{
    [Header("Pool")]
    [SerializeField] private PoolManagerSO _poolManager;
    [SerializeField] private List<PoolTypeSO> _textParticles;

    private int _currentCombo = 0;

    protected StageGameRule _stageGameRule;

    private bool _isCombo = false;

    public void Init(StageGameRule stageGameRule)
    {
        _stageGameRule = stageGameRule;
        _currentCombo = 0;
    }

    public void SuccessCombo(Vector3 particleSpawnPos)
    {
        if (_isCombo == false) _isCombo = true;

        ++_currentCombo;
        TextParticleSpawn(particleSpawnPos);
    }

    public void FailCombo()
    {
        _isCombo = false;
        _currentCombo = 0;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 3f;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;

        TextParticleSpawn(worldPos);
    }

    private void TextParticleSpawn(Vector3 particleSpawnPos)
    {
        int index = Mathf.Clamp(_currentCombo, 0, _textParticles.Count - 1);
        PoolTypeSO poolType = _textParticles[index];

        GameObject textParticle = _poolManager.Pop(poolType).GameObject;
        textParticle.transform.position = particleSpawnPos + new Vector3(0f, 0f, -0.2f);
    }
}

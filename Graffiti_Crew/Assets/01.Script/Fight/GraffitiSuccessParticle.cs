using System.Collections;
using UnityEngine;

public class GraffitiSuccessParticle : MonoBehaviour, IPoolable
{
    [Header("Pool")]
    [SerializeField] private PoolTypeSO _poolType;
    private Pool _pool;

    private ParticleSystem _particleSystem;
    private bool isReturning = false;

    #region Pool
    public PoolTypeSO PoolType => _poolType;
    public GameObject GameObject => gameObject;

    public void ResetItem()
    {
        isReturning = false;
        StartCoroutine(WaitForParticleEnd());
    }

    public void SetUpPool(Pool pool)
    {
        _pool = pool;
    }

    #endregion

    private void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    private IEnumerator WaitForParticleEnd()
    {
        yield return new WaitUntil(() => !_particleSystem.IsAlive(true));

        if (!isReturning)
        {
            isReturning = true;
            _pool.Push(this);
        }
    }
}

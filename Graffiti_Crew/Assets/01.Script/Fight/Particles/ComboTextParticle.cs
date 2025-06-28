using System.Collections;
using UnityEngine;

public class ComboTextParticle : MonoBehaviour, IPoolable
{
    private ParticleSystem _particleSystem;
    private bool isReturning = false;

    #region Pool

    [Header("Pool")]
    [SerializeField] private PoolTypeSO _poolType;
    private Pool _pool;

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
        _particleSystem = GetComponent<ParticleSystem>();
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

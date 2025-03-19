using System.Collections;
using UnityEngine;

public class GraffitiParticle : MonoBehaviour, IPoolable
{
    [SerializeField] private PoolTypeSO _poolTypeSO;
    private Pool _pool;

    private ParticleSystem _particleSystem;

    public PoolTypeSO PoolType => _poolTypeSO;
    public GameObject GameObject => gameObject;

    public void ResetItem() { }

    public void SetUpPool(Pool pool)
    {
        _pool = pool;
    }

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void ParticlePlay()
    {
        _particleSystem.Play();
        StartCoroutine(ParticleEndRoutine());
    }

    private IEnumerator ParticleEndRoutine()
    {
        yield return new WaitForSeconds(0.7f);
        _pool.Push(this);
    }
}

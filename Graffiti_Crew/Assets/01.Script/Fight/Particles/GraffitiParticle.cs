using System.Collections;
using UnityEngine;

public class GraffitiParticle : MonoBehaviour, IPoolable
{
    [SerializeField] private PoolTypeSO _poolTypeSO;
    private Pool _pool;

    private ParticleSystem _mainParticleSystem;
    private ParticleSystem _sideParticleSystem;
    private bool isReturning = false;

    public PoolTypeSO PoolType => _poolTypeSO;
    public GameObject GameObject => gameObject;

    public void ResetItem() { }

    public void SetUpPool(Pool pool)
    {
        _pool = pool;
    }

    private void Awake()
    {
        _mainParticleSystem = GetComponent<ParticleSystem>();
        _sideParticleSystem = transform.Find("Smoke").GetComponent<ParticleSystem>();
    }

    public void ParticlePlay(Color color)
    {
        ParticleSystem.MainModule main = _sideParticleSystem.main;
        main.startColor = color;

        _mainParticleSystem.Play();

        isReturning = false;
        StartCoroutine(WaitForParticleEnd());
    }

    private IEnumerator WaitForParticleEnd()
    {
        yield return new WaitUntil(() => !_sideParticleSystem.IsAlive(true));

        if (!isReturning)
        {
            isReturning = true;
            _pool.Push(this);
        }
    }
}

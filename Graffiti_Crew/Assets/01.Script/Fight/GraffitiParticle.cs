using UnityEngine;

public class GraffitiParticle : MonoBehaviour, IPoolable
{
    [SerializeField] private PoolTypeSO _poolTypeSO;
    private Pool _pool;

    public PoolTypeSO PoolType => _poolTypeSO;
    public GameObject GameObject => gameObject;

    public void ResetItem() { }

    public void SetUpPool(Pool pool)
    {
        _pool = pool;
    }

    private void OnDisable()
    {
        Debug.Log("Push Graffiti Particle");
        _pool.Push(this);
    }
}

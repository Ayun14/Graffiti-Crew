using UnityEngine;

public class SoundObject : MonoBehaviour, IPoolable
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

    public void PushObject()
    {
        _pool.Push(this);
    }

    private void OnDestroy()
    {
        PushObject();
    }
}


using UnityEngine;

public class CoinController : MonoBehaviour, IPoolable
{
    [Header("Coin")]
    private Material _coinMat;

    private void Awake()
    {
        _coinMat = transform.Find("Visual").GetComponent<Material>();
    }

    #region Pool
    [Header("Pool")]
    [SerializeField] private PoolTypeSO _poolType;
    private Pool _pool;

    public PoolTypeSO PoolType => _poolType;
    public GameObject GameObject => gameObject;

    public void ResetItem() { }

    public void SetUpPool(Pool pool) => _pool = pool;
    #endregion
}

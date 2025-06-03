using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour, IPoolable
{
    [Header("Coin")]
    [SerializeField] private List<GameObject> _coins = new();

    #region Pool
    [Header("Pool")]
    [SerializeField] private PoolTypeSO _poolType;
    private Pool _pool;

    public PoolTypeSO PoolType => _poolType;
    public GameObject GameObject => gameObject;

    public void ResetItem() 
    {
        int index = Random.Range(0, _coins.Count);
        for (int i = 0; i < _coins.Count; ++i)
            _coins[i].SetActive(i == index);
    }

    public void SetUpPool(Pool pool) => _pool = pool;
    #endregion
}

using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour, IPoolable
{
    [Header("Coin")]
    [SerializeField] private List<GameObject> _coins = new();

    [Header("Pool")]
    [SerializeField] private PoolTypeSO _sparksEffect;
    [SerializeField] private PoolManagerSO _poolManager;

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

    private void OnCollisionEnter(Collision collision)
    {
        GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Coin, 1f, true);
        GameObject effect = _poolManager.Pop(_sparksEffect).GameObject;
        effect.transform.position = collision.contacts[0].point;
    }
}

using DG.Tweening;
using UnityEngine;

public class SoundObject : MonoBehaviour, IPoolable
{
    [SerializeField] private PoolTypeSO _poolTypeSO;
    private Pool _pool;

    public PoolTypeSO PoolType => _poolTypeSO;
    public GameObject GameObject => gameObject;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void ResetItem() { }

    public void SetUpPool(Pool pool)
    {
        _pool = pool;
    }

    public void PushObject()
    {
        _audioSource.DOFade(0, 0.2f)
            .OnComplete(() => _pool.Push(this));
    }

    private void OnDestroy()
    {
        PushObject();
    }
}


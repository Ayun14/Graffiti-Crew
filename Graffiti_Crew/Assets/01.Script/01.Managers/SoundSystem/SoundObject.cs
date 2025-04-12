using DG.Tweening;
using System.Collections;
using UnityEngine;

public class SoundObject : MonoBehaviour, IPoolable
{
    [SerializeField] private PoolTypeSO _poolTypeSO;
    private Pool _pool;

    public PoolTypeSO PoolType => _poolTypeSO;
    public GameObject GameObject => gameObject;

    private AudioSource _audioSource;
    public AudioSource AudioSource => _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void ResetItem() { }

    public void SetUpPool(Pool pool)
    {
        _pool = pool;
    }

    public void PushObject(bool isLoop)
    {
        if (!gameObject.activeSelf) return;

        StartCoroutine(PushObjectRoutine(isLoop));
    }

    private IEnumerator PushObjectRoutine(bool isLoop)
    {
        if (!isLoop)
            yield return new WaitForSeconds(_audioSource.clip.length + 0.2f);

        _audioSource.DOFade(0, 0.2f)
            .OnComplete(() => _pool.Push(this));
    }
}


using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ReactionIcon : MonoBehaviour, IPoolable
{
    [Header("Pool")]
    [SerializeField] private PoolTypeSO _poolType;
    private Pool _pool;
    public PoolTypeSO PoolType => _poolType;
    public GameObject GameObject => gameObject;

    [Header("Sprite")]
    [SerializeField] private Image _iconImage;
    [SerializeField] private Sprite _goodIcon;
    [SerializeField] private Sprite _badIcon;

    public void ResetItem() { }

    public void SetUpPool(Pool pool)
    {
        _pool = pool;
    }

    public void SetIconSprite(bool isGood)
    {
        _iconImage.sprite = isGood ? _goodIcon : _badIcon;
    }

    public void PopIcon(float time)
    {
        GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.BubblePop);

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, time).SetEase(Ease.OutBack)
            .OnComplete(() => Invoke("PushObject", 1f));
    }

    private void PushObject()
    {
        transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutBack)
            .OnComplete(() => _pool.Push(this));
    }
}

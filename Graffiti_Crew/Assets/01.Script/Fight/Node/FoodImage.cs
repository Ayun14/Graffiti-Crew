using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FoodImage : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private RectTransform _canvasRect;

    [Header("Values")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _targetY = -550f;
    [SerializeField] private float _clickY = -20f; // 클릭하면 내려갈 y 값

    private Image _foodImage;
    private FightSceneUIController _uiController;

    private void Awake()
    {
        _foodImage = GetComponent<Image>();
        _foodImage.sprite = null;

        _uiController = GetComponentInParent<FightSceneUIController>();

        gameObject.SetActive(false);
    }

    private void Update()
    {
        _foodImage.rectTransform.anchoredPosition += Vector2.down * _moveSpeed * Time.deltaTime;

        if (_foodImage.rectTransform.anchoredPosition.y < _targetY)
        {
            _uiController.StartBlindRoutine(false);
        }
    }

    public void OnFoodSprite(Sprite sprite)
    {
        _foodImage.sprite = sprite;

        // 랜덤 위치
        float canvasWidth = _canvasRect.rect.width;
        float canvasHeight = _canvasRect.rect.height;

        float randomX = Random.Range(-canvasWidth / 2, canvasWidth / 2);
        float randomY = Random.Range(-canvasHeight / 2, canvasHeight / 2);

        _foodImage.rectTransform.anchoredPosition = new Vector2(randomX, randomY);

        _foodImage.gameObject.SetActive(true);
        _foodImage.transform.localScale = Vector3.zero;
        _foodImage.transform.DOScale(1, 0.7f);
    }

    public void OffFoodSprite()
    {
        _foodImage.transform.localScale = Vector3.one;
        _foodImage.transform.DOScale(0, 0.7f).OnComplete(() =>
            _foodImage.gameObject.SetActive(false));
    }

    public void OnSpriteClick()
    {
        _foodImage.rectTransform.DOAnchorPosY(_foodImage.rectTransform.localPosition.y + _clickY, 0.1f);
    }
}

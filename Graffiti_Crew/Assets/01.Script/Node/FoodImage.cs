using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FoodImage : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private RectTransform _canvasRect;

    [Header("Food Sprite")]
    [SerializeField] private Sprite _eggSprite;
    [SerializeField] private Sprite _tomatoSprite;
    [SerializeField] private float _moveSpeed;

    private Image _foodImage;
    private FightSceneUIController _uiController;

    private float _targetY = -600f;
    private float _clickY = -20f; // Ŭ���ϸ� ������ y ��

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

    public void OnFoodSprite()
    {
        Sprite food = null;
        if (Random.Range(0, 2) == 0)
            food = _eggSprite;
        else
            food = _tomatoSprite;
        _foodImage.sprite = food;

        // ���� ��ġ
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

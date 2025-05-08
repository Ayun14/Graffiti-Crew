using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FoodImage : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private RectTransform _canvasRect;

    private float _currentAlpha;
    private float _currentTime = 0;

    private Image _foodImage;
    private FightSceneUIController _uiController;

    private void Awake()
    {
        _foodImage = GetComponent<Image>();
        _foodImage.sprite = null;

        _uiController = GetComponentInParent<FightSceneUIController>();

        gameObject.SetActive(false);
    }

    public void OnFoodSprite(Sprite sprite)
    {
        _foodImage.sprite = sprite;

        // ·£´ý À§Ä¡
        float canvasWidth = _canvasRect.rect.width;
        float canvasHeight = _canvasRect.rect.height;

        float randomX = Random.Range(-canvasWidth / 2, canvasWidth / 2);
        float randomY = Random.Range(-canvasHeight / 2, canvasHeight / 2);

        _foodImage.rectTransform.anchoredPosition = new Vector2(randomX, randomY);

        _foodImage.gameObject.SetActive(true);
        _foodImage.transform.localScale = Vector3.zero;
        _foodImage.transform.DOScale(1, 0.7f);
        SetDoFade(1f, 0.7f);
    }


    private void SetAlpha(float alpha)
    {
        Color color = _foodImage.color;
        color.a = alpha;
        _foodImage.color = color;
    }

    public void SetDoFade(float endValue, float time)
    {
        float startValue = endValue == 1f ? 0f : 1f;
        SetAlpha(startValue);
        _foodImage.DOFade(endValue, time);
    }

    public void SetDoFade(float time)
    {
        StartCoroutine(SetAlphaRouine(time));
    }

    public void StopAllCoroutine() => StopAllCoroutines();

    private IEnumerator SetAlphaRouine(float time)
    {
        float waitTime = time / 7;
        yield return new WaitForSeconds(waitTime);

        time -= waitTime;
        _currentTime = 0;
        _currentAlpha = _foodImage.color.a;

        while (_currentTime < time)
        {
            _currentTime += Time.deltaTime;

            float alpha = Mathf.Lerp(_currentAlpha, 0f, _currentTime / time);
            SetAlpha(alpha);

            if (alpha <= 0f) break;

            yield return null;
        }
    }

    public void SetCurrentTime(float targetTime)
    {
        DOTween.To(() => _currentTime,
        x => _currentTime = x,
                   targetTime, 0.3f).SetEase(Ease.InOutQuad);
    }

    public void OnSpriteClick()
    {
        if (false == _uiController.isBlind) return;

        _uiController.BlindFastEvent();
    }
}

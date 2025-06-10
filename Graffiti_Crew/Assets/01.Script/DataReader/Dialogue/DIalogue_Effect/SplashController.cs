using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SplashController : MonoBehaviour
{
    [SerializeField] private Image _splashImage;

    [SerializeField] private Color _colorWhite;
    [SerializeField] private Color _colorBlack;

    [SerializeField] private float _fadeSpeed;
    [SerializeField] private float _fadeSlowSpeed;

    private bool _isFadeBlack = false;

    public bool isFinished = false;

    public IEnumerator Splash()
    {
        isFinished = false;
        yield return StartCoroutine(FadeOut(true, false));
        isFinished = false;
        yield return StartCoroutine(FadeIn(true, false));
    }

    public IEnumerator FadeOut(bool isWhite, bool isSlow)
    {
        if (_isFadeBlack) yield return null;

        Color color = isWhite ? _colorWhite : _colorBlack;
        color.a = 0;
        _splashImage.color = color;

        while (color.a < 1)
        {
            color.a += (isSlow ? _fadeSlowSpeed : _fadeSpeed) * Time.deltaTime;
            color.a = Mathf.Min(color.a, 1f);
            _splashImage.color = color;
            yield return null;
        }

        isFinished = true;
    }

    public IEnumerator FadeIn(bool isWhite, bool isSlow)
    {
        Color color = isWhite ? _colorWhite : _colorBlack;
        color.a = 1;
        _splashImage.color = color;

        while (color.a > 0)
        {
            color.a -= (isSlow ? _fadeSlowSpeed : _fadeSpeed) * Time.deltaTime;
            color.a = Mathf.Max(color.a, 0f);
            _splashImage.color = color;
            yield return null;
        }

        isFinished = true;
    }

    public void SetFade()
    {
        _isFadeBlack = true;
        _splashImage.color = _colorBlack;
    }
}

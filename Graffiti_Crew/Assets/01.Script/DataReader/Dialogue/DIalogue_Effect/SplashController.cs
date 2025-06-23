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

    public bool IsFading => _fadeRoutine != null;
    private Coroutine _fadeRoutine;
    public bool isFinished = false;

    private bool _isFadeBlack = false;

    public IEnumerator Splash()
    {
        if (IsFading) yield break;

        isFinished = false;
        yield return StartCoroutine(SafeFadeOut(true, false));
        isFinished = false;
        yield return StartCoroutine(SafeFadeIn(true, false));
    }

    public IEnumerator FadeOut(bool isWhite, bool isSlow)
    {
        yield return StartCoroutine(SafeFadeOut(isWhite, isSlow));
    }

    public IEnumerator FadeIn(bool isWhite, bool isSlow)
    {
        yield return StartCoroutine(SafeFadeIn(isWhite, isSlow));
    }

    private IEnumerator SafeFadeOut(bool isWhite, bool isSlow)
    {
        if (_fadeRoutine != null) yield break;

        _fadeRoutine = StartCoroutine(FadeRoutine(isWhite, isSlow, true));
        yield return _fadeRoutine;
        _fadeRoutine = null;
    }

    private IEnumerator SafeFadeIn(bool isWhite, bool isSlow)
    {
        if (_fadeRoutine != null) yield break;

        _fadeRoutine = StartCoroutine(FadeRoutine(isWhite, isSlow, false));
        yield return _fadeRoutine;
        _fadeRoutine = null;
    }

    private IEnumerator FadeRoutine(bool isWhite, bool isSlow, bool isOut)
    {
        isFinished = false;

        if (_isFadeBlack && isOut) yield break;

        Color color = isWhite ? _colorWhite : _colorBlack;
        color.a = isOut ? 0f : 1f;
        _splashImage.color = color;

        while (isOut ? color.a < 1f : color.a > 0f)
        {
            float delta = (isSlow ? _fadeSlowSpeed : _fadeSpeed) * Time.deltaTime;
            color.a += isOut ? delta : -delta;
            color.a = Mathf.Clamp01(color.a);
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

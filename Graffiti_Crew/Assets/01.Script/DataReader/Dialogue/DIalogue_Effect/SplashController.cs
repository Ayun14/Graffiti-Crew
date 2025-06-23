using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class SplashController : MonoBehaviour
{
    [SerializeField] private Image _splashImage;

    [SerializeField] private Color _colorWhite;
    [SerializeField] private Color _colorBlack;

    private float _fadeSpeed = 20f;
    private float _fadeSlowSpeed = 2f;

    public bool isFinished = false;

    private bool _isFadeBlack = false;

    public IEnumerator Splash()
    {
        isFinished = false;
        yield return StartCoroutine(FadeRoutine(true, false, true));
        isFinished = false;
        yield return StartCoroutine(FadeRoutine(true, false, false));
    }

    public IEnumerator FadeOut(bool isWhite, bool isSlow)
    {
        yield return StartCoroutine(FadeRoutine(isWhite, isSlow, true));

    }

    public IEnumerator FadeIn(bool isWhite, bool isSlow)
    {
        yield return StartCoroutine(FadeRoutine(isWhite, isSlow, false));
    }

    private IEnumerator FadeRoutine(bool isWhite, bool isSlow, bool isOut)
    {
        isFinished = false;

        Color targetColor = isWhite ? _colorWhite : _colorBlack;
        float targetAlpha = isOut ? 1f : 0f;

        Color currentColor = _splashImage.color;

        if (Mathf.Approximately(currentColor.a, targetAlpha) &&
            Mathf.Approximately(currentColor.r, targetColor.r) &&
            Mathf.Approximately(currentColor.g, targetColor.g) &&
            Mathf.Approximately(currentColor.b, targetColor.b))
        {
            yield return new WaitForSeconds(0.2f);
            isFinished = true;
            yield break;
        }

        targetColor.a = isOut ? 0f : 1f;
        _splashImage.color = targetColor;

        while (isOut ? targetColor.a < 1f : targetColor.a > 0f)
        {
            float delta = (isSlow ? _fadeSlowSpeed : _fadeSpeed) * Time.deltaTime;
            targetColor.a += isOut ? delta : -delta;
            targetColor.a = Mathf.Clamp01(targetColor.a);
            _splashImage.color = targetColor;
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

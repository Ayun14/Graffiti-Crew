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

    public static bool isfinished = true;

    public IEnumerator Splash()
    {
        isfinished = false;
        StartCoroutine(FadeOut(true, false));
        yield return new WaitUntil(() => isfinished);
        isfinished = false;
        StartCoroutine(FadeIn(true, false));
    }

    public IEnumerator FadeOut(bool isWhite, bool isSlow)
    {
        Color color = (isWhite == true) ? _colorWhite : _colorBlack;
        color.a = 0;

        _splashImage.color = color;

        while (color.a < 1)
        {
            color.a += (isSlow == true) ? _fadeSlowSpeed : _fadeSpeed;
            _splashImage.color = color;
            yield return null;
        }

        isfinished = true;
    }

    public IEnumerator FadeIn(bool isWhite, bool isSlow)
    {
        Color color = (isWhite == true) ? _colorWhite : _colorBlack;
        color.a = 1;

        _splashImage.color = color;

        while (color.a > 0)
        {
            color.a -= (isSlow == true) ? _fadeSlowSpeed : _fadeSpeed;
            _splashImage.color = color;
            yield return null;
        }

        isfinished = true;
    }
}

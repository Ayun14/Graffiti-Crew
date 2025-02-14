using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _fadeCanvas;
    [SerializeField] private float _fadeDuration = 0.5f;

    private void Start()
    {
        _fadeCanvas.alpha = 0f;
    }

    public void FadeIn()
    {
        _fadeCanvas.DOFade(1f, _fadeDuration).OnStart(() =>
        {
            _fadeCanvas.interactable = true;
            _fadeCanvas.blocksRaycasts = true;

        }).
        OnComplete(()=>
        {
            SceneManager.LoadScene("ComputerScene");
        });
    }
}

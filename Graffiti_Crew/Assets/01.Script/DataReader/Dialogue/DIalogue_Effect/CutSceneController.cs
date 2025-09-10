using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneController : MonoBehaviour
{
    public bool isFinished = false;

    private SplashController _splashController;

    [SerializeField] private Image _cutSceneImg;

    private void Awake()
    {
        _splashController = GetComponent<SplashController>();
    }

    public IEnumerator CutSceneRoutine(string cutSceneName, bool isShow)
    {
        

        if (isShow)
        {
            Sprite sprite = Resources.Load<Sprite>("Sprite/CutScene/" + cutSceneName);
            if (sprite != null)
            {
                Debug.Log("CutScene Fade Start");
                yield return StartCoroutine(Fade(false));
            }

            if (sprite != null)
            {
                _cutSceneImg.gameObject.SetActive(true);
                _cutSceneImg.sprite = sprite;
            }
            else
                Debug.Log("NO CutScene");
        }
        else
        {
            if(_cutSceneImg.sprite != null)
            {
                _cutSceneImg.gameObject.SetActive(false);
            }
        }

        if (_cutSceneImg.sprite != null)
        {
            yield return StartCoroutine(Fade(true));
            if(!isShow)
                _cutSceneImg.sprite = null;
        }

        isFinished = true;
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        _splashController.isFinished = false;

        if (isFadeIn)
            yield return _splashController.FadeIn(false, true);
        else
            yield return _splashController.FadeOut(false, true);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneController : MonoBehaviour
{
    public bool isFinished = false;

    [SerializeField] private SplashController _splashController;

    [SerializeField] private Image _cutSceneImg;

    public IEnumerator CutSceneRoutine(string cutSceneName, bool isShow)
    {
        Debug.Log(cutSceneName);
        _splashController.isFinished = false;
        yield return StartCoroutine(_splashController.FadeOut(false, true));

        if (isShow)
        {
            Sprite sprite = Resources.Load<Sprite>("Sprite/CutScene/" + cutSceneName);
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
            _cutSceneImg.gameObject.SetActive(false);
        }

        _splashController.isFinished = false;
        yield return StartCoroutine(_splashController.FadeIn(false, true));

        yield return new WaitForSeconds(0.5f);

        isFinished = true;
    }
}

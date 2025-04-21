using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneController : MonoBehaviour
{
    public static bool isFinished = false;

    [SerializeField] private SplashController _splashController;

    [SerializeField] private Image img_CutSscene;

    public bool CheckCutScene()
    {
        return img_CutSscene.gameObject.activeSelf;
    }

    public IEnumerator CutSceneRoutine(string p_CutSceneName, bool p_isShow)
    {
        SplashController.isfinished = false;
        StartCoroutine(_splashController.FadeOut(true, false));
        yield return new WaitUntil(() => SplashController.isfinished);

        if (p_isShow)
        {
            Sprite t_sprite = Resources.Load<Sprite>("CutScene/" + p_CutSceneName);
            if (t_sprite != null)
            {
                img_CutSscene.gameObject.SetActive(true);
                img_CutSscene.sprite = t_sprite;

            }
            else
            {
                Debug.Log("NO CutScene");
            }
        }
        else
        {
            img_CutSscene.gameObject.SetActive(false);
        }

        SplashController.isfinished = false;
        StartCoroutine(_splashController.FadeIn(true, false));
        yield return new WaitUntil(() => SplashController.isfinished);

        yield return new WaitForSeconds(0.5f);

        isFinished = true;
    }
}

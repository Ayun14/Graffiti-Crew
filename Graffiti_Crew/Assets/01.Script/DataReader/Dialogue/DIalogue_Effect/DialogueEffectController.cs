using System.Collections;
using UnityEngine;

public class DialogueEffectController : MonoBehaviour
{
    private SplashController _splashController;
    private CutSceneController _cutSceneController;

    private void Awake()
    {
        _splashController = GetComponentInChildren<SplashController>();
        _cutSceneController = GetComponentInChildren<CutSceneController>();
    }

    public IEnumerator SetBGType(DialogueData dialogueData)
    {
        switch (dialogueData.bgType)
        {
            case BGType.FadeIn:
                _splashController.isFinished = false;
                StartCoroutine(_splashController.FadeIn(false, true));
                yield return new WaitUntil(() => _splashController.isFinished);
                break;
            case BGType.FadeOut:
                _splashController.isFinished = false;
                StartCoroutine(_splashController.FadeOut(false, true));
                yield return new WaitUntil(() => _splashController.isFinished);
                break;
            case BGType.FlashIn:
                _splashController.isFinished = false;
                StartCoroutine(_splashController.Splash());
                yield return new WaitUntil(() => _splashController.isFinished);
                break;
            case BGType.FlashOut:
                _splashController.isFinished = false;
                StartCoroutine(_splashController.Splash());
                yield return new WaitUntil(() => _splashController.isFinished);
                break;
            case BGType.ShowCutScene:
                _cutSceneController.isFinished = false;
                StartCoroutine(_cutSceneController.CutSceneRoutine(_filteredDialogueList[_currentDialogueIndex].animName, true));
                yield return new WaitUntil(() => _cutSceneController.isFinished);
                break;
            case BGType.HideCutScene:
                _cutSceneController.isFinished = false;
                StartCoroutine(_cutSceneController.CutSceneRoutine(null, false));
                yield return new WaitUntil(() => _cutSceneController.isFinished);
                break;
        }
    }
}

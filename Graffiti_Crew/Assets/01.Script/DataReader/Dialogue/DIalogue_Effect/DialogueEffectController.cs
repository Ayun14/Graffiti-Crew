using System.Collections;
using UnityEngine;

public class DialogueEffectController : MonoBehaviour
{
    private DialogueUIController _dialogueController;
    private DialogueCameraController _camController;
    private SplashController _splashController;
    private CutSceneController _cutSceneController;

    private void Awake()
    {
        _dialogueController = GetComponent<DialogueUIController>();
        _camController = GetComponent<DialogueCameraController>();

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
                StartCoroutine(_cutSceneController.CutSceneRoutine
                    (_dialogueController.filteredDialogueList[_dialogueController.currentDialogueIndex].animName, true));
                yield return new WaitUntil(() => _cutSceneController.isFinished);
                break;
            case BGType.HideCutScene:
                _cutSceneController.isFinished = false;
                StartCoroutine(_cutSceneController.CutSceneRoutine(null, false));
                yield return new WaitUntil(() => _cutSceneController.isFinished);
                break;
            case BGType.ShakeCam:
                _camController.PlayCamEffect();
                break;
        }
    }
}

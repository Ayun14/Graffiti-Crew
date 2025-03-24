using AH.UI.Events;
using System;
using UnityEngine;

public class MiniDialougeController : MonoBehaviour
{
    [SerializeField] private DialogueUIController _dialogueUIController;
    [SerializeField] private NodeJudgement _nodeJudgement;

    [SerializeField] private int _explainIndex;
    private int _currentIndex;

    private AudioSource _bgm;

    private void Start()
    {
        _nodeJudgement.OnNodeSpawnStart += HandleNodeCheck;
        _currentIndex = _explainIndex;

        _bgm = GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Fight_After, true);
    }

    private void OnDestroy()
    {
        _bgm?.GetComponent<SoundObject>().PushObject();
        _nodeJudgement.OnNodeSpawnStart -= HandleNodeCheck;
    }

    private void HandleNodeCheck()
    {
        _dialogueUIController.ChangeDialogueUI?.Invoke(false);

        _dialogueUIController.StartDialogue(_currentIndex, _currentIndex, DialogueEnd);
        _currentIndex++;
    }

    private void DialogueEnd()
    {
        DialougeEvent.ShowMiniDialougeViewEvent?.Invoke(false);
    }
}

using AH.UI.Events;
using System;
using UnityEngine;

public class MiniDialougeController : MonoBehaviour
{
    [SerializeField] private DialogueUIController _dialogueUIController;
    [SerializeField] private NodeJudgement _nodeJudgement;

    [SerializeField] private int _explainIndex;
    private int _currentIndex;


    private void Start()
    {
        //_nodeJudgement.OnNodeSpawnStart += HandleNodeCheck;
        _currentIndex = _explainIndex;

    }

    private void OnDestroy()
    {
        //_nodeJudgement.OnNodeSpawnStart -= HandleNodeCheck;
    }

    private void HandleNodeCheck()
    {
        _dialogueUIController.ChangeDialogueUI?.Invoke(false);

        _dialogueUIController.StartDialogue(_currentIndex, _currentIndex, DialogueEnd);
        _currentIndex++;
    }

    private void DialogueEnd()
    {
        DialogueEvent.ShowMiniDialougeViewEvent?.Invoke(false);
    }
}

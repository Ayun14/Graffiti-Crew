using System;
using UnityEngine;

public class DialogueAnimController : MonoBehaviour
{
    [SerializeField] private DialogueController _dialogueController;
    [SerializeField] private AnimationManager _animManager;

    private int _animValue = 0;

    private void Start()
    {
        AnimationEvent.SetDialogueAnimation += HandleDialoguePlay;
        AnimationEvent.EndDialogueAnimation += HandleDialogueEnd;
    }

    private void OnDisable()
    {
        AnimationEvent.SetDialogueAnimation -= HandleDialoguePlay;
        AnimationEvent.EndDialogueAnimation -= HandleDialogueEnd;

    }

    private void HandleDialoguePlay(DialogueData dialogue)
    {
        _animValue = 0;

        switch (dialogue.characterName)
        {
            case "지아":
                _animValue = 1;
                break;
            case "엘라":
                _animValue = 2;
                break;
            case "비비안":
                _animValue = 3;
                break;
            case "":
                _animValue = 0;
                break;
        }

        if (_animValue != 0)
        {
            AnimationEvent.SetAnimation?.Invoke(_animValue,
                (AnimationEnum)Enum.Parse(typeof(AnimationEnum), dialogue.animName));
        }
    }


    private void HandleDialogueEnd()
    {
        AnimationEvent.SetAnimation?.Invoke(_animValue, AnimationEnum.Idle);
    }

}

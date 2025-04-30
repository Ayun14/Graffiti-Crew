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

        switch (dialogue.characterName)
        {
            case "Áö¾Æ":
                _animValue = 1;
                break;
            case "¿¤¶ó":
                _animValue = 2;
                break;
            case "ÀÇ·ÚÀÎ":
                _animValue = 100;
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

using AH.UI.Events;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerTalkState : PlayerState
{
    private bool _isDialogueFinished = false;

    public PlayerTalkState(Player player, PlayerStateMachine stateMachine, string animBoolName) 
        : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _isDialogueFinished = false;

        _player.MovementCompo.StopImmediately(true);

        if (_player.dialogueUIController != null)
        {
            _player.dialogueUIController.ChangeDialogueUI?.Invoke(true);
            _player.dialogueController.StartDialogue(_player.GetNPC().startIndex, _player.GetNPC().endIndex, OnDialogueComplete);
        }
    }

    public override void UpdateState()
    {
        if (_isDialogueFinished)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    private void OnDialogueComplete()
    {
        CheckCam();
        _player.GetNPC().tutorialCheck.data = true;
        
    }

    private async void CheckCam()
    {
        await Task.Delay(1500);
        _isDialogueFinished = true;
    }
}

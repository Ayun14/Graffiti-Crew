using AH.UI.Events;
using UnityEngine;

public class PlayerNPCState : PlayerState
{
    private bool _isDialogueFinished = false;

    public PlayerNPCState(Player player, PlayerStateMachine stateMachine, string animBoolName) 
        : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _isDialogueFinished = false;
        if (_player.CurrentInteractionObject != null)
            _player.CurrentInteractionObject.playerState = PlayerStateEnum.Interaction;

        _player.MovementCompo.StopImmediately(true);

        if (_player.dialogueUIController != null)
        {
            _player.dialogueUIController.ChangeDialogueUI?.Invoke(true);
            _player.dialogueUIController.StartDialogue(_player.GetNPC().startIndex, _player.GetNPC().endIndex, OnDialogueComplete);
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
        _isDialogueFinished = true;
    }
}

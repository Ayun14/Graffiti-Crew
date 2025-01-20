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
        _player.MovementCompo.StopImmediately(true);

        if (_player.dialogueUIController != null)
        {
            _player.dialogueUIController.StartDialogue
                (_player.GetNPC().startIndex, _player.GetNPC().endIndex, OnDialogueComplete);
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

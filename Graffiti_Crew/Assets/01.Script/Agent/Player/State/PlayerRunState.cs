using System;
using UnityEngine;

public class PlayerRunState : PlayerState
{
    public PlayerRunState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //_player.PlayerVFXCompo.UpdateFootStep(true);
        _player.PlayerInput.MovementEvent += HandleMovementEvent;
    }

    public override void UpdateState()
    {
        if(!_player.MovementCompo.CheckDistance())
        {
            _player.StateMachine.ChangeState(PlayerStateEnum.Idle);
        }
        base.UpdateState();
    }

    public override void Exit()
    {
        _player.PlayerInput.MovementEvent -= HandleMovementEvent;
        base.Exit();
    }

    private void HandleMovementEvent(Vector3 movement)
    {
        _player.MovementCompo.SetDestination(movement);
    }
}

using System;
using UnityEngine;

public class PlayerRunState : PlayerState
{
    public PlayerRunState(Player player, PlayerStateMachine stateMachine, string animBoolName) 
        : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.MovementCompo.StopImmediately(false);

        _player.MovementCompo.SetDestination(_player.NavMeshAgent.destination);
        _player.PlayerInput.MovementEvent += HandleMovementEvent;

        //_player.PlayerVFXCompo.UpdateFootStep(true);

    }

    public override void UpdateState()
    {
        base.UpdateState();
        if(_player.MovementCompo.CanMoveCheck())
        {
            _player.StateMachine.ChangeState(PlayerStateEnum.Idle);
        }
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

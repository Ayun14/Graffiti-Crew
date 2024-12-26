using System;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animBoolName) 
        : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.MovementEvent += HandleMovementEvent;
        _player.MovementCompo.StopImmediately(true);
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void Exit()
    {
        _player.PlayerInput.MovementEvent -= HandleMovementEvent;
        base.Exit();
    }

    private void HandleMovementEvent(Vector3 movement)
    {
        _player.NavMeshAgent.destination = movement;
        _player.StateMachine.ChangeState(PlayerStateEnum.Run);
    }

}

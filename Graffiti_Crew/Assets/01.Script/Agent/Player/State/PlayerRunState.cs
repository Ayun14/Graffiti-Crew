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
        GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Walk);
        _player.MovementCompo.StopImmediately(false);
        _player.MovementCompo.SetDestination(_player.NavMeshAgent.destination);

        _player.PlayerInput.MovementEvent += HandleMovementEvent;
        _player.PlayerInput.InteractionEvent += HandleInteractionEvent;

        //_player.PlayerVFXCompo.UpdateFootStep(true);
    }

    public override void UpdateState()
    {
        if(_player.MovementCompo.CanMoveCheck())
        {
            _player.StateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    public override void Exit()
    {
        GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Walk);

        _player.PlayerInput.MovementEvent -= HandleMovementEvent;
        _player.PlayerInput.InteractionEvent -= HandleInteractionEvent;

        base.Exit();
    }

    private void HandleInteractionEvent(InteractionObject interactionObject)
    {
        _player.CurrentInteractionObject = interactionObject;
        _player.NavMeshAgent.destination = interactionObject.TargetPos;
        _player.StateMachine.ChangeState(PlayerStateEnum.Interaction);
    }

    private void HandleMovementEvent(Vector3 movement)
    {
        _player.MovementCompo.SetDestination(movement);
    }
}

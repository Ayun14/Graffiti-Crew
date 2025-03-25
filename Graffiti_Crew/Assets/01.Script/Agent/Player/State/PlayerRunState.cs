using System;
using UnityEngine;

public class PlayerRunState : PlayerState
{
    private AudioSource _walkSound;
    public PlayerRunState(Player player, PlayerStateMachine stateMachine, string animBoolName) 
        : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _walkSound = GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Walk, true);
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
        _walkSound?.GetComponent<SoundObject>().PushObject();
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

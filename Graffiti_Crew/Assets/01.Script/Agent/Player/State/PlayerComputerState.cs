using UnityEngine;

public class PlayerComputerState : PlayerState
{
    public PlayerComputerState(Player player, PlayerStateMachine stateMachine, string animBoolName) 
        : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _player.PlayerInput.MovementEvent += HandleMovementEvent;
        _player.PlayerInput.InteractionEvent += HandleInteractionEvent;

        _player.MovementCompo.StopImmediately(true);
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void Exit()
    {
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
        _player.NavMeshAgent.destination = movement;
        _player.StateMachine.ChangeState(PlayerStateEnum.Run);
    }
}

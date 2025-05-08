using UnityEngine;

public class PlayerSitStayState : PlayerState
{
    public PlayerSitStayState(Player player, PlayerStateMachine stateMachine, string animBoolName)
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

    }

    public override void Exit()
    {
        _player.PlayerInput.MovementEvent -= HandleMovementEvent;

        base.Exit();
    }

    private void HandleMovementEvent(Vector3 movement)
    {
        _player.AnimatorCompo.speed = 1;
        _player.NavMeshAgent.destination = movement;

        _player.StateMachine.ChangeState(PlayerStateEnum.Sit);
    }
}

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

        _player.playerData.playerPosition = _player.transform.position;
        _player.MovementCompo.StopImmediately(true);
        _player.computerTimeline.Play();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void Exit()
    {
        base.Exit();
    }
}

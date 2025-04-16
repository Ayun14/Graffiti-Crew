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
        GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Typing);

        _player.MovementCompo.StopImmediately(true);
        _player.playerData.playerPosition = _player.transform.position;
        _player.playerData.playerRotation = _player.transform.rotation;
        _player.CurrentInteractionObject?.GetComponent<Computer>().ComputerEvent();
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

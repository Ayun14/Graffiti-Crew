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
        GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Typing);

        if (_player.CurrentInteractionObject != null)
            _player.CurrentInteractionObject.playerState = PlayerStateEnum.Computer;

        _player.MovementCompo.StopImmediately(true);
        _player.playerData.playerPosition = _player.transform.position;
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

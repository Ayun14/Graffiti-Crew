using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerSitState : PlayerState
{
    private bool _onInteraction = false;
    private bool _onRun = false;

    public PlayerSitState(Player player, PlayerStateMachine stateMachine, string animBoolName) 
        : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _onInteraction = true;
        _onRun = true;

        Animation_Go();

        _player.PlayerInput.MovementEvent += HandleMovementEvent;

        _player.MovementCompo.StopImmediately(true);
    }

    public override void UpdateState()
    {
        float animTime = _player.AnimatorCompo.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (_onInteraction)
        {
            if (animTime >= 1.0f || animTime == 0)
            {
                _player.StateMachine.ChangeState(PlayerStateEnum.Interaction);
            }
        }

        if (_onRun)
        {
            if (animTime >= 1.0f || animTime == 0)
            {
                _player.StateMachine.ChangeState(PlayerStateEnum.Run);
            }
        }
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

        Animation_Reverse();
        _onRun = true;
        //_player.StateMachine.ChangeState(PlayerStateEnum.Run);
    }

    private void Animation_Go()
    {
        _player.AnimatorCompo.SetFloat("Reverse", 1);
        //_player.AnimatorCompo.Play("Sit");
    }

    private void Animation_Reverse()
    {
        _player.AnimatorCompo.SetFloat("Reverse", -1);
        _player.AnimatorCompo.Play("Sit");
    }
}

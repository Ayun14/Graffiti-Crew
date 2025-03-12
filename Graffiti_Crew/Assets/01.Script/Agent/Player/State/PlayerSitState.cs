using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerSitState : PlayerState
{
    private bool _onRun = false;

    public PlayerSitState(Player player, PlayerStateMachine stateMachine, string animBoolName) 
        : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _onRun = false;

        if (_player.NavMeshAgent.destination == _player.CurrentInteractionObject.TargetPos)
            Animation_Go();
        else
            Animation_Reverse();

        _player.PlayerInput.MovementEvent += HandleMovementEvent;
        _player.MovementCompo.StopImmediately(true);
    }

    public override void UpdateState()
    {
        float animTime = _player.AnimatorCompo.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (_onRun)
        {
            if (animTime >= 1.0f || animTime == 0)
            {
                _player.StateMachine.ChangeState(PlayerStateEnum.Run);
            }
        }

        if(animTime == 1.0f)
            _player.StateMachine.ChangeState(PlayerStateEnum.SitStay);
    }

    public override void Exit()
    {
        _player.PlayerInput.MovementEvent -= HandleMovementEvent;

        base.Exit();
    }

    private void HandleMovementEvent(Vector3 movement)
    {
        _player.NavMeshAgent.destination = movement;

        Animation_Reverse();
        _onRun = true;
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

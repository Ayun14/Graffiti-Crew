using UnityEngine;

public class PlayerInteractionState : PlayerState
{
    private AudioSource _walkSound;

    public PlayerInteractionState(Player player, PlayerStateMachine stateMachine, string animBoolName) 
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
    }

    public override void UpdateState()
    {


        if (_player.MovementCompo.CanMoveCheck())
        {
            Vector3 target = _player.CurrentInteractionObject.transform.position;

            if (_player.CurrentInteractionObject != null)
            {
                Quaternion targetRotation;
                target.y = _player.transform.position.y;

                if (_player.CurrentInteractionObject.stateEnum == PlayerStateEnum.Sit
                    || _player.CurrentInteractionObject.stateEnum == PlayerStateEnum.Computer)
                {
                    targetRotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    targetRotation = Quaternion.LookRotation(target - _player.transform.position);
                }

                _player.transform.rotation = Quaternion.Slerp(
                    _player.transform.rotation,
                    targetRotation,
                    Time.deltaTime * 5f
                );
            }

            if (_player.CurrentInteractionObject.stateEnum == PlayerStateEnum.Sit
                || _player.CurrentInteractionObject.stateEnum == PlayerStateEnum.Computer)
            {
                if(Quaternion.Angle(_player.transform.rotation,Quaternion.Euler(0,0,0)) < 10f)
                    _player.StateMachine.ChangeState(_player.CurrentInteractionObject.stateEnum);
            }
            else
            {
                if (Quaternion.Angle(_player.transform.rotation,
                    Quaternion.LookRotation(target - _player.transform.position)) < 10f)
                    _player.StateMachine.ChangeState(_player.CurrentInteractionObject.stateEnum);
            }
        }
    }

    public override void Exit()
    {
        _walkSound?.GetComponent<SoundObject>().PushObject(false);

        _player.PlayerInput.MovementEvent -= HandleMovementEvent;
        _player.PlayerInput.InteractionEvent -= HandleInteractionEvent;

        base.Exit();
    }

    private void HandleInteractionEvent(InteractionObject interactionObject)
    {
        _player.CurrentInteractionObject = interactionObject;
        _player.NavMeshAgent.SetDestination(interactionObject.TargetPos);
    }

    private void HandleMovementEvent(Vector3 movement)
    {
        _player.NavMeshAgent.destination = movement;
        _player.StateMachine.ChangeState(PlayerStateEnum.Run);
    }
}

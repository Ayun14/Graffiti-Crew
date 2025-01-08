using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void AnimationEnd()
    {
        _player.StateMachine.CurrentState.AnimationFinishTrigger();
    }
}

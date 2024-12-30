using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void AnimationEnd()
    {
        _player.StateMachine.CurrentState.AnimationFinishTrigger();
    }

    private void PlayBladeVFX()
    {
        _player.PlayBladeVFX();
    }
}

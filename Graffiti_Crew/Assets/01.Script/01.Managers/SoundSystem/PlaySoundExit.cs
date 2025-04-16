using UnityEngine;
public class PlaySoundExit : StateMachineBehaviour
{
    [SerializeField] private SoundType sound;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.Instance.SoundSystemCompo.PlaySFX(sound);
    }
}
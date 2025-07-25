using UnityEngine;

public class FightScenePlayerController : Observer<GameStateController>
{
    [SerializeField] private ParticleSystem _resultParticle;

    private Transform _player;
    private Transform _resultTrm;

    private void Awake()
    {
        Attach();

        _player = transform.Find("PlayerAnim").GetComponent<Transform>();
        _resultTrm = transform.Find("ResultPos").GetComponent<Transform>();
    }

    private void OnDestroy()
    {
        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            if (mySubject.GameState == GameState.Timeline)
            {
                AnimationEvent.SetAnimation?.Invoke(1, AnimationEnum.Ready);

                _player.position = _resultTrm.position;
                _player.localRotation = _resultTrm.localRotation;
            }
            else if (mySubject.GameState == GameState.Finish)
            {
                AnimationEvent.SetAnimation?.Invoke(1, AnimationEnum.Idle);
            }
        }
    }

    public void WaitAnimation()
    {
        AnimationEvent.SetAnimation?.Invoke(1, AnimationEnum.Idle);
    }

    public void WinLoseAnimation()
    {
        if (mySubject.IsPlayerWin) {
            AnimationEvent.SetAnimation?.Invoke(1, AnimationEnum.Win);
            _resultParticle.Play();
            UIAnimationEvent.SetPlayerBackgroundColor?.Invoke();
        }
        else {
            UIAnimationEvent.SetRivalBackgroundColor?.Invoke();
            AnimationEvent.SetAnimation?.Invoke(1, AnimationEnum.Lose);
        }
    }
}

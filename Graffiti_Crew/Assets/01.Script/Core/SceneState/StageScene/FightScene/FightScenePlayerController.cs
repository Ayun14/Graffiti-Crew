using UnityEngine;

public class FightScenePlayerController : Observer<GameStateController>
{
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
        }
    }

    public void WaitAnimation()
    {
        AnimationEvent.SetAnimation?.Invoke(1, AnimationEnum.Idle);
    }

    public void WinLoseAnimation()
    {
        Color winColor = new Color(241, 255, 76, 255); // yellow

        if (mySubject.IsPlayerWin) {
            AnimationEvent.SetAnimation?.Invoke(1, AnimationEnum.Win);
            UIAnimationEvent.SetPlayerBackgroundColor?.Invoke(winColor);
        }
        else {
            UIAnimationEvent.SetRivalBackgroundColor?.Invoke(winColor);
            AnimationEvent.SetAnimation?.Invoke(1, AnimationEnum.Lose);
        }
    }
}

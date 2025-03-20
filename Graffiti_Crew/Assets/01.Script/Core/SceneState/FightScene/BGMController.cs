using UnityEngine;

public class BGMController : Observer<GameStateController>
{
    private SoundObject _fightBeforeSoundObj;
    private SoundObject _fightMiddleSoundObj;
    private SoundObject _fightAfterSoundObj;

    private void Awake()
    {
        Attach();
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
                _fightBeforeSoundObj = SoundManager.Instance.PlaySound(SoundType.Fight_Before, true)
                    .GetComponent<SoundObject>();
            }
            else if (mySubject.GameState == GameState.Fight)
            {
                _fightMiddleSoundObj = SoundManager.Instance.PlaySound(SoundType.Fight_Middle, true)
                    .GetComponent<SoundObject>();
            }
            else if (mySubject.GameState == GameState.Finish)
            {
                _fightMiddleSoundObj?.PushObject();
            }
            else if (mySubject.GameState == GameState.Result)
            {
                _fightAfterSoundObj = SoundManager.Instance.PlaySound(SoundType.Fight_After, true)
                    .GetComponent<SoundObject>();
            }
        }
    }

    private void OnDisable()
    {
        _fightAfterSoundObj?.PushObject();
    }

    public void FightBeforeBGMStop()
    {
        _fightBeforeSoundObj?.PushObject();
    }
}

using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BGMController : Observer<GameStateController>
{
    [SerializeField] private bool _isFightScene = false;

    private AudioSource _fightMiddleAudioSource;

    private void Awake()
    {
        Attach();

        mySubject.OnRivalCheckEvent += HandleRivalCheckEvent;
    }

    private void OnDisable()
    {
        mySubject.OnRivalCheckEvent -= HandleRivalCheckEvent;

        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            if (mySubject.GameState == GameState.Timeline)
            {
                GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Fight_Before);
            }
            else if (mySubject.GameState == GameState.Countdown)
            {
                GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Fight_Before);
            }
            else if (mySubject.GameState == GameState.Fight)
            {
                GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Fight_Middle);
            }
            else if (mySubject.GameState == GameState.Finish)
            {
                GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Fight_Middle);
                SoundType type = _isFightScene ? SoundType.Fight_Result : SoundType.Fight_After;
                GameManager.Instance.SoundSystemCompo.PlayBGM(type);
                GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.DJ_Sound);
            }
        }
    }

    public void FightBeforeBGMStop()
    {
        GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Fight_Before);
    }

    private void HandleRivalCheckEvent()
    {
        if (_fightMiddleAudioSource == null) return;
        _fightMiddleAudioSource.pitch = 1.1f;
    }

    public void StartCountdownSound() => StartCoroutine(CountDownRoutine());

    private IEnumerator CountDownRoutine()
    {
        GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.DJ_Sound);
        yield return new WaitForSeconds(1f);
        GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.DJ_Sound);
        yield return new WaitForSeconds(1f);
        GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.DJ_Sound);
        yield return new WaitForSeconds(1f);
        GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.DJ_Yeah);
        GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.DJ_Sound);
    }

    public void PlayResultSound()
    {
        SoundType soundType = mySubject.IsPlayerWin ? SoundType.Win : SoundType.Lose;
        GameManager.Instance.SoundSystemCompo.PlaySFX(soundType);

        GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Drum_Roll);
        GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Fight_After);
    }

    public void PlayPoliceBGM()
    {
        StartCoroutine(PoliceBGMRoutine());
    }

    private IEnumerator PoliceBGMRoutine()
    {
        AudioSource source = GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Police);
        yield return new WaitForSeconds(4.5f);
        source.DOFade(0, 1.8f).OnComplete(() => GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Police));
    }

    public void PlaySpraySFX()
    {
        GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Spray_NoneGas);
    }
}

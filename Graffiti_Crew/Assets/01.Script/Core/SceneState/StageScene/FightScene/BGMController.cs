using DG.Tweening;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BGMController : Observer<GameStateController>
{
    private AudioSource _fightMiddleAudioSource;

    private void Awake()
    {
        Attach();

        mySubject.OnRivalCheckEvent += HandleRivalCheckEvent;
    }

    private void OnDestroy()
    {
        GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Fight_After);

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

            if (mySubject.GameState == GameState.Finish)
            {
                GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Fight_Middle);
                GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Fight_After);
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

    #region SFX Play

    public void CountDownSound()
    {
        StartCoroutine(CountDownRoutine());
    }

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

        _fightMiddleAudioSource = GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Fight_Middle);
    }

    #endregion
}

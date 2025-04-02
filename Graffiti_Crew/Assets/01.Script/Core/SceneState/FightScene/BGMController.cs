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
        GameManager.Instance.SoundSystemCompo.StopLoopSound(SoundType.Fight_After);

        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            if (mySubject.GameState == GameState.Timeline)
            {
                GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Fight_Before, true);
            }

            if (mySubject.GameState == GameState.Finish)
            {
                GameManager.Instance.SoundSystemCompo.StopLoopSound(SoundType.Fight_Middle);
                GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.DJ_Sound);
            }

            if (mySubject.GameState == GameState.Result)
            {
                GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Fight_After, true);
            }
        }
    }

    public void FightBeforeBGMStop()
    {
        GameManager.Instance.SoundSystemCompo.StopLoopSound(SoundType.Fight_Before);
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
        GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.DJ_Sound);
        yield return new WaitForSeconds(1f);
        GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.DJ_Sound);
        yield return new WaitForSeconds(1f);
        GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.DJ_Sound);
        yield return new WaitForSeconds(1f);
        GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.DJ_Yeah);
        GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.DJ_Sound);

        _fightMiddleAudioSource = GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Fight_Middle, true);
    }

    #endregion
}

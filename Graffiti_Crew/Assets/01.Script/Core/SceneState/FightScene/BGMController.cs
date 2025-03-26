using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class BGMController : Observer<GameStateController>
{
    private AudioSource _fightBeforeAudioSource;
    private AudioSource _fightMiddleAudioSource;
    private AudioSource _fightAfterAudioSource;

    private void Awake()
    {
        Attach();

        mySubject.OnRivalCheckEvent += HandleRivalCheckEvent;
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
                _fightBeforeAudioSource = GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Fight_Before, true);
            }

            if (mySubject.GameState == GameState.Finish)
            {
                _fightMiddleAudioSource?.GetComponent<SoundObject>().PushObject(true);

                GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.DJ_Sound); 
                _fightAfterAudioSource = GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Fight_After, true);
            }

            if (mySubject.GameState == GameState.Result)
            {
                //_fightAfterAudioSource = GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Fight_After, true);
            }
        }
    }

    private void OnDisable()
    {
        _fightAfterAudioSource?.GetComponent<SoundObject>().PushObject(true);
    }

    public void FightBeforeBGMStop()
    {
        _fightBeforeAudioSource.DOFade(0, 0.8f)
            .OnComplete(() => _fightBeforeAudioSource?.GetComponent<SoundObject>().PushObject(true));
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

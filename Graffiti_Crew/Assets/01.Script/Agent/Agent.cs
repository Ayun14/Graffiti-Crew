using System;
using System.Collections;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    #region ÄÞÆ÷³ÍÆ® ¿µ¿ª
    public Animator AnimatorCompo { get; protected set; }
    public INavigationable MovementCompo { get; protected set; }
    public AgentVFX VFXCompo { get; protected set; }
    #endregion

    public bool CanStateChangeable { get; protected set; } = true;
    public bool isDead;

    protected virtual void Awake()
    {
        Transform visualTrm = transform.Find("Visual");
        AnimatorCompo = visualTrm.GetComponent<Animator>();
        MovementCompo = GetComponent<INavigationable>();

        //VFXCompo = transform.Find("AgentVFX").GetComponent<AgentVFX>();
    }

    #region Delay Callback coroutine 
    public Coroutine StartDelayCallback(float delayTime, Action Callback)
    {
        return StartCoroutine(DelayCoroutine(delayTime, Callback));
    }
    protected IEnumerator DelayCoroutine(float delayTime, Action Callback)
    {
        yield return new WaitForSeconds(delayTime);
        Callback?.Invoke();
    }
    #endregion
}

using UnityEngine;

public class NPCState
{
    protected NPCStateMachine _stateMachine;
    protected NPCController _npc;

    protected int _animBoolHash;
    protected bool _endTriggerCalled;

    public NPCState(NPCController npc, NPCStateMachine stateMachine, string animBoolName)
    {
        _npc = npc;
        _stateMachine = stateMachine;
        _animBoolHash = Animator.StringToHash(animBoolName);
    }

    public virtual void Enter()
    {
        _npc.AnimatorCompo.SetBool(_animBoolHash, true);
        _endTriggerCalled = false;
    }

    public virtual void Exit()
    {
        _npc.AnimatorCompo.SetBool(_animBoolHash, false);
    }

    public virtual void UpdateState()
    {

    }

    public virtual void AnimationFinishTrigger()
    {
        _endTriggerCalled = true;
    }
}

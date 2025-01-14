using UnityEngine;

public class NPCIdleState : NPCState
{
    public NPCIdleState(NPCController npc, NPCStateMachine stateMachine, string animBoolName) 
        : base(npc, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateState()
    {
        if (_npc.player.StateMachine.stateDictionary[PlayerStateEnum.NPC] == _npc.player.StateMachine.CurrentState)
        {
            _stateMachine.ChangeState(NPCStateEnum.Talk);
        }
    }
}

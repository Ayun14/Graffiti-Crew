using UnityEngine;

public class NPCTalkState : NPCState
{
    public NPCTalkState(NPCController npc, NPCStateMachine stateMachine, string animBoolName) 
        : base(npc, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateState()
    {
        if (_npc.player.CurrentInteractionObject == _npc)
        {
            if (_npc.player.StateMachine.stateDictionary[PlayerStateEnum.NPC] != _npc.player.StateMachine.CurrentState)
            {
                _stateMachine.ChangeState(NPCStateEnum.Idle);
            }
        }
    }
}

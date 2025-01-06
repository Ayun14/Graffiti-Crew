using System.Collections.Generic;

public class NPCStateMachine
{
    public NPCState CurrentState { get; private set; }
    public Dictionary<NPCStateEnum, NPCState> stateDictionary;

    private NPCController _npc;

    public NPCStateMachine()
    {
        stateDictionary = new Dictionary<NPCStateEnum, NPCState>();
    }

    public void Initialize(NPCStateEnum startState, NPCController npc)
    {
        _npc = npc;
        CurrentState = stateDictionary[startState];
        CurrentState.Enter();
    }

    public void ChangeState(NPCStateEnum newState)
    {
        if (_npc.CanStateChangeable == false) return;

        CurrentState.Exit();
        CurrentState = stateDictionary[newState];
        CurrentState.Enter();
    }

    public void AddState(NPCStateEnum stateEnum, NPCState state)
    {
        stateDictionary.Add(stateEnum, state);
    }
}

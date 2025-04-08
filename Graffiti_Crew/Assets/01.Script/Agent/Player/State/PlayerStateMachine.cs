using System.Collections.Generic;

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }
    public PlayerStateEnum CurrentStateEnum { get; private set; }
    public Dictionary<PlayerStateEnum, PlayerState> stateDictionary;

    private Player _player;

    public PlayerStateMachine()
    {
        stateDictionary = new Dictionary<PlayerStateEnum, PlayerState>();
    }

    public void Initialize(PlayerStateEnum startState, Player player)
    {
        _player = player;
        CurrentStateEnum = startState;
        CurrentState = stateDictionary[startState];
        CurrentState.Enter();
    }

    public void ChangeState(PlayerStateEnum newState)
    {
        if (_player.CanStateChangeable == false) return;

        CurrentState.Exit();
        CurrentStateEnum = newState;
        CurrentState = stateDictionary[newState];
        CurrentState.Enter();
    }

    public void AddState(PlayerStateEnum stateEnum, PlayerState state)
    {
        stateDictionary.Add(stateEnum, state);
    }

}

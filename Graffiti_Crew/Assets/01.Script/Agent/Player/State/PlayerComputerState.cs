using UnityEngine;

public class PlayerComputerState : PlayerState
{
    public PlayerComputerState(Player player, PlayerStateMachine stateMachine, string animBoolName) 
        : base(player, stateMachine, animBoolName)
    {
    }
}

using UnityEngine;

public class PlayerNPCState : PlayerState
{
    public PlayerNPCState(Player player, PlayerStateMachine stateMachine, string animBoolName) 
        : base(player, stateMachine, animBoolName)
    {
    }
}

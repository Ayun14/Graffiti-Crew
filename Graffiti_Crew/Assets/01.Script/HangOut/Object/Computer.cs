using UnityEngine;
using AH.SaveSystem;

public class Computer : InteractionObject
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void ComputerSignal()
    {
        //PresentationEvents.FadeInOutEvent?.Invoke(false);
    }
}

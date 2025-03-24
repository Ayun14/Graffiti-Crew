using UnityEngine;
using AH.SaveSystem;
using System.Threading.Tasks;

public class Computer : InteractionObject
{
    protected override void Awake()
    {
        base.Awake();
    }

    private async void Start()
    {
        PresentationEvents.SetFadeEvent?.Invoke(true);
        await Task.Delay(1100);
        PresentationEvents.FadeInOutEvent?.Invoke(true);
    }

    public async void ComputerSignal()
    {
        PresentationEvents.FadeInOutEvent?.Invoke(false);
        await Task.Delay(1100);
    }
}

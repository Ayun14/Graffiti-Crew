using UnityEngine;

public class Computer : InteractionObject
{
    [SerializeField] private FadeController _fadeController;

    protected override void Awake()
    {
        base.Awake();
    }

    public void ComputerSignal()
    {
        _fadeController.FadeIn();
    }
}

using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFX : AgentVFX
{
    [SerializeField] private VisualEffect _footStep;

    public void UpdateFootStep(bool state)
    {
        if (state)
            _footStep.Play();
        else
            _footStep.Stop();
    }
}

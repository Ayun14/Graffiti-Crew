using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFX : AgentVFX
{
    [SerializeField] private ParticleSystem[] _bladeParticles;
    [SerializeField] private VisualEffect _footStep;
    [SerializeField] private ParticleSystem _collectParticle;

    public void PlayCollectVFX()
    {
        _collectParticle.Play();
    }

    public void PlayBladeVFX(int comboIndex)
    {
        _bladeParticles[comboIndex].Play();
    }

    public void StopBladeVFX()
    {
        foreach (ParticleSystem p in _bladeParticles)
        {
            p.Stop();
        }
    }

    public void UpdateFootStep(bool state)
    {
        if (state)
            _footStep.Play();
        else
            _footStep.Stop();
    }
}

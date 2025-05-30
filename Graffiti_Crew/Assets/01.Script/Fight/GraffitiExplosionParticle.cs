using System.Collections.Generic;
using UnityEngine;

public class GraffitiExplosionParticle : Observer<GameStateController>
{
    [SerializeField] private List<ParticleSystem> _particleList = new();

    private void Awake()
    {
        Attach();
    }

    private void OnDestroy()
    {
        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            if (mySubject.GameState == GameState.Finish)
                PlayGraffitiExplosionParticle();
        }
    }

    private void PlayGraffitiExplosionParticle()
    {
        foreach (ParticleSystem particle in _particleList)
            particle.Play();
    }
}

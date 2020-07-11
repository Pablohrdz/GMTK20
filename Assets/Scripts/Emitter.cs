using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Emitter : MonoBehaviour
{
    public KeyCode linkedKey;
    public bool active;
    ParticleSystem particles;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    public void enableParticles(bool enable)
    {
        ParticleSystem.EmissionModule emission = particles.emission;
        emission.enabled = enable;
    }
}

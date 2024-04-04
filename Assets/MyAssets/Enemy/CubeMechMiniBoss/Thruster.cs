using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public ParticleSystem thrusterParticles;
    private ParticleSystem.MainModule mainModule;

    public bool enabled = false;

    private void Start()
    {
        thrusterParticles = GetComponent<ParticleSystem>();
        mainModule = thrusterParticles.main;

        thrusterParticles.Stop();
        enabled = false;
    }

    public void EnableParticles(float simSpeed)
    {
        mainModule.simulationSpeed = simSpeed;
        thrusterParticles.Play();
        enabled = true;
    }

    public void DisableParticles()
    {
        thrusterParticles.Stop();
        enabled = false;
    }
}

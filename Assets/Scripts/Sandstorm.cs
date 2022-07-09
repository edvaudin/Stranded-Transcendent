using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sandstorm : MonoBehaviour
{
    [SerializeField] CanvasGroup sandstormBackground;
    [SerializeField] ParticleSystem sandstormParticles;
    private bool inSandstorm = false;

    private void Start()
    {
        sandstormBackground.alpha = 1f;
        sandstormParticles.Pause();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) { return; }
        inSandstorm = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) { return; }
        inSandstorm = false;
    }

    private void Update()
    {
        if (inSandstorm)
        {
            IncreaseSandstormIntensity();
        }
        else
        {
            DecreaseSandstormIntensity();
        }
    }

    private void DecreaseSandstormIntensity()
    {
        sandstormParticles.Pause();
        sandstormParticles.Clear();
        if (sandstormBackground.alpha > 0)
        {
            sandstormBackground.alpha -= Time.deltaTime;
        }
    }

    private void IncreaseSandstormIntensity()
    {
        sandstormParticles.Play();
        if (sandstormBackground.alpha < 0.72)
        {
            sandstormBackground.alpha += Time.deltaTime;
        }
    }
}

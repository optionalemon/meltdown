using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class Scanner : MonoBehaviour
{
    public ParticleSystem scanningParticles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.activated.AddListener(x => StartScanning());
            grabInteractable.deactivated.AddListener(x => StopScanning());
        }
    }

    public void StartScanning()
    {
       scanningParticles.Play();
    }

    public void StopScanning()
    {
        scanningParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}

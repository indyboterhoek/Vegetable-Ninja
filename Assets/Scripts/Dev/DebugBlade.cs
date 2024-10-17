using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBlade : MonoBehaviour
{
    public Material debugMaterial;
    private Renderer debugRenderer;
    private Material defaultMaterial;
    
    private bool touching = false;

    private void Awake()
    {
        debugRenderer = GetComponent<Renderer>();
        defaultMaterial = debugRenderer.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        touching = true;
    }
    
    private void OnTriggerExit(Collider other)
    {
        debugRenderer.material = defaultMaterial;
        touching = false;
    }
    
    public void Reset()
    {
        debugRenderer.material = defaultMaterial;
        touching = false;
    }

    public void Slash(float velocity, float minVelocity)
    {
        if (velocity > minVelocity)
        {
            debugRenderer.material = touching ? debugMaterial : defaultMaterial;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegetable : MonoBehaviour
{
    /* ================ Public variables ================ */
    public GameObject whole; // The whole vegetable object
    public GameObject sliced; // The sliced vegetable object
    public Rigidbody vegetableRigidbody; // The vegetable rigidbody
    public Collider vegetableCollider; // The vegetable collider
    public ParticleSystem juiceParticleEffect; // The juice particle effect

    /* ================ Unity methods ================ */
    private void OnTriggerEnter(Collider other) // When the vegetable collides with another collider
    {
        if (other.CompareTag("Player")) // If the collider has the tag "Player" which is the blade
        {
            Blade blade = other.GetComponent<Blade>(); // Get the blade component from the collider
            Slice(blade.slicingDirection, blade.transform.position, blade.slideForce); // Call the Slice method with the blade's slicing direction, position, and force
        }
    }
    
    /* ================ Private methods ================ */
    /// <summary>
    /// Slice the vegetable.
    /// Switch the whole and sliced objects.
    /// Activate the juice particle effect.
    /// Apply force to the sliced vegetable parts.
    /// Increase the score.
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="position"></param>
    /// <param name="force"></param>
    private void Slice(Vector3 direction, Vector3 position, float force)
    {
        FindObjectOfType<GameManager>().IncreaseScore(); // Call the IncreaseScore method from the GameManager script
        
        whole.SetActive(false); // Disable the whole vegetable object
        sliced.SetActive(true); // Enable the sliced vegetable object
        
        vegetableCollider.enabled = false; // Disable the vegetable collider
        juiceParticleEffect.Play(); // Play the juice particle effect
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle of the slice
        sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle); // Rotate the sliced object
        
        Rigidbody[] slicedRigidbodies = sliced.GetComponentsInChildren<Rigidbody>(); // Get the rigidbodies of the sliced object
        foreach (Rigidbody rb in slicedRigidbodies) // For each rigidbody in the sliced rigidbodies
        {
            rb.velocity = vegetableRigidbody.velocity; // Set the velocity of the rigidbody to the vegetable rigidbody velocity
            rb.AddForceAtPosition(direction * force, position, ForceMode.Impulse); // Add force to the rigidbody at the position
        }
    }
}

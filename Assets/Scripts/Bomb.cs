using UnityEngine;

public class Bomb : MonoBehaviour
{
    /* ================ Unity methods ================ */
    private void OnTriggerEnter(Collider other) // When the bomb collides with another collider
    {
        if (other.CompareTag("Player")) // If the collider has the tag "Player" which is the blade
        {
            FindObjectOfType<GameManager>().Explode(); // Call the Explode method from the GameManager script
        }
    }
}

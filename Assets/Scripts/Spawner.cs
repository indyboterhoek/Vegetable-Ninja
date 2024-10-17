using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    /* ================ Private variables ================ */
    private Collider _spawnArea; // The spawn area collider
    
    /* ================ Public variables ================ */
    public GameObject[] spawnPrefabs; // The array of vegetable prefabs to spawn
    public GameObject bombPrefab; // The bomb prefab to spawn
    [Range(0f, 1f)]
    public float bombChance = 0.05f; // The chance of spawning a bomb
    public float minSpawnDelay = 0.25f; // The minimum delay between spawns
    public float maxSpawnDelay = 1f; // The maximum delay between spawns
    public float minAngle = -15f; // The minimum angle of the spawn
    public float maxAngle = 15f; // The maximum angle of the spawn
    public float minForce = 18f; // The minimum force of the spawn
    public float maxForce = 22f; // The maximum force of the spawn
    public float maxLifetime = 5f; // The maximum lifetime of the spawned object
    public float preStartDelay = 2f; // The delay before the first spawn

    /* ================ Unity methods ================ */
    private void Awake() // Called when the script instance is being loaded
    {
        _spawnArea = GetComponent<Collider>(); // Get the collider component
    }
    
    private void OnEnable() // Called when the object becomes enabled and active
    {
        StartCoroutine(Spawn()); // Start the spawn coroutine
    }
    
    private void OnDisable() // Called when the object becomes disabled
    {
        StopAllCoroutines(); // Stop all coroutines
    }

    /* ================ Private methods ================ */
    /// <summary>
    /// Spawn a vegetable or bomb.
    /// Using the spawn area bounds, spawn a random vegetable or bomb prefab.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(preStartDelay); // Wait for the preStartDelay time before spawning
        
        while (enabled) // While the script is enabled
        {
            GameObject spawnPrefab = spawnPrefabs[Random.Range(0, spawnPrefabs.Length)]; // Get a random vegetable prefab
            
            if (Random.value < bombChance) // If the random value is less than the bomb chance
            {
                spawnPrefab = bombPrefab; // Set the spawn prefab to the bomb prefab
            }
            
            Vector3 position = new Vector3( // Create a random position within the spawn area bounds
                Random.Range(_spawnArea.bounds.min.x, _spawnArea.bounds.max.x),
                Random.Range(_spawnArea.bounds.min.y, _spawnArea.bounds.max.y),
                Random.Range(_spawnArea.bounds.min.z, _spawnArea.bounds.max.z)
            );
            
            Quaternion rotation = Quaternion.Euler( // Create a random rotation within the min and max angle
                0f,
                0f,
                Random.Range(minAngle, maxAngle)
            );

            GameObject vegetable = Instantiate(spawnPrefab, position, rotation); // Instantiate the spawn prefab at the random position and rotation
            Destroy(vegetable, maxLifetime); // Destroy the spawned object after the max lifetime
            
            Rigidbody rb = vegetable.GetComponent<Rigidbody>(); // Get the rigidbody component
            rb.AddForce(vegetable.transform.up * Random.Range(minForce, maxForce), ForceMode.Impulse); // Add a random force to the rigidbody
            
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay)); // Wait for a random delay before spawning again
        }
    }
}

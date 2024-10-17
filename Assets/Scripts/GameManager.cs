using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /* ================ Public variables ================ */
    public Text scoreText; // The text object that displays the _score
    public Image fadeImage; // The image object that fades in and out
    public DebugBlade debugBlade; // The DebugBlade script for testing when the blade is slicing

    /* ================ Private variables ================ */
    private Blade _blade; // Reference to the blade script
    private Spawner _spawner; // Reference to the spawner script
    private int _score; // The current score

    /* ================ Unity methods ================ */
    private void Awake() // Called when the script instance is being loaded
    {
        _blade = FindObjectOfType<Blade>(); // Get the blade script
        _spawner = FindObjectOfType<Spawner>(); // Get the spawner script
    }
    
    private void Start() // Called before the first frame update
    {
        NewGame(); // Start a new game
    }

    private void Update() // Called every frame
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // If the player presses the escape key
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single); // Load the main menu scene
        }
    }

    /* ================ Private methods ================ */
    /// <summary>
    /// New game setup.
    /// Clears any timescale.
    /// Resets the score.
    /// Clears scene of any vegetables and bombs.
    /// </summary>
    private void NewGame()
    {
        _blade.enabled = true; // Enable the blade script
        _spawner.enabled = true; // Enable the spawner script
        
        _score = 0; // Reset the score
        scoreText.text = _score.ToString(); // Update the score text
        
        Time.timeScale = 1f; // Reset the timescale
        
        ClearScene(); // Clear the scene of any vegetables and bombs
        
        if (debugBlade) // If the debug blade script exists
            debugBlade.Reset(); // Reset the debug blade
    }

    /// <summary>
    /// Clear scene.
    /// Destroys all vegetables and bombs in the scene.
    /// </summary>
    private void ClearScene()
    {
        Vegetable[] vegetables = FindObjectsOfType<Vegetable>(); // Find all vegetables in the scene
        foreach (Vegetable vegetable in vegetables) // Loop through each vegetable
        {
            Destroy(vegetable.gameObject); // Destroy the vegetable
        }
        
        Bomb[] bombs = FindObjectsOfType<Bomb>(); // Find all bombs in the scene
        foreach (Bomb bomb in bombs) // Loop through each bomb
        {
            Destroy(bomb.gameObject); // Destroy the bomb
        }
    }
    
    /// <summary>
    /// Explode sequence.
    /// Fades the screen to white.
    /// Waits 1 second.
    /// Fades the screen back to clear.
    /// Starts a new game.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ExplodeSequence()
    {
        float elapsedTime = 0f; // The time elapsed
        float duration = 0.5f; // The duration of the fade
        
        while (elapsedTime < duration) // While the elapsed time is less than the duration
        {
            float t = Mathf.Clamp01(elapsedTime / duration); // Calculate the alpha
            fadeImage.color = new Color(1f, 1f, 1f, t); // Set the fade image color
            
            Time.timeScale = 1f - t; // Set the timescale so it slows down the vegetables and bombs
            
            elapsedTime += Time.unscaledDeltaTime; // Increment the elapsed time by the unscaled delta time otherwise the fade will be affected by the timescale
            
#if UNITY_EDITOR
            Debug.Log($"ExplodeSequence | {t} | {elapsedTime} | {duration}");
#endif
            
            yield return null;
        }

        elapsedTime = 0f; // Reset the elapsed time
        
#if UNITY_EDITOR
        Debug.Log("ExplodeSequence - Waiting 1 second");
#endif
        
        yield return new WaitForSecondsRealtime(1f); // Wait 1 second
        
#if UNITY_EDITOR
        Debug.Log("ExplodeSequence - New Game");
#endif
        
        NewGame(); // Start a new game
        
        while (elapsedTime < duration) // While the elapsed time is less than the duration
        {
            float t = Mathf.Clamp01(elapsedTime / duration); // Calculate the alpha
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t); // Set the fade image color
            
            elapsedTime += Time.unscaledDeltaTime; // Increment the elapsed time by the unscaled delta time otherwise the fade will be affected by the timescale
            
#if UNITY_EDITOR
            Debug.Log($"ExplodeSequence | {t} | {elapsedTime} | {duration}");
#endif
            
            yield return null;
        }
        
#if UNITY_EDITOR
        Debug.Log("ExplodeSequence - Done");
#endif
    }

    /* ================ Public methods ================ */
    /// <summary>
    /// Explode.
    /// Stops the blade and spawner scripts.
    /// Starts the explode sequence.
    /// </summary>
    public void Explode()
    {
        _blade.enabled = false; // Disable the blade script
        _spawner.enabled = false; // Disable the spawner script

        StartCoroutine(ExplodeSequence()); // Start the explode sequence as a coroutine so it can wait
    }
    
    /// <summary>
    /// Increase the score by 1.
    /// </summary>
    public void IncreaseScore()
    {
        _score++; // Increment the score
        scoreText.text = _score.ToString(); // Update the score text
    }
}

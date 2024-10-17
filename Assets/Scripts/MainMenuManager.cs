#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    /* ================ Private variables ================ */
    [SerializeField] private UIDocument uiDoc; // The UI document
    private VisualElement _rootEl; // The root visual element

    /* ================ Unity methods ================ */
    private void OnEnable() // Called when the script instance is being loaded
    {
        _rootEl = uiDoc.rootVisualElement; // Get the root visual element from the UI document and assign it to the rootEl variable
        
        Button playButton = _rootEl.Q<Button>("PlayButton"); // Get the play button from the root element by querying for the button with the ID "PlayButton"
        Button quitButton = _rootEl.Q<Button>("QuitButton"); // Get the quit button from the root element by querying for the button with the ID "QuitButton"
        
        playButton.clicked += PlayGame; // Add a listener to the play button that calls the PlayerGame method when clicked
        quitButton.clicked += QuitGame; // Add a listener to the quit button that calls the QuitGame method when clicked
    }

    /* ================ Private methods ================ */
    /// <summary>
    /// Play game.
    /// Switches to the "VegetableNinja" scene.
    /// </summary>
    private void PlayGame()
    {
        SceneManager.LoadScene("VegetableNinja", LoadSceneMode.Single); // Load the "VegetableNinja" scene
    }
    
    /// <summary>
    /// Quit game.
    /// Quits the application or stops playing in the editor.
    /// </summary>
    private void QuitGame()
    {
        Application.Quit(); // Quit the application
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop playing in the editor only if the application is running in the editor
#endif
    }
}

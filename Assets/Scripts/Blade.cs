using UnityEngine;

public class Blade : MonoBehaviour
{
    /* ================ Private variables ================ */
    private Camera _mainCamera; // Reference to the main camera
    private Collider _bladeCollider; // Reference to the blade collider
    private TrailRenderer _bladeTrail; // Reference to the blade trail
    private bool _isSlicing; // Is the player slicing?
    
    /* ================ Public variables ================ */
    public Vector3 slicingDirection { get; private set; } // The direction the blade is slicing. Allows other scripts to read the value but not change it. (Used in Vegetable.cs)
    public float slideForce = 10f; // The force applied to the sliced vegetables
    public float minSliceVelocity = 0f; // The minimum velocity required to slice
    
    /* ================ Debug variables ================ */
    public DebugBlade debugBlade; // Reference to the debug blade script. This is used to visualize the slicing using a coloured cube.

    /* ================ Unity methods ================ */
    private void Awake() // Called when the script instance is being loaded
    {
        _mainCamera = Camera.main; // Get the main camera
        _bladeCollider = GetComponent<Collider>(); // Get the blade collider
        _bladeTrail = GetComponentInChildren<TrailRenderer>(); // Get the blades trail renderer
    }
    
    private void OnEnable() // Called when the object becomes enabled and active
    {
        StopSlicing(); // Stop slicing when the object is enabled
    }

    private void OnDisable()
    {
        StopSlicing(); // Stop slicing when the object is disabled
    }

    private void Update() // Called every frame
    {
        if (Input.GetMouseButtonDown(0)) // If the player presses the left mouse button or touches the screen
        {
            StartSlicing(); // Start slicing
        }
        else if (Input.GetMouseButtonUp(0)) // If the player releases the left mouse button or stops touching the screen
        {
            StopSlicing(); // Stop slicing
        }
        else if (_isSlicing) // If the player is slicing
        {
            ContinuingSlice(); // Continue slicing
        }    
    }

    /* ================ Private methods ================ */
    /// <summary>
    /// Start the slicing process.
    /// Update the blade's position to the mouse position.
    /// Enable the blade collider and trail renderer.
    /// Clear the blade trail of any previous points.
    /// </summary>
    private void StartSlicing() 
    {
        Vector3 newPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position in world space
        newPosition.z = 0f; // Set the z position to 0
        
        transform.position = newPosition; // Set the blade's position to the mouse position
        
        _isSlicing = true; // Set the slicing flag to true
        _bladeCollider.enabled = true; // Enable the blade collider
        _bladeTrail.enabled = true; // Enable the blade trail
        _bladeTrail.Clear(); // Clear the blade trail
    }
    
    /// <summary>
    /// Stop the slicing process.
    /// Disable the blade collider and trail renderer.
    /// </summary>
    private void StopSlicing()
    {
        _isSlicing = false; // Set the slicing flag to false
        _bladeCollider.enabled = false; // Disable the blade collider
        _bladeTrail.enabled = false; // Disable the blade trail
    }
    
    /// <summary>
    /// Continue the slicing process.
    /// Finds the velocity of the blade and enables the blade collider if the velocity is greater than the minimum slice velocity.
    /// Sets the blade's position to the mouse position.
    /// </summary>
    private void ContinuingSlice()
    {
        Vector3 newPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;
        
        slicingDirection = newPosition - transform.position;
        
        float velocity = slicingDirection.magnitude / Time.deltaTime;
        _bladeCollider.enabled = velocity > minSliceVelocity;

        if (debugBlade)
            debugBlade.Slash(velocity, minSliceVelocity);
        
        transform.position = newPosition;
    }
}

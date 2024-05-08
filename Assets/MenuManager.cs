using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static bool gamePaused = false;
    public GameObject pauseMenu;
    public MonoBehaviour cameraController; // Reference to your camera controller script

    // Update is called once per frame
    void Update()
    {
        // Check if the Escape key was pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true); // Enable the pause menu UI
        Time.timeScale = 0f; // Pause the game
        gamePaused = true;

        // Show the cursor and unlock it
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Disable the camera movement script
        if (cameraController != null)
        {
            cameraController.enabled = false;
        }

        // Optionally, disable other player inputs here
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
        pauseMenu.SetActive(false); // Disable the pause menu UI
        gamePaused = false;

        // Hide the cursor and lock it back
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Enable the camera movement script
        if (cameraController != null)
        {
            cameraController.enabled = true;
        }

        // Optionally, enable other player inputs here
    }

    // Optional: Implement your methods for the pause menu like QuitGame, RestartGame etc.
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class PauseGame : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI; //Drop in the object on the canvas that serves as a pausemenu

    public GameObject uiCamera;
    public GameObject playerCamera;
    public GameObject music;
    public GameObject cursor;

    

    // Was first just a basic pause menu but I added stuff so it works as a main menu as well

    void Update()

    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked; //deactivates the cursor so it doesnt interfere with the game
        Cursor.visible = false;
        uiCamera.SetActive(false);
        playerCamera.SetActive(true);
        music.GetComponent<AudioSource>().UnPause(); // resumes audio
        cursor.SetActive(true);

    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None; //unlocks the cursor so buttons can be clicked with the mouse
        Cursor.visible = true; // activates the cursor
        uiCamera.SetActive(true); // change to an uicamera when paused
        playerCamera.SetActive(false); // deactivates the main camera so it doesnt move when paused
        music.GetComponent<AudioSource>().Pause(); //Pauses audio
        cursor.SetActive(false); // hides the crosshair so it doesnt get away in the pausemenu
        
    }

    public void QuitGame() // Quits the game to desktop
    {
        Application.Quit();
    }

    public void NewGame() // New game function. Double check so the scene is in the right place in build settings
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void BackToMenu() // Basic back to menu function. Needs to be altered if more scenes are added
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    
}
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

    

    // basic pause menu with resume and quit function so the player can pause and close the application

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
        Cursor.visible = true;
        uiCamera.SetActive(true);
        playerCamera.SetActive(false);
        music.GetComponent<AudioSource>().Pause(); //Pauses audio
        cursor.SetActive(false);
        
    }

    public void QuitGame() // Quits the game to desktop
    {
        Application.Quit();
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    
}
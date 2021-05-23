using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    //public GameObject quitButton;
    public bool isPaused;
    public AudioSource audioHighlighted;
    public AudioSource audioClick;
    public AudioSource gamePaused;
    public AudioSource gameUnpaused;

    private void Start()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        //quitButton.SetActive(false);
    }

    private void Update()
    {
        //Check if game is paused
        if (Input.GetButtonDown("Cancel"))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }


    //Enable pause game
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        //quitButton.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        gamePaused.Play();
    }

    //Disable pause game
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        gameUnpaused.Play();
    }

    //Go to menu
    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    //Exit game
    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void setFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SoundWhenHover()
    {
        audioHighlighted.Play();
    }

    public void SoundWhenClick()
    {
        audioClick.Play();
    }
}

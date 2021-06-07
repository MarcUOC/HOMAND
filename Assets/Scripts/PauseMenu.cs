using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public bool isPaused;
    public AudioSource audioHighlighted;
    public AudioSource audioClick;
    public AudioSource gamePaused;
    public AudioSource gameUnpaused;
    private Player playerHP;

    private void Start()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        playerHP = FindObjectOfType<Player>();
    }

    private void Update()
    {
        //CHECK GAME PAUSED
        if ((Input.GetButtonDown("Cancel") || (Input.GetKeyDown(KeyCode.Escape))) && playerHP.health > 0) //PAUSE GAME
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


    //ENABLE PAUSE GAME
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        gamePaused.Play();
    }

    //DISABLE PAUSE GAME
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        gameUnpaused.Play();
    }

    //MENU
    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    //EXIT GAME
    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    //FULL SCREEN MODE
    public void setFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    //SOUND WHEN MOUSE OVER
    public void SoundWhenHover()
    {
        audioHighlighted.Play();
    }

    //SOUND WHEN CLICK BUTTON
    public void SoundWhenClick()
    {
        audioClick.Play();
    }
}

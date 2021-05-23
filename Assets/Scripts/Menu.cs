using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    public AudioSource audioHighlighted;
    public AudioSource audioClick;

    //Go to game scene
    public void PlayGame() 
    {
        SceneManager.LoadScene("Game");
    }

    //Return to main menu
    public void FinalGame()
    {
        SceneManager.LoadScene("Menu");
    }

    //Full Screen
    public void setFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    //Quit Game
    public void QuitGame()
    {
        Debug.Log("QUIT GAME");
        Application.Quit();
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
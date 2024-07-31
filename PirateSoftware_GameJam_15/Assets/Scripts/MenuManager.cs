using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public GameObject PauseUI;

    public bool GameIsPaused = false;

    public GameObject GameCamera;
    public GameObject GameFinishedScreen;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PauseGame()
    {
        GameIsPaused = true;
        PauseUI.SetActive(GameIsPaused);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        GameIsPaused = false;
        PauseUI.SetActive(GameIsPaused);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void FinishedGame()
    {
        GameCamera.SetActive(false);
        GameFinishedScreen.GetComponent<Animator>().SetBool("GameEnd", true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Single_Player : MonoBehaviour
{
    public void LoadSinglePlayerGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadCoOpGame()
    {
        Debug.Log("Co-Op Game Loading");
        SceneManager.LoadScene("Co-Op");
    }

    public void QuitGameMenu()
    {
        Application.Quit();
    }
}

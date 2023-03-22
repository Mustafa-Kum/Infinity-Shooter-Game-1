using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenuPanel;

    private Animator _pauseAnimator;

    private void Start()
    {
        _pauseAnimator = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Update() // ---> R'ye basarak oyunu restartladığımız yer.
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _pauseMenuPanel.SetActive(true);
            _pauseAnimator.SetBool("isPaused", true);
            Time.timeScale = 0;
        }
    }

    public void ResumeGame()
    {
        _pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }
}

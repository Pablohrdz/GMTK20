using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject GameOverScreen;

    private GameObject Player;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        /*
        // TODO: Implement this when oxygen bar feature is ready
        if (Player.RanOutOfOxygen)
        {
            // Display Game Over screen
            GameOverScreen.SetActive(true);
        }
        */

        // TODO: Borrar esto. Es para testing.
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restarted Game");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

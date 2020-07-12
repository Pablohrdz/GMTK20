using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    bool gamePaused;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        gamePaused = !gamePaused;
        Time.timeScale = gamePaused ? 0 : 1;
    }
}

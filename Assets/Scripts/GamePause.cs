using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    bool gamePaused;
    bool healing;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !healing)
        {
            Pause(!gamePaused);
        }
    }

    void Pause(bool pause)
    {
        if (!healing)
        {
            // TODO: show text
            gamePaused = pause;
            Time.timeScale = gamePaused ? 0 : 1;
        }
    }

    public void Healing(bool healing)
    {
        if (healing)
        {
            // TODO: show text
            Pause(true);
            this.healing = true;
        }
        else
        {
            // TODO: remove text
            this.healing = false;
            Pause(false);
        }
    }
}

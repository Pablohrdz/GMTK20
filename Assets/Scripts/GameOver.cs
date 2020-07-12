using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject GameOverScreen;
    public float fadeTimeSeconds;

    private GameObject Player;
    private float timerSeconds;
    private bool startFade = false;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        timerSeconds = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
        // TODO: Implement this when oxygen bar feature is ready
        if (Player.GetComponent<SubmarineController>().gameEnded)
        {
            // Display Game Over screen
            if (!startFade)
            {
                startFade = true;
                StartCoroutine(DoFade());
                GameOverScreen.SetActive(true);
            }
        }
        

        // TODO: Borrar esto. Es para testing.
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        //StartCoroutine(AudioManager.instance.StartFade("volumeMaster", 3.0f, 1.0f));
        timerSeconds = 0.0f;
        AudioManager.instance.mixer.SetFloat("volumeMaster", 1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        Debug.Log("Restarted Game");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator DoFade()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
         Debug.Log(" init " + fadeTimeSeconds + " timer " + timerSeconds);

        while (timerSeconds < fadeTimeSeconds)
        {
            timerSeconds += Time.deltaTime / 2;

            canvasGroup.alpha = timerSeconds / (fadeTimeSeconds+0.0f);
            yield return null;
        }
        //  canvasGroup.interactable = false;
        // yield return null;
    }
}

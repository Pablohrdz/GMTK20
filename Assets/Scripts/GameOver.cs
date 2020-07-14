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
    private bool fadeGame = false;
    private IEnumerator audioCoroutine;

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
            if (!fadeGame)
            {
                fadeGame = true;
                StartCoroutine(DoFade());
                audioCoroutine = AudioManager.instance.StartFade("volumeMaster", 13.0f, 0.0f);
                StartCoroutine(audioCoroutine);
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
        
        fadeGame = false;
        if (audioCoroutine != null)
        {
            StopCoroutine(audioCoroutine);
        }
        AudioManager.instance.mixer.SetFloat("volumeMaster", 0.0f);
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

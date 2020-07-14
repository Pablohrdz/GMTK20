using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadNextLevel : MonoBehaviour
{
    public void LoadNextScene()
    { 
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioManager.instance.sendAudioEvent(
                AudioEvent.Play,
                new AudioEventArgs() { sampleId = "pearl-collect", volume = 1.0f, mixerChannelName="FX" }
            );
            // LoadNextScene();
            this.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(waitLoadNextScene());

        }
    }

    public IEnumerator waitLoadNextScene()
    {
        yield return new WaitForSeconds(0.4f);
        LoadNextScene();
    }

    public void Quit()
    {
        Application.Quit();
    }
}

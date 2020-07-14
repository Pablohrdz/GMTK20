using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : Enemy
{

    public float DetectionRadius = 15.0f;
    
    private Transform Player;
    private bool notPlayed = true;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        gameObject.transform.Find("Text").GetComponent<TextMesh>().text = linkedKey.ToString();
        gameObject.transform.Find("Text").GetComponent<MeshRenderer>().sortingOrder = 10;
    }

    void Update()
    {
        if (!destroyed)
        {
            Destroy(GetComponent<PolygonCollider2D>());
            gameObject.AddComponent<PolygonCollider2D>();
        }
        gameObject.transform.Find("Text").transform.up = Vector2.up;
        float DistanceToPlayer = Vector3.Distance(transform.position, Player.position);
        // Debug.Log(DistanceToPlayer);

        if (DistanceToPlayer <= DetectionRadius)
        {
            AudioManager.instance.sendAudioEvent(AudioEvent.Play, this.GetComponent<AudioSource>(), new AudioEventArgs() { sampleId = "crab-move", volume =0.2f, mixerChannelName = "FX", throttleSeconds = Random.Range(0.4f, 1.0f) });
        }
        else
        {
            // AudioManager.instance.sendAudioEvent(AudioEvent.Stop, this.GetComponent<AudioSource>(), new AudioEventArgs() { sampleId = "crab-move", volume = 1.0f });
        }

    }
}

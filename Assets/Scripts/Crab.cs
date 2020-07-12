using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : Enemy
{

    public float DetectionRadius = 15.0f;
    
    private Transform Player;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();

        float DistanceToPlayer = Vector3.Distance(transform.position, Player.position);
        // Debug.Log(DistanceToPlayer);

        if (DistanceToPlayer <= DetectionRadius)
        {
            AudioManager.instance.sendAudioEvent(AudioEvent.Play, this.GetComponent<AudioSource>(), new AudioEventArgs() { sampleId = "crab-move", volume = 0.55f, mixerChannelName = "Fauna", throttleSeconds = Random.Range(0.1f, 0.4f) });
        }
        else
        {
            AudioManager.instance.sendAudioEvent(AudioEvent.Stop, this.GetComponent<AudioSource>(), new AudioEventArgs() { sampleId = "crab-move", volume = 1.0f });
        }

    }
}

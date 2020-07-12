﻿using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    // Move emissionForce to each individual emitter and enemy?
    public GameObject emitterPrefab;
    //public GameObject stampPrefab;
    public float airMax;
    public float air;
    public float airLossMultiplier;
    public List<GameObject> pool;
    public bool HasCollectedPearl;
    public float crashDuration;

    List<Emitter> emitters;
    Rigidbody2D rb;
    float timeOfCrash;
    bool crashing { get { return Time.time - timeOfCrash < crashDuration; } }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        emitters = new List<Emitter>();
        foreach(Transform child in transform.Find("Emitters"))
        {
            Emitter emitter = child.GetComponent<Emitter>();
            if (emitter != null)
            {
                emitter.InstantiateLetter();
                emitters.Add(emitter);
            }
        }
    }

    void Update()
    {
        var uncoveredNumber = 0;
        if (air >= 0)
        {
            foreach (var emitter in emitters)
            {
                bool holeUncovered = emitter.active && (!Input.GetKey(emitter.linkedKey) || crashing);
                emitter.enableParticles(holeUncovered);
                if (holeUncovered)
                {
                    uncoveredNumber += 1;
                    // To avoid stacking crashes
                    emitter.disableLetter();
                    Vector3 force = -emitter.transform.forward.normalized * emitter.emissionForce;
                    rb.AddForceAtPosition(force, emitter.transform.position);
                    air -= emitter.emissionForce * airLossMultiplier * Time.deltaTime;
                }
                else
                {
                    emitter.enableLetter();
                    // AudioManager.instance.sendAudioEvent(AudioEvent.Play, transform.Find("Emitters").GetComponent<AudioSource>(), new AudioEventArgs() { sampleId = "air-leak-fix", volume = 1.0f, mixerChannelName = "Leaks" });
                }
            }
            if (uncoveredNumber > 0)
            {
                // Debug.Log("Playing leak");
                //  0.4f + (uncoveredNumber/(emitters.Count+0.0f))*0.6f
                AudioManager.instance.sendAudioEvent(AudioEvent.Play, transform.Find("Emitters").GetComponent<AudioSource>(), new AudioEventArgs() { sampleId = "air-leak-loop", volume =(uncoveredNumber / (emitters.Count + 0.0f)), mixerChannelName = "Leaks", loop=true });
                AudioManager.instance.sendAudioEvent(AudioEvent.Play, emitters[0].GetComponent<AudioSource>(), new AudioEventArgs() { sampleId = "submarine-bubbles-loop", volume =  0.2f + (uncoveredNumber / (emitters.Count + 0.0f)) * 0.8f , mixerChannelName = "Leaks", loop = true });
            }
            else
            {
                AudioManager.instance.sendAudioEvent(AudioEvent.Stop, emitters[0].GetComponent<AudioSource>(), new AudioEventArgs() { sampleId = "submarine-bubbles-loop", volume = 1.0f});
                AudioManager.instance.sendAudioEvent(AudioEvent.Stop, transform.Find("Emitters").GetComponent<AudioSource>(), new AudioEventArgs() { sampleId = "air-leak-loop", volume = 1.0f });
                
            }
        }
        else
        {
            foreach (var emitter in emitters)
            {
                emitter.enableParticles(false);
                // TODO: dead animation
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SwapLetters();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {

            AudioManager.instance.sendAudioEvent(AudioEvent.Play, this.GetComponent<AudioSource>(), new AudioEventArgs() { sampleId = "swordfish-submarine-collision", volume = 0.7f, mixerChannelName = "Submarine" });
            Swordfish swordfish = collision.gameObject.GetComponent<Swordfish>();
            if (swordfish != null)
            {
                AudioManager.instance.sendAudioEvent(AudioEvent.Play, enemy.GetComponent<AudioSource>(), new AudioEventArgs() { sampleId = "swordfish-attack", volume = 1.0f, mixerChannelName = "Fauna" });
                swordfish.DestroyCrosshair();
            }

            Destroy(collision.gameObject); // TODO: animate, remember to disable collider while it fades

            // TODO: there should only be one hit, but we should double check...
            ContactPoint2D contact = collision.contacts[0];

            Debug.DrawRay(contact.point, contact.normal, Color.green, 2, false);
            Vector2 point = contact.point;
            GameObject emitterGO = Instantiate(
                emitterPrefab,
                new Vector3(point.x, point.y, 0),
                Quaternion.LookRotation(-contact.normal),
                transform.Find("Emitters"));
            Emitter emitter = emitterGO.GetComponent<Emitter>();
            emitter.setLinkedKey(collision.gameObject.GetComponent<Enemy>().linkedKey);
            emitter.emissionForce = enemy.emissionForce;
            emitter.InstantiateLetter();
            emitter.enableLetter();
            emitters.Add(emitter);
        }

        var swapper = collision.gameObject.GetComponent<Swapper>();
        if (swapper != null)
        {
            SwapLetters();
            Destroy(collision.gameObject); // TODO: animate, remember to disable collider while it fades
        }

        // Check for collisions with the environment to shake the camera.
        if (collision.gameObject.tag == "Environment")
        {
            AudioManager.instance.sendAudioEvent(AudioEvent.Play, this.GetComponent<AudioSource>(), new AudioEventArgs() { sampleId = "submarine-crash", volume = 0.7f, mixerChannelName = "Submarine", throttleSeconds=0.2f });
            // To avoid stacking crashes
            if (!crashing)
            {
                timeOfCrash = Time.time;
            }
            CameraShake.Instance.ShakeCamera(10.0f, 0.3f /* secs */);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var airPocket = collision.gameObject.GetComponent<AirPocket>();
        if (airPocket != null)
        {
            AudioManager.instance.sendAudioEvent(
                AudioEvent.Play,
                this.GetComponent<AudioSource>(),
                new AudioEventArgs() { sampleId = "bubble-pop", volume = 0.7f, mixerChannelName = "Submarine" }
            );

            Destroy(collision.gameObject); // TODO: animate, remember to disable collider while it fades
            air += airPocket.air * Time.deltaTime;
            if (air > airMax)
            {
                air = airMax;
            }
        }
    }

    private void SwapLetters()
    {

        if (transform.Find("Emitters").childCount < 2)
            return;
        var letter1 = Random.Range(0,transform.Find("Emitters").childCount);
        var letter2 = Random.Range(0,transform.Find("Emitters").childCount);

        while (letter1 == letter2)
        {
            letter2 = Random.Range(0, transform.Find("Emitters").childCount);
        }
        //Aqui ya se decidio cuales
        Emitter em1 = transform.Find("Emitters").GetChild(letter1).GetComponent<Emitter>();
        KeyCode kc1 = em1.linkedKey;
        Emitter em2 = transform.Find("Emitters").GetChild(letter2).GetComponent<Emitter>();
        KeyCode kc2 = em2.linkedKey;
        
        em1.swapLetterWith(em2.transform, kc2);
        em2.swapLetterWith(em1.transform, kc1);
        AudioManager.instance.sendAudioEvent(AudioEvent.Play, this.GetComponent<AudioSource>(), new AudioEventArgs() { sampleId = "key-swap", volume = 1.0f, mixerChannelName = "Submarine" });
    }
}

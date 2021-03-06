﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordfish : Enemy
{
    // Behavior:
        // 0. Patrol? (TODO)
        // 1. Detectar enemigo a cierta distancia
        // 2. Telegraph
        // 3. Perseguir enemigo

    public float Speed = 2.0f;
    public float DetectionRadius = 15.0f;
    public float TelegraphDuration = 1.0f;
    public GameObject CrosshairPrefab;

    private Transform LastPlayerTransform;
    private Transform Player;
    private bool IsChasing;
    private SpriteRenderer Renderer;
    private GameObject InstancedCrosshair;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        Renderer = GetComponent<SpriteRenderer>();
        gameObject.transform.Find("Text").GetComponent<TextMesh>().text = linkedKey.ToString();
        gameObject.transform.Find("Text").GetComponent<MeshRenderer>().sortingOrder = 10;
    }

    // Update is called once per frame
    void Update()
    {
        float DistanceToPlayer = Vector3.Distance(transform.position, Player.position);
        // Debug.Log(DistanceToPlayer);

        gameObject.transform.Find("Text").transform.up = Vector2.up;
        if (DistanceToPlayer <= DetectionRadius && !destroyed)
        {
            StartCoroutine(Chase());
        }
        else
        {
            IsChasing = false;
            Renderer.material.color = Color.white;
            transform.right = Vector3.right;

            DestroyCrosshair();
        }
    }

    private IEnumerator Chase()
    {
        if (!IsChasing)
        {
            IsChasing = true;
            AudioManager.instance.sendAudioEvent(
                AudioEvent.Play,
                this.GetComponent<AudioSource>(),
                new AudioEventArgs() { sampleId = "swordfish-swim-loop", volume = 0.8f, throttleSeconds = 2.0f }
            );

            // Instance crosshair on player position
            InstancedCrosshair = Instantiate(CrosshairPrefab, Player.position, Quaternion.identity);

            // Change color
            //Renderer.material.color = Color.red;
            
            // Wait in order to telegraph action
            yield return new WaitForSeconds(TelegraphDuration);
        }

        // Move towards target
        transform.position = Vector2.MoveTowards(transform.position, Player.position, Speed * Time.deltaTime);

        // Look at target
        transform.right = transform.position - Player.position;

        //if (InstancedCrosshair != null)
        //{
        //    InstancedCrosshair.transform.position = Player.position;
        //}
    }

    
    public void DestroyCrosshair()
    {
        if (InstancedCrosshair != null)
        {
            Destroy(InstancedCrosshair);
            InstancedCrosshair = null;
        }
    }
}

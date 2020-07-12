﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swapper : MonoBehaviour
{
    public float Speed = 2.0f;
    public float DetectionRadius = 15.0f;
    public float TelegraphDuration = 1.0f;

    private Transform LastPlayerTransform;
    private Transform Player;
    private bool IsChasing;
    private SpriteRenderer Renderer;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        Renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float DistanceToPlayer = Vector3.Distance(transform.position, Player.position);
        // Debug.Log(DistanceToPlayer);

        if (DistanceToPlayer <= DetectionRadius)
        {
            StartCoroutine(Chase());
        }
        else
        {
            IsChasing = false;
            Renderer.material.color = Color.white;
            transform.up = Vector3.up;
        }
    }
    private IEnumerator Chase()
    {
        if (!IsChasing)
        {
            IsChasing = true;

            // Record original position where the fish was triggered
            LastPlayerTransform = Player;


            Renderer.material.color = Color.magenta;
            // Look at target
            transform.up = transform.position - LastPlayerTransform.position;

            // Wait in order to telegraph action
            yield return new WaitForSeconds(TelegraphDuration);
        }

        transform.up = transform.position - LastPlayerTransform.position;
        // Move towards target
        transform.position = Vector2.MoveTowards(transform.position, LastPlayerTransform.position, Speed * Time.deltaTime);
    }
}

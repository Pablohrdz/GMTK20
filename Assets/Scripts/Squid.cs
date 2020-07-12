using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Squid : MonoBehaviour
{
    public float Speed = 10.0f;
    public float DetectionRadius = 10.0f;
    public float SafeRadius = 20.0f;
    public float TelegraphDuration = .3f;

    private Transform LastPlayerTransform;
    private Transform Player;
    private bool HasTurned;
    private SpriteRenderer Renderer;
    private ParticleSystem Ink;
    private bool IsRunning;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        Renderer = GetComponent<SpriteRenderer>();
        Ink = gameObject.transform.Find("EmitterInk").GetComponent<ParticleSystem>();
        IsRunning = false;
        //enableParticles(true);
    }

    // Update is called once per frame
    void Update()
    {
        float DistanceToPlayer = Vector3.Distance(transform.position, Player.position);

        if (DistanceToPlayer <= DetectionRadius || (IsRunning && DistanceToPlayer <= SafeRadius))
        {
            StartCoroutine(Run());
        }
        else
        {
            Renderer.material.color = Color.white;
            transform.up = Vector3.up;
            enableParticles(false);
            IsRunning = false;
        }
    }
    private IEnumerator Run()
    {

        if (!IsRunning)
        {
            // Record original position where the squid
            LastPlayerTransform = Player;
            Renderer.material.color = Color.blue;
            transform.up = transform.position - LastPlayerTransform.position;
            IsRunning = true;
            enableParticles(true);
            // Wait in order to telegraph action
            yield return new WaitForSeconds(TelegraphDuration);
        }
        // Move away from target
        //transform.position = Vector2.MoveTowards(transform.position, LastPlayerTransform.position, Speed * Time.deltaTime);
        var direction = transform.position - LastPlayerTransform.position;
        direction.Normalize();
        transform.position += direction * Speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<SubmarineController>()!= null)
        {
            //If touched
        }
    }
    public void enableParticles(bool enable)
    {
        ParticleSystem.EmissionModule emission = Ink.emission;
        emission.enabled = enable;
    }
}

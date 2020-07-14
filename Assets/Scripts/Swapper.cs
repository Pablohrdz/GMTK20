using System.Collections;
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
    public bool destroyed=false;

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

        if (DistanceToPlayer <= DetectionRadius && !destroyed)
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
           


            //Renderer.material.color = Color.magenta;
            // Look at target
            transform.up = transform.position - LastPlayerTransform.position;

            // Wait in order to telegraph action
            yield return new WaitForSeconds(TelegraphDuration);
        }
        AudioManager.instance.sendAudioEvent(
               AudioEvent.Play,
               this.GetComponent<AudioSource>(),
               new AudioEventArgs() { sampleId = "big-octopus-swim-stem", volume = 0.5f, throttleSeconds = 0.1f }
           );

        transform.up = transform.position - LastPlayerTransform.position;
        // Move towards target
        transform.position = Vector2.MoveTowards(transform.position, LastPlayerTransform.position, Speed * Time.deltaTime);
    }

    public IEnumerator FadeOut()
    {
        if (this.gameObject != null)
        {
            yield return new WaitForSeconds(.5f);
            IsChasing = false;
            var collider = GetComponent<Collider2D>();
            collider.enabled = false;
            var step = .1f;
            var delta = .05f;
            for (float f = 1; f >= 0; f -= step)
            {
                var rend = GetComponent<SpriteRenderer>();
                Color c = rend.material.color;
                c.a = f;
                rend.material.color = c;
                yield return new WaitForSeconds(delta);
            }
            Destroy(this.gameObject);
        }
    }
}

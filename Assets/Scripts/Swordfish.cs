using System.Collections;
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
            transform.right = Vector3.right;

            DestroyCrosshair();
        }
    }

    private IEnumerator Chase()
    {
        if (!IsChasing)
        {
            IsChasing = true;

            // Record original position where the fish was triggered
            LastPlayerTransform = Player;

            // Instance crosshair on player position
            InstancedCrosshair = Instantiate(CrosshairPrefab, LastPlayerTransform.position, Quaternion.identity);

            // Change color
            Renderer.material.color = Color.red;

            // Look at target
            transform.right = LastPlayerTransform.position - transform.position;

            // Wait in order to telegraph action
            yield return new WaitForSeconds(TelegraphDuration);
        }

        // Move towards target
        transform.position = Vector2.MoveTowards(transform.position, LastPlayerTransform.position, Speed * Time.deltaTime);
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

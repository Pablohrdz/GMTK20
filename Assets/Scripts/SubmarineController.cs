using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    // move to each individual emitter?
    public float emissionForce; 
    List<Emitter> emitters;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        emitters = new List<Emitter>();
        foreach(Transform child in transform)
        {
            Emitter emitter = child.GetComponent<Emitter>();
            if (emitter != null)
            {
                emitters.Add(emitter);
            }
        }
    }

    void Update()
    {
        foreach (var emitter in emitters)
        {
            if (!emitter.active) { continue; }
            bool holeCovered = Input.GetKey(emitter.linkedKey);
            emitter.enableParticles(!holeCovered);
            if (!holeCovered)
            {
                rb.AddForce(-emitter.transform.forward.normalized * emissionForce);
            }
        }
    }
}

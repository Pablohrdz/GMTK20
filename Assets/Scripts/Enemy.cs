using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public KeyCode linkedKey;
    public float emissionForce;
    public bool destroyed=false;

    public IEnumerator FadeOut()
    {
        if (this.gameObject != null)
        {
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
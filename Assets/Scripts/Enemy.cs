using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public KeyCode linkedKey;
    public float emissionForce;
    public bool destroyed=false;

    public IEnumerator FadeOut(Transform otherTransform = null)
    {
        if (this.gameObject != null)
        {
            var collider = GetComponent<Collider2D>();
            collider.enabled = false;
            var step = .1f;
            var delta = .05f;
            for (float f = 1; f >= 0; f -= step)
            {
                if (otherTransform != null)
                {
                    var letter = this.transform.Find("Text");

                    letter.transform.position = Vector3.Lerp(
                            letter.transform.position, otherTransform.position, 2 * Time.deltaTime);
                }
                var rendT = this.transform.Find("Text").GetComponent<MeshRenderer>();
                Color cT = rendT.material.color;
                cT.a = f;
                rendT.material.color = cT;

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
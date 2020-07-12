using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Perla : MonoBehaviour
{
    public Image PerlImg;

    private void Start()
    {
        // Set alpha to 0
        PerlImg.color = new Color(PerlImg.color.r, PerlImg.color.g, PerlImg.color.b, 0f); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            // Registrar perla en jugador
            SubmarineController sub = collision.gameObject.GetComponent<SubmarineController>();
            sub.HasCollectedPearl = true;

            // Activar perla en UI
            PerlImg.color = new Color(PerlImg.color.r, PerlImg.color.g, PerlImg.color.b, 1f);

            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame : MonoBehaviour
{
    public GameObject WinScreen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SubmarineController sub = collision.gameObject.GetComponent<SubmarineController>();

            if (sub.HasCollectedPearl)
            {
                // Display Win Game screen
                WinScreen.SetActive(true);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue Dialogue;

    public void TriggerDialogue()
    {
        // Debug.Log("DialogueManager Instance null? " + (DialogueManager.Instance == null));
        DialogueManager.Instance.StartDialogue(Dialogue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TriggerDialogue();
        }
    }
}

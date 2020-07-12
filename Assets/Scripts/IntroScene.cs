using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : MonoBehaviour
{
    private DialogueTrigger DialogueTrigger;

    // Start is called before the first frame update
    void Start()
    {
        DialogueTrigger = GetComponent<DialogueTrigger>();
        DialogueTrigger.TriggerDialogue();
    }
}

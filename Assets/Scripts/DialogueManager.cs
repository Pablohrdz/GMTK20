using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance = null;

    public Text NameText;
    public Text DialogueText;
    public Text ContinueText;
    public Animator Animator;

    [HideInInspector]
    public bool IsOpen;

    private Queue<string> Sentences;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if(Instance != this)
            Destroy(this);

        // DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartDialogue(Dialogue dialogue)
    {
        IsOpen = true;
        Animator.SetBool("IsOpen", IsOpen);
        NameText.text = dialogue.Name;

        Sentences = new Queue<string>();
        Debug.Log("Sentences : " + string.Join(",", dialogue.Sentences));
        Sentences.Clear();

        foreach(string sentence in dialogue.Sentences)
        {
            Sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        Debug.Log("Sentence count: " + Sentences.Count );
        if(Sentences.Count == 0)
        {
            EndDialogue();
            return ;
        }

        string sentence = Sentences.Dequeue();

        Debug.Log("Next Sentence: " + sentence);

        ContinueText.text = Sentences.Count > 0 ? ">>" : string.Empty;

        // In case user skips sentences, we need to stop animating the previous sentence before animating the new one
        StopAllCoroutines();

        // Animate showing sentence on dialogue box
        StartCoroutine(TypeSentence(sentence));
    }

    public void EndDialogue()
    {
        Sentences.Clear();
        IsOpen = false;
        Animator.SetBool("IsOpen", IsOpen);
    }

    IEnumerator TypeSentence(string sentence)
    {
        DialogueText.text = "";
        Debug.Log("Coroutine start sentence: " + sentence);
        foreach(char letter in sentence.ToCharArray())
        {
            DialogueText.text += letter;
            yield return null;
        }

        // Wait 2 seconds before displaying the next sentence.
        yield return new WaitForSeconds(2f);

        DisplayNextSentence();
    }
}

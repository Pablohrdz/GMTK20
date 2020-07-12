using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    // move emissionForce to each individual emitter and enemy?
    public GameObject emitterPrefab;
    public GameObject stampPrefab;
    List<Emitter> emitters;
    Rigidbody2D rb;
    public List<GameObject> pool;
    public bool HasCollectedPearl;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        emitters = new List<Emitter>();
        foreach(Transform child in transform.Find("Emitters"))
        {
            Emitter emitter = child.GetComponent<Emitter>();
            if (emitter != null)
            {
                emitter.letter = InstantiateLetter(emitter);
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
                emitter.disableLetter();
                Vector3 force = -emitter.transform.forward.normalized * emitter.emissionForce;
                rb.AddForceAtPosition(force, emitter.transform.position);
            }
            else
            {
               emitter.enableLetter();
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SwapLetters();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            Destroy(collision.gameObject); // TODO: animate, remember to disable collider while it fades

            // TODO: there should only be one hit, but we should double check...
            ContactPoint2D contact = collision.contacts[0];

            Debug.DrawRay(contact.point, contact.normal, Color.green, 2, false);
            Vector2 point = contact.point;
            GameObject emitterGO = Instantiate(
                emitterPrefab,
                new Vector3(point.x, point.y, 0),
                Quaternion.LookRotation(-contact.normal),
                transform.Find("Emitters"));
            Emitter emitter = emitterGO.GetComponent<Emitter>();
            emitter.setLinkedKey(collision.gameObject.GetComponent<Enemy>().linkedKey);
            emitter.emissionForce = enemy.emissionForce;
            emitter.letter = InstantiateLetter(emitter);
            emitter.enableLetter();
            emitters.Add(emitter.GetComponent<Emitter>());
        }
        var swapper = collision.gameObject.GetComponent<Swapper>();
        if (swapper != null)
        {
            SwapLetters();
            Destroy(collision.gameObject); // TODO: animate, remember to disable collider while it fades
        }
    }

    private GameObject InstantiateLetter(Emitter emitter)
    {
        if (stampPrefab == null)
            throw new System.Exception("missing stamp prefab");
        GameObject letter = Instantiate(
            stampPrefab,
            new Vector3(emitter.transform.position.x, emitter.transform.position.y, 0),
            Quaternion.identity,
            transform.Find("Letters"));
        letter.transform.Find("Text").GetComponent<TextMesh>().text = emitter.linkedKey.ToString();
        return letter;
    }

    private void SwapLetters()
    {
        if (transform.Find("Emitters").childCount < 2)
            return;
        var letter1 = Random.Range(0,transform.Find("Emitters").childCount);
        var letter2 = Random.Range(0,transform.Find("Emitters").childCount);
        while(letter1 == letter2)
            letter2 = Random.Range(0, transform.Find("Emitters").childCount);
        //Aqui ya se decidio cuales
        Emitter em1 = transform.Find("Emitters").GetChild(letter1).GetComponent<Emitter>();
        KeyCode kc1 = em1.linkedKey;
        Emitter em2 = transform.Find("Emitters").GetChild(letter2).GetComponent<Emitter>();
        KeyCode kc2 = em2.linkedKey;

        AssignLetterToEmitter(em1, kc2);
        AssignLetterToEmitter(em2, kc1);

    }

    private void AssignLetterToEmitter(Emitter emitter, KeyCode kc)
    {
        emitter.linkedKey = kc;
        Object.Destroy(emitter.letter);
        emitter.letter = InstantiateLetter(emitter);
    }

}

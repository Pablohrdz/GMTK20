using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Emitter : MonoBehaviour
{
    public KeyCode linkedKey;
    public float emissionForce;
    GameObject letter;
    public GameObject stampPrefab;
    public bool active = true;
    ParticleSystem particles;
    public float particleIntensityMultiplier = 30;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = emissionForce * particleIntensityMultiplier;
    }
    public void Update()
    {
        letter.transform.Find("Text").transform.up = Vector2.up;
    }
    public void enableParticles(bool enable)
    {
        ParticleSystem.EmissionModule emission = particles.emission;
        emission.enabled = enable;
    }

    public void setLinkedKey(KeyCode keyCode)
    {
        linkedKey = keyCode;
    }

    public void enableLetter()
    {
        var color = letter.transform.Find("Sticker").GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        letter.transform.Find("Sticker").GetComponent<SpriteRenderer>().color = color;
    }

    public void disableLetter()
    {
        var color = letter.transform.Find("Sticker").GetComponent<SpriteRenderer>().color;
        color.a = .5f;
        letter.transform.Find("Sticker").GetComponent<SpriteRenderer>().color = color;
    }

    public void swapLetterWith(Transform otherTransform, KeyCode kc, float newForce)
    {
        linkedKey = kc;
        emissionForce = newForce;

        StartCoroutine(SwapLetterPosition(otherTransform));
    }

    IEnumerator SwapLetterPosition(Transform otherTransform)
    {
        active = false;
        while ((letter.transform.position - otherTransform.position).sqrMagnitude > 0.01f)
        {
            letter.transform.position = Vector3.Lerp(
                letter.transform.position, otherTransform.position, 3 * Time.deltaTime);
            yield return null;
        }
        active = true;
        InstantiateLetter();
    }

    public void InstantiateLetter()
    {
        if (stampPrefab == null) { throw new System.Exception("missing stamp prefab"); }

        Object.Destroy(letter);
        letter = Instantiate(
            stampPrefab,
            transform.position,
            Quaternion.identity,
            transform);
        letter.transform.Find("Text").GetComponent<TextMesh>().text = linkedKey.ToString();
        //letter.transform.Find("Text").GetComponent<MeshRenderer>().sortingOrder = 10;
    }
}
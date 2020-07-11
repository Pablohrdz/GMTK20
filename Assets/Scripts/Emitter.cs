using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Emitter : MonoBehaviour
{
    public KeyCode linkedKey;
    public float emissionForce;
    public GameObject letter;

    // this should start out as false if we end up using preset positions for the punctures
    // we can delete it if we don't end up doing that.
    public bool active = true;
    ParticleSystem particles;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
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
        letter.GetComponent<SpriteRenderer>().enabled = true;
    }
    public void disableLetter()
    {
        letter.GetComponent<SpriteRenderer>().enabled = false;
    }

}

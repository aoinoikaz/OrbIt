using UnityEngine;

public class Net : MonoBehaviour
{
    // The nets sprite renderer
    private SpriteRenderer netRenderer;

    // The sprite source
    public Sprite spriteColour;

    // A representation of the net's colour that is comparable to orbs
    public OrbType myNetColour;

    // This will control the orbs audio
    private AudioSource audioController;

    // Public so we can assign them in inspector and play them at runtime with only 1 audio source
    public AudioClip goalClip;
    public AudioClip deathClip;


    private void Start()
    {
        // Get this nets sprite renderer
        netRenderer = GetComponent<SpriteRenderer>();

        audioController = GetComponent<AudioSource>();

        // Set a reference to the net colour
        netRenderer.sprite = spriteColour;

        myNetColour = OrbInfo.DetermineColourType(spriteColour.ToString()[0]);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the colour from the incoming orb
        Orb incomingOrb = collision.gameObject.GetComponent<Orb>();

        // ensure it's an orb
        if (incomingOrb != null)
        {
            // Check if the colours match
            if (incomingOrb.Colour == myNetColour)
            {
                audioController.PlayOneShot(goalClip);
                GameManager.Instance.Points++;
            }
            else
            {
                audioController.PlayOneShot(deathClip);
            }
        }

        // Properly destroy the orb
        incomingOrb.Destroy(0);
    }


    public void Destroy(int seconds)
    {
        if (seconds != 0)
            Destroy(gameObject, seconds);
        else
            Destroy(gameObject);
    }
}

using UnityEngine;

public class Net : MonoBehaviour
{
    // The nets sprite renderer
    private SpriteRenderer netRenderer;

    // The sprite source
    public Sprite spriteColour;

    // A representation of the net's colour that is comparable to orbs
    public OrbType myNetColour;

    private void Start()
    {
        // Get this nets sprite renderer
        netRenderer = GetComponent<SpriteRenderer>();

        // Set a reference to the net colour
        netRenderer.sprite = spriteColour;

        myNetColour = OrbInfo.DetermineColourType(spriteColour.ToString()[0]);
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the colour from the incoming orb
        Orb incomingOrb = collision.gameObject.GetComponent<Orb>();

        // Assure it's an orb
        if (incomingOrb != null)
        {
            // Check if the colours match
            if (incomingOrb.Colour == myNetColour)
            {
                GameManager.Instance.Points++;
            }
            else
            {
                GameManager.Instance.Lives--;
            }

            // Properly destroy the orb
            incomingOrb.Destroy(0, false);
        }
    }


    public void Destroy(int seconds)
    {
        if (seconds != 0)
            Destroy(gameObject, seconds);
        else
            Destroy(gameObject);
    }
}

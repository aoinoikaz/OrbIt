using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;


// This class provides functionality to an individual orb. Dragging, throwing and collision validation
public class Orb : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // The lifetime of the orb in seconds
    public float Lifetime { get; set; }

    // Whether or not this orb can be interacted with/thrown
    public bool IsThrowable { get; set; }

    public bool LinePassed { get; set; }

    // Whether or not this orbs position is static
    public bool IsFrozen { get; set; }

    // An identifier that represents it's position on the orb grid
    public KeyValuePair<int, int> ID;

    // A public reference to the orbs generic colour
    public OrbType Colour;

    // Simply the orb colour
    private Sprite spriteColour;

    // Reference to the orb sprites
    private Sprite[] orbColours = new Sprite[5];

    private Vector3 startPosition;
    private Vector3 offsetToMouse;
    private float distanceToCamera;

    private const float MAX_SPEED = 9;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D orbRigidbody;

    // Use this for initialization
    void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        orbRigidbody = GetComponent<Rigidbody2D>();

        // Load and randomize colour
        orbColours = Resources.LoadAll<Sprite>("Graphics/Orbs");
        spriteColour = orbColours[Random.Range(0, 5)];

        // Start rendering the assigned random colour
        spriteRenderer.sprite = spriteColour;

        // Determine colour code link
        Colour = OrbInfo.DetermineColourType(spriteColour.ToString()[0]);

        Lifetime = 2;
    }

    private void Update()
    {
        if (this.transform.position.y >= GameUI.Instance.ThrowLine.transform.position.y)
        {
            LinePassed = true;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsThrowable)
            return;

        startPosition = transform.position;
        distanceToCamera = Mathf.Abs(startPosition.z - Camera.main.transform.position.z);
        
        offsetToMouse = startPosition - Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToCamera)
        );
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
            return;

        if (!IsThrowable)
            return;

        if (LinePassed)
            Destroy(0);

        orbRigidbody.velocity = eventData.delta;

        // Move the orb based on where the user is touching the screen
        orbRigidbody.MovePosition(Camera.main.ScreenToWorldPoint(
       new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToCamera)
       ) + offsetToMouse);

        // Normalize the speed if it reaches the max limit
        if (orbRigidbody.velocity.magnitude > MAX_SPEED)
        {
            orbRigidbody.velocity = eventData.delta.normalized * MAX_SPEED;
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsThrowable)
            return;

        if (orbRigidbody.velocity.magnitude == 0)
        {
            Destroy(3);
        }

        // They have touched the orb at least once
        IsThrowable = false;
        offsetToMouse = Vector3.zero;
    }


    // This function removes position constraints
    public void Defrost()
    {
        IsFrozen = false;
        orbRigidbody.GetComponent<CircleCollider2D>().enabled = true;
    }


    // This function freezes the position of the orb
    public void Freeze()
    {
        IsFrozen = true;
        orbRigidbody.GetComponent<CircleCollider2D>().enabled = false;
    }


    public void ManuallySetID(int x, int y)
    {
        ID = new KeyValuePair<int, int>(x, y);
    }


    public void Destroy(int seconds)
    {
        // Deduce the amount of orbs left in the active row
        RowManager.Instance.OrbsLeftInRow--;
        RowManager.Instance.CleanUpOrb(ID.Key, ID.Value);

        if (seconds != 0)
            Destroy(gameObject, seconds);
        else
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;


// This class provides functionality to an individual orb. Dragging, throwing and collision validation
public class Orb : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Reference to this orbs physics rigidbody and it's rendering component
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D orbRigidbody;
    private CircleCollider2D circleCollider2d;

    // The text component that is used to render the orbs amount of time left
    private TextMesh orbTimer;

    // Simply the orb colour
    private Sprite spriteColour;

    // Used for orb physics
    private Vector3 startPosition;
    private Vector3 offsetToMouse;
    private float distanceToCamera;

    private string TimerText;

    // The max speed the orb can move
    private const float MAX_SPEED = 9.15f;

    // An identifier that represents this orbs position on the orb grid
    public KeyValuePair<int, int> ID;

    // A public reference to the orbs generic colour
    public OrbType Colour;

    // The lifetime of the orb in seconds
    public float Lifetime { get; set; }

    // Whether or not this orb can be interacted with/thrown
    public bool IsThrowable { get; set; }

    // Whether or not this orb has passed the throw line or not
    public bool LinePassed { get; set; }

    // Whether or not this orb is currently being touched
    public bool Touched { get; set; }

    // Whether or not this orbs position is static
    public bool IsFrozen { get; set; }

    // Use this for initialization
    void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        orbRigidbody = GetComponent<Rigidbody2D>();
        circleCollider2d = orbRigidbody.GetComponent<CircleCollider2D>();
        orbTimer = GetComponentInChildren<TextMesh>();

        // Randomly assign a colour to this orb
        spriteColour = ResourceManager.OrbColours[Random.Range(0, 4)];

        // Start rendering the assigned random colour
        spriteRenderer.sprite = spriteColour;

        // Determine colour code link
        Colour = OrbInfo.DetermineColourType(spriteColour.ToString()[0]);

        Lifetime = 2;
    }


    // Will be used for handling time and ui state
    private void Update()
    {
        // Start timer once the orb has been touched
        if (Touched)
        {
            Lifetime -= Time.deltaTime;

            TimerText = Mathf.Round(Lifetime).ToString();

            orbTimer.text = TimerText;

            // Ensure it hasn't reached negative time
            if (Lifetime < 0)
            {
                Lifetime = 0;
                Destroy(0);
            }
            
        }
    }


    // Fixed update for physics.. better for frame rate and rendering optimization
    private void FixedUpdate()
    {
        // Check if this orb has passed the throw line
        if (!LinePassed && transform.position.y >= GameUI.Instance.ThrowLine.transform.position.y)
        {
            LinePassed = true;
            IsThrowable = false;

            // We know the orb stopped on the line
            if (orbRigidbody.velocity.magnitude == 0)
            {
                Destroy(0);
            }
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        Touched = true;

        if (!IsThrowable)
        {
            return;
        }                   

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

        // Move the orb based on where the user is touching the screen
        orbRigidbody.MovePosition(Camera.main.ScreenToWorldPoint(
       new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToCamera)
       ) + offsetToMouse);
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsThrowable)
            return;

        // Destroy if it's sitting on the line
        if (orbRigidbody.velocity.magnitude == 0 && LinePassed)
        {
            Destroy(0);
        }

        if (orbRigidbody.velocity.magnitude == 0)
        {
            orbRigidbody.velocity = eventData.delta.normalized * 1;
        }

        // Ensure orb doesn't exceed max velocity
        orbRigidbody.velocity = 
            eventData.delta.magnitude > MAX_SPEED ? 
            eventData.delta.normalized * MAX_SPEED : 
            eventData.delta;

        offsetToMouse = Vector3.zero;

    }


    // This function removes position constraints
    public void Defrost()
    {
        IsFrozen = false;
        circleCollider2d.enabled = true;
    }

    // This function freezes the position of the orb
    public void Freeze()
    {
        IsFrozen = true;
        circleCollider2d.enabled = false;
    }


    public void ManuallySetID(int x, int y)
    {
        ID = new KeyValuePair<int, int>(x, y);
    }


    public void Destroy(int seconds)
    {
        // Deduce the amount of orbs left in the active row
        RowManager.Instance.OrbsLeftInActiveRow--;
        RowManager.Instance.CleanUpOrb(ID.Key, ID.Value);

        if (seconds != 0)
            Destroy(gameObject, seconds);
        else
            Destroy(gameObject);
        
    }
}

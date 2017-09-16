using System.Collections.Generic;
using UnityEngine;


public class RowManager : MonoBehaviour
{
    // The singleton instance of the rowmanager
    public static RowManager Instance;

    // How many orbs are left in the active row
    private int orbsLeft;

    private const int AmountOfRows = 3;
    private const int AmountOfOrbsPerRow = 4;

    private float xOrbOffset = 1.6f;
    private float yOrbOffset = 1.05f;

    public delegate void ShiftDelegate();
    public event ShiftDelegate HandleShift;

    // A reference to all of the orbs in game
    public Orb[,] Rows;
    public Vector3[,] PositionGrid;

    private void Awake()
    {
        // Again, setup singleton
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        // Initialize orb grids
        Rows = new Orb[AmountOfOrbsPerRow, AmountOfRows];
        PositionGrid = new Vector3[AmountOfOrbsPerRow, AmountOfRows];
    }


    // This algorithm spawns the game default of rows with proper layout offset
    public void SpawnRows(Orb orb)
    {
        Orb defaultOrb = null;
        Orb previousOrb = null;
        Orb currentOrb = null;
        Vector2? orbCoordinates = null;

        for (int y = 0; y < AmountOfRows; y++)
        {
            for (int x = 0; x < AmountOfOrbsPerRow; x++)
            {
                // Ensure we're placing at [0,0]
                if (x == 0 && y == 0)
                {
                    // Instantiate current orb
                    currentOrb = Instantiate(orb);
                    defaultOrb = currentOrb;
                }
                else
                {
                    // These turnary expressions determine the coordinates for where to spawn each orb
                    orbCoordinates = new Vector2(
                    
                    // This expression simply determines the x coord
                    x == 0 && y == 1 ? defaultOrb.transform.position.x :
                    x == 0 && y == 2 ? defaultOrb.transform.position.x :
                    previousOrb.transform.position.x + xOrbOffset
                    ,
                    // This expression determines the y coordinate for each orb
                    x == 0 && y == 1 ? defaultOrb.transform.position.y - yOrbOffset :
                    x == 0 && y == 2 ? defaultOrb.transform.position.y - (yOrbOffset * 2) :
                    previousOrb.transform.position.y);

                    // Instantiate the orb with the proper coordinates
                    currentOrb = Instantiate(orb, orbCoordinates.Value, Quaternion.identity);
                }

                // Set the previous orb to the current to be referenced next iteration
                previousOrb = currentOrb;

                // Reset the current orb
                currentOrb = null;

                // Push orb to array
                Rows[x, y] = previousOrb;

                // Cache the position inside the grid
                PositionGrid[x, y] = new Vector3(previousOrb.transform.position.x, previousOrb.transform.position.y, previousOrb.transform.position.z);
            }
        }

        OrbsLeftInRow = 4;

        // Clean up
        previousOrb = null;
        currentOrb = null;
        defaultOrb = null;
        orbCoordinates = null;

        SetOrbIDs();
        Activate();
    }


    // This function shifts each row up 1 space and add a new row to the bottom
    public void Shift(Orb orb)
    {
        OrbsLeftInRow = 4;

        for (int y = 0; y < AmountOfRows; y++)
        {
            for (int x = 0; x < AmountOfOrbsPerRow; x++)
            {
                if (y == 0 || y == 1)
                {
                    // Assign the reference
                    Rows[x, y] = Rows[x, y + 1];
                    // Reinitialize the orbs id
                    Rows[x, y].ManuallySetID(x, y);
                    // Move it's position
                    Rows[x, y].transform.position = PositionGrid[x, y];
                }
                else
                {
                    // Instantiate the new row
                    Rows[x, y] = Instantiate(orb, new Vector3(PositionGrid[x, y].x, PositionGrid[x, y].y, PositionGrid[x, y].z), Quaternion.identity);
                    Rows[x, y].ManuallySetID(x, y);
                }
            }
        }

        Activate();
    }


    // This function activates orbs with interaction and if they should be frozen or not
    public void Activate()
    {
        for (int y = 0; y < AmountOfRows; y++)
        {
            for (int x = 0; x < AmountOfOrbsPerRow; x++)
            {
                // If first row
                if (y == 0)
                {
                    // Enable throwing
                    Rows[x, y].IsThrowable = true;

                    // Set it's layer to the active orb
                    Rows[x, y].gameObject.layer = LayerMask.NameToLayer("ActiveOrb");

                    // Check if it's frozen
                    if (Rows[x, y].IsFrozen)
                    {
                        Rows[x, y].Defrost();
                    }
                }
                else
                {
                    // Freeze the orb
                    Rows[x, y].Freeze();
                }
            }
        }
    }


    // This function assigns each orb to a valid identifier
    public void SetOrbIDs()
    {
        for (int y = 0; y < AmountOfRows; y++)
        {
            for (int x = 0; x < AmountOfOrbsPerRow; x++)
            {
                Rows[x, y].ID = new KeyValuePair<int, int>(x, y);
            }
        }
    }


    // This will handle triggering the row shifting event
    public int OrbsLeftInRow
    {
        get { return orbsLeft; }

        set
        {
            orbsLeft = value;
            
            // Fire the shifting event once there is no more orbs in the active row
            if (orbsLeft == 0 && HandleShift != null)
            {
                HandleShift();
            }
        }
    }


    // This simply cleans up orb identifiers
    public void CleanUpOrb(int x, int y)
    {
        Rows[x, y] = null;
    }
}

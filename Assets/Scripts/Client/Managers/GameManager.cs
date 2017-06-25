using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Whehter or not this game is in progress
    public bool InProgress;

    public int Points { get; set; }

    // Used to get rid of concatenation
    public int OldPoints { get; set; }

    public int Lives { get; set; }

    public int OldLives { get; set; }


    private void Start ()
    {
        // Again, setup singleton
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        // Load resources
        ResourceManager.LoadGameResources();

        // Initialize user interface 
        GameUI.Instance.Setup();

        // Spawn the rows, set orb identifiers and enable interaction
        RowManager.Instance.SpawnRows(ResourceManager.OrbInstance);

        // Spawn nets
        NetManager.Instance.SpawnNets(ResourceManager.NetInstance);

        // Set game state
        InProgress = true;

        // Set initial points
        Points = 0;
        Lives = 3;

        RowManager.Instance.HandleShift += OnShift;

        Debug.Log("GM Init");
    }


    private void Update()
    {
        // Game over if we hve no lives left and the game is in progress
        if (Lives == 0 && InProgress)
            GameOver();
    }


    // This is called automatically by the row manager when a row needs to be shifted
    public void OnShift()
    {
        Debug.Log("Handling delegate shift event");

        if (InProgress)
        {
            // Shift rows
            RowManager.Instance.Shift(ResourceManager.OrbInstance);

            // Shuffle nets
            NetManager.Instance.Shuffle();
        }
    }


    public void GameOver()
    {
        // Set game state to not in progress
        InProgress = false;

        // obtain the clones
        var netClones = FindObjectsOfType<Net>();
        var orbClones = FindObjectsOfType<Orb>();

        // Clear all the nets
        foreach (Net net in netClones)
        {
            net.Destroy(0);
        }

        // clear all orbs
        foreach(Orb orb in orbClones)
        {
            orb.Destroy(0, false);
        }

        // render game over ui
        GameUI.Instance.ShowGameOverPanel();
    }
}

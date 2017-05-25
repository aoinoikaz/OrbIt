﻿using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // An instance holder for the default orb
    public Orb OrbInstance;

    // Whehter or not this game is in progress
    public bool InProgress;

    public int Points;

    private void Awake ()
    {
        // Again, setup singleton
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        InitializeGame();
    }


    private void Update()
    {
       // If there is no orbs left in the active row and the system isn't already in progress of shifting a row
        if (InProgress && RowManager.Instance.OrbsLeftInRow == 0 && !RowManager.Instance.IsShifting)
        {
            // Shift rows
            RowManager.Instance.Shift(OrbInstance);
        }

        if (Points == 0 || GameUI.Instance.TimeLeft < 1)
        {
            GameOver();
        }
    }


    public void InitializeGame()
    {
        // Load resources
        // TODO: make a resource manager
        if (OrbInstance == null)
            OrbInstance = Resources.Load<Orb>("Prefabs/Orb");

        // Initialize user interface 
        GameUI.Instance.SetupUI();

        // Spawn the rows, set orb identifiers and enable interaction
        RowManager.Instance.SpawnRows(OrbInstance);
        RowManager.Instance.SetOrbIDs();
        RowManager.Instance.Activate();

        // Set game state
        InProgress = true;

        // Set initial points
        Points = 1;
    }


    public void Add()
    {
        Points++;
        GameUI.Instance.TimeLeft++;
    }


    public void Deduct()
    {
        Points--;
        GameUI.Instance.TimeLeft--;
    }


    public void GameOver()
    {
        // Set game state to not in progress
        InProgress = false;

        // Reset time
        GameUI.Instance.TimeLeft = 0;

        // Update ui
        GameUI.Instance.TimeLeftLabel.text = "Game Over";

        GameUI.Instance.ShowGameOverPanel();
    }
}

using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Whehter or not this game is in progress
    public bool InProgress;

    public int Points { get; set; }

    // Used to get rid of concatenation
    public int OldPoints { get; set; }

    public float GameTimeLeft { get; private set; }

    public delegate void GameOverDelegate();
    public event GameOverDelegate HandleGameOver;

    // Using start instead of awake for initialization so it gives time for the other component dependencies
    private void Start ()
    {
        // Again, setup singleton
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        ResourceManager.LoadGameResources();
        NetManager.Instance.SpawnNets(ResourceManager.NetInstance);
        RowManager.Instance.SpawnRows(ResourceManager.OrbInstance);
        RowManager.Instance.HandleShift += OnShift;

        GameUI.Instance.Subscribe();

        Init(30f);
    }

    private void Init(float gametime)
    {
        // Set game state
        InProgress = true;

        // Set initial points
        OldPoints = -1;
        Points = 0;

        GameTimeLeft = gametime;
    }

    // Render stuff to screen
    private void Update()
    {
        // if there's time left
        if (GameTimeLeft > 0)
        {
            GameTimeLeft -= Time.deltaTime;

            if (GameTimeLeft < 0)
            {
                GameTimeLeft = 0;
                if (HandleGameOver != null)
                {
                    CleanUp();
                    HandleGameOver();
                }
            }
        }
    }


    // This is called automatically by the row manager when a row needs to be shifted
    public void OnShift()
    {
        if (InProgress)
        {
            // Shift rows
            RowManager.Instance.Shift(ResourceManager.OrbInstance);

            // Shuffle nets
            NetManager.Instance.Shuffle();
        }
    }


    public void CleanUp()
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
            orb.Destroy(0);
        }

        Debug.Log("Cleaned up");
    }
}

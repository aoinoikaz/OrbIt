using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameUI : MonoBehaviour
{
    // Instance of this game ui singleton
    public static GameUI Instance;

    // The screen bounds
    public EdgeCollider2D ScreenBounds;

    // Main camera reference
    public Camera MainCamera;

    // Label info
    public Text TimeLeftLabel;
    public Text PointsLabel;

    // UI elements
    public GameObject GameOverPanel;
    public GameObject ThrowLine;

    // Amount to offset the y axis of the throwline
    private const float yOffset = 0.5f;

    // Use this for initialization
    private void Awake ()
    {
        // Setup singleton instance
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        if (PointsLabel == null)
            PointsLabel = GameObject.Find("PointsLabel").GetComponent<Text>();

        if (TimeLeftLabel == null)
            TimeLeftLabel = GameObject.Find("TimeLeftLabel").GetComponent<Text>();

        if(ScreenBounds == null)
            ScreenBounds = GameObject.Find("ScreenBounds").GetComponent<EdgeCollider2D>();

        if (GameOverPanel == null)
            GameOverPanel = GameObject.Find("GameOverPanel");

        if (ThrowLine == null)
            ThrowLine = GameObject.Find("ThrowLine");

        // Ensure its disabled
        if (GameOverPanel.activeInHierarchy)
            GameOverPanel.SetActive(false);

        MainCamera = Camera.main;

        Setup();
    }



    // Render stuff to screen
    private void Update()
    {
        if (GameManager.Instance.Points != GameManager.Instance.OldPoints)
        {
            PointsLabel.text = "Points: " + GameManager.Instance.Points;
            GameManager.Instance.OldPoints = GameManager.Instance.Points;
        }

        float t = GameManager.Instance.GameTimeLeft;
        
        // if there's time left
        if (t > 0)
        {
            TimeLeftLabel.text = Mathf.Round(t).ToString();
        }
    }


    private void Setup()
    {
        
        // Get the coordinates of the the 4 vertex's of the camera viewport
        Vector2 bottomLeft = MainCamera.ScreenToWorldPoint(new Vector3(0, 0, MainCamera.nearClipPlane));
        Vector2 topLeft = MainCamera.ScreenToWorldPoint(new Vector3(0, MainCamera.pixelHeight, MainCamera.nearClipPlane));
        Vector2 topRight = MainCamera.ScreenToWorldPoint(new Vector3(MainCamera.pixelWidth, MainCamera.pixelHeight, MainCamera.nearClipPlane));
        Vector2 bottomRight = MainCamera.ScreenToWorldPoint(new Vector3(MainCamera.pixelWidth, 0, MainCamera.nearClipPlane));

        // Assign the edge point references into a square viewport
        Vector2[] edgePoints = new[] { bottomLeft, topLeft, topRight, bottomRight, bottomLeft };
        ScreenBounds.points = edgePoints;

        // Set the position to the center of the camera's frustrum
        Vector3 pos = Vector3.zero;
        pos.y -= yOffset;
        ThrowLine.transform.position = pos;
    }


    public void Subscribe()
    {
        GameManager.Instance.HandleGameOver += OnGameOver;
    }

    public void OnGameOver()
    {
        GameOverPanel.SetActive(true);
        ThrowLine.SetActive(false);
        PointsLabel.gameObject.SetActive(false);
        TimeLeftLabel.gameObject.SetActive(false);
        GameObject.Find("GameOverScoreLabel").GetComponent<Text>().text = "You scored: " + GameManager.Instance.Points;
    }


    public void Return()
    {
        SceneManager.LoadScene(0);
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameUI : MonoBehaviour
{
    // Instance of this game ui singleton
    public static GameUI Instance;

    // The screen bounds
    public EdgeCollider2D ScreenBounds;

    // Main camera reference
    public Camera MainCamera;

    // Amount of time left in each round
    public float TimeLeft;
    public float StartTime;

    // Label info
    public Text TimeLeftLabel;
    public Text PointsLabel;
    public Text OrbsLeftLabel;

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

        if (OrbsLeftLabel == null)
            OrbsLeftLabel = GameObject.Find("OrbsLeftLabel").GetComponent<Text>();

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

        StartTime = TimeLeft = 30;
    }


    // Render stuff to screen
    private void Update()
    {
        // Ensure the time hasn't ran out
        if (TimeLeft > 0)
        {
            // Subtract a second from the start time
            TimeLeft -= Time.deltaTime;

            // Render the score to screen
            TimeLeftLabel.text = "Time left: " + Mathf.Round(TimeLeft);
            PointsLabel.text = "Points: " + GameManager.Instance.Points;
            OrbsLeftLabel.text = "Orbs left: " + RowManager.Instance.OrbsLeftInRow;
        }
        else
        {
            // Check if the game is in progress
            if (GameManager.Instance.InProgress)
            {
                GameManager.Instance.GameOver();
            }
        }
    }


    public void SetupUI()
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


    public void Return()
    {
        SceneManager.LoadScene(0);
    }


    public void ShowGameOverPanel()
    {
        GameOverPanel.SetActive(true);
        GameObject.Find("GameOverScoreLabel").GetComponent<Text>().text = "You scored: " + GameManager.Instance.Points;
    }

}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    // Amount of time left in each round
    public float TimeLeft;
    public float StartTime;

    // Label for the amount of time left
    public Text TimeLeftLabel;
    public Text PointsLabel;
    public Text OrbsLeftLabel;
    public GameObject GameOverPanel;

    public GameObject ThrowLine;

    public EdgeCollider2D ScreenBounds;

    public Camera MainCamera;

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

        if (GameOverPanel == null)
            GameOverPanel = GameObject.Find("GameOverPanel");

        if (ThrowLine == null)
            ThrowLine = GameObject.Find("ThrowLine");

        if(ScreenBounds == null)
        ScreenBounds = GameObject.Find("ScreenBounds").GetComponent<EdgeCollider2D>();

        // Ensure its disabled
        if (GameOverPanel.activeInHierarchy)
            GameOverPanel.SetActive(false);

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
        MainCamera = Camera.main;

        Vector2 bottomLeft = MainCamera.ScreenToWorldPoint(new Vector3(0, 0, MainCamera.nearClipPlane));
        Vector2 topLeft = MainCamera.ScreenToWorldPoint(new Vector3(0, MainCamera.pixelHeight, MainCamera.nearClipPlane));
        Vector2 topRight = MainCamera.ScreenToWorldPoint(new Vector3(MainCamera.pixelWidth, MainCamera.pixelHeight, MainCamera.nearClipPlane));
        Vector2 bottomRight = MainCamera.ScreenToWorldPoint(new Vector3(MainCamera.pixelWidth, 0, MainCamera.nearClipPlane));

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

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    // A reference to the menu's play now button
    private Button PlayNow;

    // Called before anything else
	private void Awake ()
    {
        if (PlayNow == null)
            PlayNow = GameObject.Find("PlayNowButton").GetComponent<Button>();
	}

    public void Login()
    {
        SceneManager.LoadScene(1);
    }
}

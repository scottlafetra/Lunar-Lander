using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreTextController : MonoBehaviour {

    public PlayerController trackedPlayer;

    private Text scoreText;

    // Use this for initialization
    void Start () {
        scoreText = GetComponent<Text>();

        UpdateScore();
        trackedPlayer.ScoreChanged += new PlayerController.StatusChangedHandler(UpdateScore);
	}
	
	// Update is called once per frame
	private void UpdateScore () {
        scoreText.text = "$" + trackedPlayer.GetScore();
	}
}

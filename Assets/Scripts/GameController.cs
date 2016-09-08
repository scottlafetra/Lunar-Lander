using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

    public Text scoreText;

    public PlayerController NASAPlayer;
    public PlayerController CCCPPlayer;

    public Transform NASASpawn;
    public Transform CCCPSpawn;

    public GameObject menuUI;   //A game object containing all menu UI
    public GameObject inGameUI; //A game object containing all menu UI

    private TransparentSwitch scoreTintSwitch;

    public delegate void StatusChangedHandler();

    public event StatusChangedHandler GameReset;

    public int winningDifference = 50;

    //Singleton pointer
    public static GameController instance;

    void Start() {
        scoreTintSwitch = GetComponent<TransparentSwitch>();
        //Manually trigger menu entry sequence
        OnEnterMenu();

        instance = this;
    }

    public void OnEnterMenu()
    {
        scoreTintSwitch.SetTransparent(true);
        inGameUI.SetActive(false);
        menuUI.SetActive(true);

        //reset the player's position
        NASAPlayer.transform.Translate(NASASpawn.position - NASAPlayer.transform.position);
        CCCPPlayer.transform.Translate(CCCPSpawn.position - CCCPPlayer.transform.position);

        //disable the players
        NASAPlayer.gameObject.SetActive(false);
        CCCPPlayer.gameObject.SetActive(false);

        //subscribe to score updates
        NASAPlayer.ScoreChanged += new StatusChangedHandler(UpdateScore);
        CCCPPlayer.ScoreChanged += new StatusChangedHandler(UpdateScore);

        //subscribe to crashed updates
        NASAPlayer.Crashed += new StatusChangedHandler(UpdateScore);
        CCCPPlayer.Crashed += new StatusChangedHandler(UpdateScore);
    }

    public void OnPlay()
    {
        scoreTintSwitch.SetTransparent(false);
        menuUI.SetActive(false);
        inGameUI.SetActive(true);

        //enable the players
        NASAPlayer.gameObject.SetActive(true);
        CCCPPlayer.gameObject.SetActive(true);

        UpdateScore();
    }

    public void OnExit()
    {
        Application.Quit();
    }

    public void UpdateScore () {
        scoreText.text  = "NASA Budget: $" + NASAPlayer.GetScore() + "\n";
        scoreText.text += "CCCP Budget: $" + CCCPPlayer.GetScore() + "\n";

        if (NASAPlayer.IsDead()) {
            scoreText.text += "NASA Hull Wrecked!\n";
        }

        if (CCCPPlayer.IsDead()) {
            scoreText.text += "CCCP Hull Wrecked!\n";
        }
    }
}

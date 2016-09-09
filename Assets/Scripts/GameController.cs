using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

    public Text scoreText;

    public PlayerController NASAPlayer;
    public PlayerController CCCPPlayer;

    public Transform NASASpawn;
    public Transform CCCPSpawn;

    public FlagController NASAFlag;
    public FlagController CCCPFlag;
    public FlagController TieFlag;

    public GameObject menuUI;   //A game object containing all menu UI
    public GameObject inGameUI; //A game object containing all menu UI

    private bool isReseting;

    private TransparentSwitch scoreTintSwitch;

    public delegate void StatusChangedHandler();

    public event StatusChangedHandler GameReset;

    public int winningDifference = 50;

    private Rigidbody NASARigidbody;
    private Rigidbody CCCPRigidbody;

    //Singleton pointer
    public static GameController instance;

    void Awake() {
        scoreTintSwitch = GetComponent<TransparentSwitch>();
        //Manually trigger menu entry sequence
        OnEnterMenu();
        isReseting = false;

        NASARigidbody = NASAPlayer.GetComponent<Rigidbody>();
        CCCPRigidbody = CCCPPlayer.GetComponent<Rigidbody>();

        //subscribe to score updates
        NASAPlayer.ScoreChanged += new StatusChangedHandler(UpdateScore);
        CCCPPlayer.ScoreChanged += new StatusChangedHandler(UpdateScore);

        //subscribe to crashed updates
        NASAPlayer.Crashed += new StatusChangedHandler(OnPlayerCrash);
        CCCPPlayer.Crashed += new StatusChangedHandler(OnPlayerCrash);

        //Subscribe to reset finished
        NASAFlag.ResetFinished += new StatusChangedHandler(OnReset);
        CCCPFlag.ResetFinished += new StatusChangedHandler(OnReset);
        TieFlag.ResetFinished += new StatusChangedHandler(OnReset);

        instance = this;
    }

    public void OnEnterMenu()
    {
        scoreTintSwitch.SetTransparent(true);
        inGameUI.SetActive(false);
        menuUI.SetActive(true);

        //disable the players
        NASAPlayer.gameObject.SetActive(false);
        CCCPPlayer.gameObject.SetActive(false);
    }

    public void OnPlay()
    {
        scoreTintSwitch.SetTransparent(false);
        menuUI.SetActive(false);
        inGameUI.SetActive(true);

        //reset the player's position and rotation
        NASAPlayer.transform.position = NASASpawn.position;
        CCCPPlayer.transform.position = CCCPSpawn.position;

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

    public void OnPlayerCrash()
    {
        if (isReseting)
        {//Do nothing
            return;
        }
        else
        {
            isReseting = true;
        }

        if (NASAPlayer.GetScore() > CCCPPlayer.GetScore())
        {
            NASAFlag.gameObject.SetActive(true);
        }
        else if (CCCPPlayer.GetScore() > NASAPlayer.GetScore())
        {
            CCCPFlag.gameObject.SetActive(true);
        }
        else
        {
            TieFlag.gameObject.SetActive(true);
        }
    }

    public void OnReset()
    {
        GameReset();//tell the others
        OnPlay();//Reset stuff ourselves

        isReseting = false;
    }
}

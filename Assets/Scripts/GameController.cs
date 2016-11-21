using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public SpinScored spinScorer;
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

    public AudioSource themeMusic;
    public AudioSource battleMusic;

    private bool isInMenu;

    public StarCounter starCounter;

    //Singleton pointer
    public static GameController instance;

    public List<GameObject> landingPads;
    public float padHeightVariance;
    public float padHeightBase;
    public float padHeightCurve;
    public float padSpacingMin;
    public float padSpacingMax;
    public float edgeOfScreen;

    public float scoreMultiplier = 1;

    public float padValueMin;
    public float padValueMax;

    void Awake() {
        scoreTintSwitch = GetComponent<TransparentSwitch>();
        //Manually trigger menu entry sequence
        OnEnterMenu();
        isReseting = false;

        NASARigidbody = NASAPlayer.GetComponent<Rigidbody>();
        CCCPRigidbody = CCCPPlayer.GetComponent<Rigidbody>();

        //subscribe to score updates
        spinScorer.PlayerWon += new StatusChangedHandler(OnPlayerWin);

        NASAPlayer.ScoreChanged += new StatusChangedHandler(UpdateScore);
        CCCPPlayer.ScoreChanged += new StatusChangedHandler(UpdateScore);

        //subscribe to crashed updates
        NASAPlayer.Crashed += new StatusChangedHandler(OnPlayerCrash);
        CCCPPlayer.Crashed += new StatusChangedHandler(OnPlayerCrash);

        //Subscribe to reset finished
        NASAFlag.ResetFinished += new StatusChangedHandler(OnReset);
        CCCPFlag.ResetFinished += new StatusChangedHandler(OnReset);
        TieFlag.ResetFinished += new StatusChangedHandler(OnReset);

        //subscribe to wins
        

        instance = this;
    }

    void Update()
    {
        if (!isInMenu && Input.GetButtonDown("Menu"))
        {
            OnEnterMenu();
        }
    }

    public void OnEnterMenu()
    {
        isInMenu = true;

        scoreTintSwitch.SetTransparent(true);
        inGameUI.SetActive(false);
        menuUI.SetActive(true);

        //disable the players
        NASAPlayer.gameObject.SetActive(false);
        CCCPPlayer.gameObject.SetActive(false);

        battleMusic.Stop();
        themeMusic.Play();

        //reset round scores
        starCounter.ChangeStars(-starCounter.GetScore());
    }

    public void OnPlay()
    {
        isInMenu = false;

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

        themeMusic.Stop();
        battleMusic.Play();

        //procederally gennerate the level
        ResetPlatforms();
    }

    private void ResetPlatforms()
    {
        //space horizontally
        int padsInPlay = -1; //the amount of pads being used
        float currentBase = -edgeOfScreen -padSpacingMin;//start from the left and work right
        foreach (GameObject landingPad in landingPads)
        {
            landingPad.transform.position = new Vector3(currentBase + Random.Range( padSpacingMin, padSpacingMax), 0);

            //setup next placement
            currentBase = landingPad.transform.position.x;

            //if partly offscreen, move to trash area
            if( landingPad.transform.position.x > edgeOfScreen )
            {
                if(padsInPlay == -1 )//if has not been set
                {
                    padsInPlay = landingPads.IndexOf( landingPad );
                }

                landingPad.transform.position = new Vector3( edgeOfScreen + 20, 0 );
            }
        }
        //space vertically
        foreach (GameObject landingPad in landingPads)
        {
            landingPad.transform.position = new Vector3(
                landingPad.transform.position.x,
                Mathf.Pow(Mathf.Abs(landingPad.transform.position.x), padHeightCurve)
                    + Random.Range(-padHeightVariance, padHeightVariance)
                    + padHeightBase
                );
        }

        //score the pads
        float padValueMid = ( padValueMax + padValueMin ) / 2;
        for( int i = 0; i < padsInPlay; ++i )
        {
            //calculate how far below the other pads this one is
            float padDip =  ( landingPads[ (int) ModulousScreenBound.ActualModulo(i - 1, padsInPlay ) ].transform.position.y 
                            + landingPads[ (int)ModulousScreenBound.ActualModulo( i + 1, padsInPlay ) ].transform.position.y ) / 2
                            - landingPads[ i ].transform.position.y;

            landingPads[ i ].GetComponent<LandingPadController>().value = (int)Mathf.Clamp( padValueMid + padDip * scoreMultiplier, padValueMin, padValueMax );

            Debug.Log( "Score: " + landingPads[ i ].GetComponent<LandingPadController>().value + " -> " + (i + 1) + " Out of " + padsInPlay );
        }
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

    public void OnPlayerWin()
    {
        if (!isReseting)
        {//Do nothing
            ProcessEndgame();
        }
    }

    public void OnPlayerCrash()
    {
        if (!isReseting && (NASAPlayer.IsDead() && CCCPPlayer.IsDead()) )
        {
            ProcessEndgame();
        }
    }

    private void ProcessEndgame()
    {
        isReseting = true;

        if (NASAPlayer.GetScore() > CCCPPlayer.GetScore())
        {
            NASAFlag.gameObject.SetActive(true);
            starCounter.ChangeStars(1);
        }
        else if (CCCPPlayer.GetScore() > NASAPlayer.GetScore())
        {
            CCCPFlag.gameObject.SetActive(true);
            starCounter.ChangeStars(-1);
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

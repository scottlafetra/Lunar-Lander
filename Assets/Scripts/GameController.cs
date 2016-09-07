using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

    public Text scoreText;

    public PlayerController NASAPlayer;
    public PlayerController NCCPPlayer;

    public Transform NASASpawn;
    public Transform NCCPSpawn;

    public GameObject menuUI; //A game object containing all menu UI
    public GameObject inGameUI; //A game object containing all menu UI

    private TransparentSwitch scoreTintSwitch;


    void Start() {
        scoreTintSwitch = GetComponent<TransparentSwitch>();
        //Manually trigger menu entry sequence
        OnEnterMenu();
    }

    public void OnEnterMenu()
    {
        scoreTintSwitch.SetTransparent(true);
        inGameUI.SetActive(false);
        menuUI.SetActive(true);

        //reset the player's position
        NASAPlayer.transform.Translate(NASASpawn.position - NASAPlayer.transform.position);
        NCCPPlayer.transform.Translate(NCCPSpawn.position - NCCPPlayer.transform.position);

        //disable the players
        NASAPlayer.gameObject.SetActive(false);
        NCCPPlayer.gameObject.SetActive(false);
    }

    public void OnPlay()
    {
        scoreTintSwitch.SetTransparent(false);
        menuUI.SetActive(false);
        inGameUI.SetActive(true);

        //enable the players
        NASAPlayer.gameObject.SetActive(true);
        NCCPPlayer.gameObject.SetActive(true);
    }

    public void OnExit()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update () {
        scoreText.text  = "NASA Budget: $" + NASAPlayer.GetScore() + "\n";
        scoreText.text += "NCCP Budget: $" + NCCPPlayer.GetScore() + "\n";

        if (NASAPlayer.IsDead()) {
            scoreText.text += "NASA Hull Wrecked!\n";
        }

        if (NCCPPlayer.IsDead()) {
            scoreText.text += "NCCP Hull Wrecked!\n";
        }
    }
}

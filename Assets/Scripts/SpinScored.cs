using UnityEngine;
using System.Collections;

public class SpinScored : MonoBehaviour {

    public PlayerController playerA;
    public PlayerController playerB;

    public float changeSpeed = 1.0f;

    public event GameController.StatusChangedHandler PlayerWon;

    // Update is called once per frame
    void Update () {
        //Get the percent to turn
        float percentWinning = (playerA.GetScore() - playerB.GetScore()) /(float) GameController.instance.winningDifference;
        percentWinning = Mathf.Clamp(percentWinning, -1, 1);

        //Get disired rotation
        Quaternion correctRotation = Quaternion.Euler(0, Mathf.Asin(percentWinning) * Mathf.Rad2Deg, 0);

        //Rotate with a lerp
        transform.rotation = Quaternion.Lerp(transform.rotation, correctRotation, changeSpeed * Time.deltaTime);

        //Call a win if apropriate
        if(Mathf.Abs(percentWinning) == 1)
        {
            PlayerWon();
        }
    }
}

using UnityEngine;
using System.Collections;

public class LandingPadController : MonoBehaviour {

    public int value = 10;
    private bool landedOn = false;

    private Renderer myRenderer;

    void Start() {
        myRenderer = GetComponent<Renderer>();
        GameController.instance.GameReset += new GameController.StatusChangedHandler(Reset);
    }

    public void landOn(bool isNASA) {
        landedOn = true;

        if (isNASA) {
            myRenderer.material.color = Color.blue;

        } else {
            myRenderer.material.color = Color.red;
        }
    }

    public bool isLandedOn() {
        return landedOn;
    }

    public void Reset()
    {
        landedOn = false;
        myRenderer.material.color = Color.white;
    }
}

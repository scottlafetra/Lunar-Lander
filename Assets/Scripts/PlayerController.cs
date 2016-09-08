using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public int playerNumber = 1;

    public GameObject visuals;

    public float rotateSpeed = -180.0f;
    public float acceleration = 10.0f;

    public float maxCollideSpeed = 2;
    public float maxTilt = 45;

    public ParticleSystem flames;
    public ParticleSystem delfectionSmoke;
    private bool isFiring;

    private int score = 0;
    private bool isDead = false;

    private Rigidbody myRigidbody;

    public delegate void StatusChangedHandler();
    public event StatusChangedHandler ScoreChanged;
    public event StatusChangedHandler Crashed;

    // Use this for initialization
    void Start () {
        myRigidbody = GetComponent<Rigidbody>();

        flames.simulationSpace          = ParticleSystemSimulationSpace.World;
        delfectionSmoke.simulationSpace = ParticleSystemSimulationSpace.World;

        //set to NASA or CCCP
        if(playerNumber == 1) {
            visuals.GetComponent<Renderer>().material.color = Color.blue;

        } else {
            visuals.GetComponentInChildren<Renderer>().material.color = Color.red;
        }

        isFiring = false;
    }

    // Update is called once per frame
    void Update () {

        if (!isDead) {
            if(Input.GetAxisRaw("Rotation_" + playerNumber) != 0)//if rotating
            {
                //kill rotation speed
                myRigidbody.angularVelocity = Vector3.zero;

                //rotate
                transform.Rotate(0, 0, Input.GetAxisRaw("Rotation_" + playerNumber) * rotateSpeed * Time.deltaTime);
            }

            if (Input.GetButton("Thrust_" + playerNumber)) {
                myRigidbody.velocity += transform.up * acceleration * Time.deltaTime;

                if (!isFiring)
                {
                    setFiring(true);
                }

            } else if (isFiring)
            {
                setFiring(false);
            }
            
        }
    }

    void OnCollisionEnter(Collision collision) {

        Debug.Log("Collision (" + collision.gameObject + "): " + collision.relativeVelocity.magnitude);

        float relTilt = Mathf.Min(transform.rotation.eulerAngles.z, 360 - transform.rotation.eulerAngles.z);

        if(collision.gameObject.tag != "Player") {
            if (collision.relativeVelocity.magnitude >= maxCollideSpeed || relTilt >= maxTilt) {
                isDead = true;
                setFiring(false);
                Crashed();

            } else {
                LandingPadController landingPad = collision.gameObject.GetComponent<LandingPadController>();

                //if is landing pad
                if (landingPad != null && !landingPad.isLandedOn() && !isDead) {
                    score += landingPad.value;

                    landingPad.landOn(playerNumber == 1);

                    ScoreChanged();
                }
            }
        }
        
    }

    public int GetScore() {
        return score;
    }

    public bool IsDead() {
        return isDead;
    }

    private void setFiring(bool on) {

        if (on) {
            flames.Play();
            isFiring = true;

        } else {
            flames.Stop();
            isFiring = false;
        }
    }

    public bool IsFiring() {
        return isFiring;
    }
}

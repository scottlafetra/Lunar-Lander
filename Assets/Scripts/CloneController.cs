using UnityEngine;
using System.Collections;

public class CloneController : MonoBehaviour {

    public PlayerController source;

    public float xOffset;

    public ParticleSystem flames;
    public ParticleSystem deflectionSmoke;

    public GameObject visuals;

    // Use this for initialization
    void Start() {

        flames.simulationSpace = ParticleSystemSimulationSpace.World;
        deflectionSmoke.simulationSpace = ParticleSystemSimulationSpace.World;

        //set to NASA or NCCP
        if (source.playerNumber == 1) {
            visuals.GetComponent<Renderer>().material.color = Color.blue;

        }
        else {
            visuals.GetComponentInChildren<Renderer>().material.color = Color.red;
        }
    }

    // Update is called once per frame
    void LateUpdate() {

        //copy position with an offset
        transform.position = new Vector3(
            source.transform.position.x + xOffset,
            source.transform.position.y,
            0
            );

        //copy rotation
        transform.rotation = source.transform.rotation;

        //Update Particles
        if (source.IsFiring()) {
            if (flames.isStopped)
            {
                flames.Play();
            }
            

        }
        else if(flames.isPlaying) {
            flames.Stop();
        }
    }
}

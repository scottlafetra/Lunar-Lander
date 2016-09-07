using UnityEngine;
using System.Collections;

public class HasAngularDrag : MonoBehaviour {

    public float angularDrag;//must be between 0 and 1

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().angularDrag = angularDrag;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

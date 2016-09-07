using UnityEngine;
using System.Collections;

public class LockedToXYPlane : MonoBehaviour {


	void Update () {
        //handle translation
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        //handle rotation
        transform.rotation = Quaternion.Euler(
            0,
            0,
            transform.eulerAngles.z
        );

    
    }

    
}

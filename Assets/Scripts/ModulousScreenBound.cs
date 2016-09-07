using UnityEngine;
using System.Collections;

public class ModulousScreenBound : MonoBehaviour {

    public float width;

	// Update is called once per frame
	void Update () {

        transform.position = new Vector3(
            ActualModulo(transform.position.x + width/2, width) - width/2,
            transform.position.y,
            transform.position.z
            );
	}

    private float ActualModulo(float x, float y) {
        if(x >= 0) {
            return x % y;

        } else {
            return (x % y) + y;
        }
    }
}

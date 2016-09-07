using UnityEngine;
using System.Collections;

public class Transparent : MonoBehaviour {

    public float alpha = 0.5f;
    
	void Awake () {
        MeshRenderer meshRender = GetComponent<MeshRenderer>();
        meshRender.material.color = new Color(
            meshRender.material.color.r,
            meshRender.material.color.g,
            meshRender.material.color.b,
            alpha
            );
	}
	
}

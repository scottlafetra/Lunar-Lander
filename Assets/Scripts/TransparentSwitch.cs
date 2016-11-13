using UnityEngine;
using System.Collections.Generic;

public class TransparentSwitch : MonoBehaviour {

    public List<GameObject> transparentList;

    private List<float> alphaValues;
    private bool isSetup = false;

	// Use this for initialization
	void Setup () {
        alphaValues = new List<float>();

        foreach (GameObject item in transparentList)
        {
            alphaValues.Add( item.GetComponent<Renderer>().material.color.a );
        }
    }
	
	public void SetTransparent(bool makeTransparent)
    {
        //to protect against being called before setup
        if(!isSetup)
        {
            Setup();
            isSetup = true;
        }
        
        for(int i = 0; i < transparentList.Count; ++i)
        {
            GameObject item = transparentList[i];
            Renderer renderer = item.GetComponent<Renderer>();
            Color color = renderer.material.color;

            renderer.material.color = new Color(
                color.r,
                color.g,
                color.b,
                makeTransparent ? 0.0f : alphaValues[i]);
            Debug.Log(alphaValues[i]);
            
        }
    }
}

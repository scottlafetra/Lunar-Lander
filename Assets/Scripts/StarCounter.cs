using UnityEngine;
using System.Collections.Generic;

public class StarCounter : MonoBehaviour {

    public GameObject star;
    public float starSpacing;

    private float xOrigin;

    private int score = 0;
    private List<GameObject> stars = new List<GameObject>();

    void Start()
    {
        xOrigin = transform.position.x;
    }

    public int GetScore()
    {
        return score;
    }

    public void ChangeStars(int changeBy)
    {
        score += changeBy;

        //change score apropriatly
        while (stars.Count < Mathf.Abs(score))
        {
            AddStar();
        }

        while (stars.Count > Mathf.Abs(score))
        {
            RemoveStar();
        }

        //set star colors
        if (score > 0)
        {
            foreach (GameObject star in stars)
            {
                star.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                star.GetComponent<Renderer>().material.SetColor("_TintColor", Color.blue);//Particle shader support
            }
        }
        else
        {
            foreach (GameObject star in stars)
            {
                star.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                star.GetComponent<Renderer>().material.SetColor("_TintColor", Color.red);//Particle shader support
            }
        }
    }

    private void AddStar()
    {
        GameObject newStar = Instantiate(star);
        newStar.transform.SetParent(transform);
        newStar.transform.localPosition = new Vector3(stars.Count * starSpacing, 0);

        stars.Add(newStar);

        Reposition();
    }

    private void RemoveStar()
    {
        //find the last placed star and remove it
        GameObject starToRemove = stars[stars.Count - 1];
        stars.Remove(starToRemove);
        Destroy(starToRemove);

        //correct position
        Reposition();
    }

    private void Reposition()
    {
        transform.position = new Vector3( xOrigin - ( ( stars.Count - 1 ) * starSpacing ) / 2, transform.position.y );
    }
	
}

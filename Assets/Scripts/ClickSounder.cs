using UnityEngine;
using System.Collections;

public class ClickSounder : MonoBehaviour
{
    private AudioSource sound;
    private int i = 0;

    // Use this for initialization
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    public void OnClick()
    {
        Debug.Log( "Click " + i++ );
        sound.Play();
    }
}

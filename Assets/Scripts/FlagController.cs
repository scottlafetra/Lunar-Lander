using UnityEngine;
using System.Collections;

public class FlagController : MonoBehaviour {

    public float timeToReset = 3.0f;

    public event GameController.StatusChangedHandler ResetFinished;
	
	void OnEnable () {
        StartCoroutine(ResetTimer());
	}

    public IEnumerator ResetTimer()
    {
        yield return new WaitForSeconds(timeToReset);
        ResetFinished();
        gameObject.SetActive(false);
    }
}

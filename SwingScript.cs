using System.Collections;
using UnityEngine;

public class SwingScript : MonoBehaviour {

    private void Awake()
    {
        StartCoroutine(endAfter());
    }

    private IEnumerator endAfter () {
        yield return new WaitForSeconds(.5f);
        Destroy(this.gameObject);
	}
}

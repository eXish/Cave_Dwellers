using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlScript : MonoBehaviour {

    public Camera cam;
	
	// Update is called once per frame
	void Update () {
        float d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0f)
        {
            cam.orthographicSize -= 5;
        }
        else if (d < 0f)
        {
            cam.orthographicSize += 5;
        }
        if(Input.GetKey(KeyCode.W))
        {
            cam.transform.position += new Vector3(0, 5, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            cam.transform.position += new Vector3(-5, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            cam.transform.position += new Vector3(0, -5, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            cam.transform.position += new Vector3(5, 0, 0);
        }
    }
}

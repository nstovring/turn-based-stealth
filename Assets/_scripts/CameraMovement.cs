using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{

    public Transform PlayerPosition;
    public Vector3 CameraDistance;



    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
        if(Input.GetAxis("Horizontal") > 0.1 || Input.GetAxis("Horizontal") < -0.1 || Input.GetAxis("Vertical") > 0.1 || Input.GetAxis("Vertical") < -0.1)
	    {
	        
	    }
        CameraDistance += new Vector3(Input.GetAxis("Vertical")*-1, 0, Input.GetAxis("Horizontal"));
	    transform.position = Vector3.Lerp(transform.position,PlayerPosition.position + CameraDistance,0.1f);

	}
}

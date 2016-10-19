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

	    transform.position = PlayerPosition.position + CameraDistance;

	}
}

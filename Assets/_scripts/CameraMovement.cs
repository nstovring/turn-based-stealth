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

	    Vector3 xAxisMovement = transform.right * Input.GetAxis("Horizontal");
        Vector3 zAxisMovement = (Quaternion.AngleAxis(-45, transform.right) * transform.forward) * Input.GetAxis("Vertical");

        CameraDistance += xAxisMovement +zAxisMovement;

        Vector3 translation = Vector3.Lerp(transform.position, PlayerPosition.position + CameraDistance, 0.1f);
        //transform.Translate(translation, Space.World);
	    transform.position = translation;


	}
}

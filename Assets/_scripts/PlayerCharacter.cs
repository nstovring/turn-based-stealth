using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character
{

    public static Vector3 playerVector3;
    public Door TestDoor;
    public Guard TestGuard;
    public Item TestItem;
    // Use this for initialization
    void Start()
    {
        AddActionToQueue(TestDoor.Action());
        AddActionToQueue(TestGuard.GetBlackJacked());
        AddActionToQueue(TestItem.GetStolen());
        AddActionToQueue(TestDoor.Action());
    }
	// Update is called once per frame
	void Update ()
	{
	    updatePosition();
        if (Input.anyKeyDown)
        {
            StartActions();
        }
    }

   

    void updatePosition()
    {
        playerVector3 = transform.position;
    }
}

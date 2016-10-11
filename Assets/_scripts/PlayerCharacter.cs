using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character
{

    public static Vector3 playerVector3;
    public Door TestDoor;
    public Guard TestGuard;
    public Item TestItem;
    public Cell TestGrid;
    // Use this for initialization
    void Start()
    {
        Initialize();
        //GetPathfindingVector3s(TestGrid.transform.position);
        AddActionToQueue(Move(GetCellTransform(TestGrid.transform)));
        AddActionToQueue(Move(GetCellTransform(TestGrid.transform)));
        AddActionToQueue(Move(GetCellTransform(TestGrid.transform)));

        //AddActionToQueue(TestDoor.Action());
        //AddActionToQueue(TestGuard.GetBlackJacked());
        //AddActionToQueue(TestItem.GetStolen());
        //AddActionToQueue(Move(TestGrid.transform));
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

using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character
{
    public Door TestDoor;
    public Guard TestGuard;
    public Item TestItem;
    public Cell TestGrid;
    // Use this for initialization
    void Start()
    {
        AddActionToQueue(TestDoor.Action());
        AddActionToQueue(TestGuard.GetBlackJacked());
        AddActionToQueue(TestItem.GetStolen(transform.position));
        AddActionToQueue(AddMoveActionsToQueue(TestGrid.transform));
    }
    // Update is called once per frame
    void Update ()
	{
        if (Input.anyKeyDown)
        {
            StartActions();
        }

    }
}

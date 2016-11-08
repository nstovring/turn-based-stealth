using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Seeker : Character
{
    private Character parentCharacter;
    public Cell[] path;
	// Use this for initialization
	void Start ()
	{
        Initialize();
	    parentCharacter = transform.parent.GetComponent<Character>();
        actionPoints = parentCharacter.totalActionPoints;
        totalActionPoints = parentCharacter.totalActionPoints;
	}
    public Cell[] GetPathToDestination(Transform endCell)
    {
        List<Cell> tempList = new List<Cell>();
        if (parentCharacter.actionPoints <= 0)
        {
            actionPoints = parentCharacter.totalActionPoints*2;
        }
        else
        {
            actionPoints = parentCharacter.actionPoints* 2;
        }
        if (Vector3.Distance(parentCharacter.transform.position, endCell.position) > 2)
        {
            for (int i = 0; i < actionPoints; i++)
            {
                Cell cell = GetClosestCell(endCell);
                if (cell && !tempList.Contains(cell))
                {
                    tempList.Add(MoveSeeker(cell));
                }
                else
                {
                    break;
                }
            }
        }
        return tempList.ToArray();
    }

    public void SetPathToDestination(Transform endTransform)
    {
        if (!parentCharacter.isMoving)
        {
            path = GetPathToDestination(endTransform);
            foreach (var cell in path)
            {
                cell.SetActiveViewEdge((parentCharacter.viewType+2)%4, true);
            }
        }
    }

    public void ResetPosition()
    {
        actionPoints = parentCharacter.totalActionPoints;
        transform.position = transform.parent.position;
        foreach (var cell in path)
        {
            cell.SetActiveViewEdge((parentCharacter.viewType + 2) % 4, false);
        }
    }

    public Cell MoveSeeker(Cell destination)
    {
        Transform cellTransform = destination.myTransform;
        Vector3 tempDestination = new Vector3(cellTransform.position.x, transform.position.y, cellTransform.position.z);
        Vector3 relativePos = tempDestination - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        
        transform.rotation = rotation;
        
        //tempDestination = new Vector3(destination.position.x, transform.position.y, destination.position.z);

        transform.position = tempDestination;
        currentCell = destination;
        actionPoints--;
        if(currentCell)
        return currentCell;
        return null;
    }
    // Update is called once per frame
    void Update () {
	
	}
}

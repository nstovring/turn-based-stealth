using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Seeker : Character
{
    private Character parentCharacter;
    public Transform[] path;
	// Use this for initialization
	void Start ()
	{
	    parentCharacter = transform.parent.GetComponent<Character>();
        actionPoints = parentCharacter.totalActionPoints;
        GetCurrentCell();
	}
    public Transform[] GetPathToDestination(Transform endCell)
    {
        List<Transform> tempList = new List<Transform>();
        actionPoints = parentCharacter.actionPoints *2;
        if (Vector3.Distance(parentCharacter.transform.position, endCell.position) > 4)
        {
            for (int i = 0; i < actionPoints; i++)
            {
                Transform cell = GetClosestCellTransform(endCell);
                if (cell && !tempList.Contains(cell))
                {
                    tempList.Add(MoveSeeker(cell));
                }
            }
        }
        return tempList.ToArray();
    }

    public void SetPathToDestination(Transform endTransform)
    {
        path = GetPathToDestination(endTransform);
        foreach (var cell in path)
        {
            cell.GetComponent<Renderer>().material.color = Color.cyan;
        }
    }

    public void ResetPosition()
    {
        transform.position = transform.parent.position;
        foreach (var cell in path)
        {
            cell.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public Transform MoveSeeker(Transform destination)
    {
        Vector3 tempDestination = new Vector3(destination.position.x, transform.position.y, destination.position.z);
        Vector3 relativePos = tempDestination - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        
        transform.rotation = rotation;
        
        tempDestination = new Vector3(destination.position.x, transform.position.y, destination.position.z);

        transform.position = tempDestination;
        ChangeCurrentCell(destination);

        return currentCell.transform;
    }
    // Update is called once per frame
    void Update () {
	
	}
}

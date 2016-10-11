using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;


public class Character : MonoBehaviour, IClickable
{
    public int actionPoints;
    public Queue<IEnumerator> actions = new Queue<IEnumerator>();
    public Coroutine myCoroutine;
    public Cell currentCell;
    public NavMeshAgent myAgent;

    private enum movementDirection
    {
        Forward,
        Right,
        Left,
        Backwards
    };

    public float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);
        if (dir > 0.0f)
        {
            return 1.0f;
        }
        if (dir < 0.0f)
        {
            return -1.0f;
        }
        return 0.0f;
    }

    public Vector3 GetDirection(Vector3 desiredDirection)
    {
        float angle = Vector3.Angle(transform.forward, desiredDirection);
        if (angle > 135)
        {
            return transform.forward * -1;
        }
        if (angle < 45)
        {
            return transform.forward;
        }
        if (angle > 45 && angle < 135 && AngleDir(transform.forward, desiredDirection, transform.up) > 0)
        {
            return transform.right;
        }
        if (angle > 45 && angle < 135 && AngleDir(transform.forward, desiredDirection, transform.up) < 1)
        {
            return transform.right * -1;
        }
        Debug.Log("No valid Direction Found!");
        return Vector3.zero;
    }


    public Vector3[] GetPathfindingVector3s(Vector3 targetPosition)
    {
        myAgent.Resume();
        NavMeshPath tempPath = new NavMeshPath();
        myAgent.CalculatePath(targetPosition, tempPath);
        myAgent.path = tempPath;
        myAgent.Stop();
        return tempPath.corners;
    }


    public Transform GetClosestCellTransform(Transform endPosition)
    {
        RaycastHit hit;
        Vector3 cornerDirection = GetPathfindingVector3s(endPosition.position)[1] - transform.position;
        Vector3 tempPosVector3 = transform.position + GetDirection(cornerDirection);
        Debug.DrawRay(tempPosVector3, GetDirection(cornerDirection), Color.red,30f);
        Debug.DrawRay(tempPosVector3, Vector3.down, Color.blue,30f);

        if (Physics.Raycast(tempPosVector3, Vector3.down, out hit))
        {
            return hit.transform;
        }
        return null;
    }


    public void AddActionToQueue(IEnumerator action)
    {
        actions.Enqueue(action);
    }

    public void StartActions()
    {
        myCoroutine = StartCoroutine(ExecuteActions());
    }

    public void CancelActions()
    {
        StopCoroutine(ExecuteActions());
        myCoroutine = null;
    }
              
    IEnumerator ExecuteActions()
    {
        while (true)
        {
            if (actions.Count > 0)
                yield return StartCoroutine(actions.Dequeue());
            else
                yield return null;
        }
    }

    public IEnumerator AddMoveActionsToQueue(Transform finalDestination)
    {
        Vector3 tempfinalDestination = new Vector3(finalDestination.position.x, transform.position.y, finalDestination.position.z);
        while (Vector3.Distance(transform.position, tempfinalDestination) > 0.1f)
        {
            yield return StartCoroutine(Move(GetClosestCellTransform(finalDestination)));
        }
    }

    public IEnumerator Move(Transform destination)
    {
        Vector3 tempDestination = new Vector3(destination.position.x, transform.position.y, destination.position.z);
        while (Vector3.Distance(transform.position, tempDestination) > 0.1f)
        {
            tempDestination = new Vector3(destination.position.x, transform.position.y, destination.position.z);
            transform.position = Vector3.Lerp(transform.position, tempDestination, 0.1f);
            if (Vector3.Distance(transform.position, tempDestination) <= 0.1f)
            {
                transform.position = tempDestination;
                currentCell.isOccupied = false;
                currentCell = destination.GetComponent<Cell>();
                currentCell.isOccupied = true;

            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void LeftClicked()
    {
        throw new NotImplementedException();
    }

    public void RightClicked()
    {
        throw new NotImplementedException();
    }
}

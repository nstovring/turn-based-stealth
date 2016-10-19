using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;


public class Character : MonoBehaviour, IClickable
{
    public int totalActionPoints;
    public int actionPoints;
    public Queue<IEnumerator> actions = new Queue<IEnumerator>();
    public Coroutine myCoroutine;
    public Cell currentCell;
    public NavMeshAgent myAgent;
    public int coneSize = 5;

    public enum orientation
    {
        Forward,
        Right,
        Left,
        Backwards
    };

    public orientation myOrientation;

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
            myOrientation = orientation.Backwards;
            return transform.forward * -1;
        }
        if (angle < 45)
        {
            myOrientation = orientation.Forward;
            return transform.forward;
        }
        if (angle > 45 && angle < 135 && AngleDir(transform.forward, desiredDirection, transform.up) > 0)
        {
            myOrientation = orientation.Right;
            return transform.right;
        }
        if (angle > 45 && angle < 135 && AngleDir(transform.forward, desiredDirection, transform.up) < 1)
        {
            myOrientation = orientation.Left;
            return transform.right * -1;
        }
        Debug.Log("No valid Direction Found!");
        return Vector3.zero;
    }

    public void GetCurrentCell()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 1, transform.up * -1, out hit, LayerMask.NameToLayer("Ground")))
        {
            MonoBehaviour monohit = hit.transform.GetComponent<MonoBehaviour>();
            var cell = monohit as Cell;
            if (cell != null)
            {
                currentCell = cell;
            }
        }
    }


    public Vector3[] GetPathfindingVector3s(Vector3 targetPosition)
    {
        myAgent.enabled = true;
        myAgent.Resume();
        NavMeshPath tempPath = new NavMeshPath();
        myAgent.CalculatePath(targetPosition, tempPath);
        myAgent.path = tempPath;
        myAgent.Stop();
        myAgent.enabled = false;
        return tempPath.corners;
    }


    public Transform GetClosestCellTransform(Transform endPosition)
    {
        RaycastHit hit;
        Vector3 cornerDirection = GetPathfindingVector3s(endPosition.position)[1] - transform.position;
        Vector3 tempPosVector3 = transform.position + GetDirection(cornerDirection)*currentCell.transform.localScale.z;
        Debug.DrawRay(tempPosVector3, GetDirection(cornerDirection), Color.red,30f);
        Debug.DrawRay(tempPosVector3, Vector3.down, Color.blue,30f);

        if (Physics.Raycast(tempPosVector3, Vector3.down, out hit,LayerMask.NameToLayer("Ground")))
        {
            Debug.Log("Got transform");
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
            {
                yield return StartCoroutine(actions.Dequeue());
            }
            else
            {
                Debug.Log("All Actions Complete");
                CancelActions();
                break;
                //yield return null;
            }
        }
    }

    public virtual IEnumerator QueuedMove(Transform finalDestination)
    {
        Vector3 tempfinalDestination = new Vector3(finalDestination.position.x, transform.position.y, finalDestination.position.z);
        while (Vector3.Distance(transform.position, tempfinalDestination) > 0.1f && ActionPointsLeft())
        {
                yield return StartCoroutine(Move(GetClosestCellTransform(finalDestination)));
        }
    }

    public virtual IEnumerator Move(Transform destination)
    {
        Vector3 tempDestination = new Vector3(destination.position.x, transform.position.y, destination.position.z);
        Vector3 relativePos = tempDestination - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        while (Vector3.Angle(transform.forward, tempDestination - transform.position) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.2f);
            yield return new WaitForEndOfFrame();
        }

        //transform.rotation = rotation;
        while (Vector3.Distance(transform.position, tempDestination) > 0.1f)
        {
            tempDestination = new Vector3(destination.position.x, transform.position.y, destination.position.z);
            transform.position = Vector3.Lerp(transform.position, tempDestination, 0.2f);
            yield return new WaitForEndOfFrame();
        }
        transform.position = tempDestination;
        ChangeCurrentCell(destination);
        yield return new WaitForEndOfFrame();
    }
    public virtual void ChangeCurrentCell(Transform destination)
    {
        currentCell.isOccupied = false;
        currentCell.occupier = null;
        currentCell = destination.GetComponent<Cell>();
        currentCell.occupier = transform;
        currentCell.isOccupied = true;
        Debug.Log("reached destination");
        actionPoints--;
    }

  
    public void LeftClicked()
    {
        throw new NotImplementedException();
    }

    public void RightClicked()
    {
        throw new NotImplementedException();
    }
    public void newActions()
    {
        //actions = new Queue<IEnumerator>();
        actionPoints = totalActionPoints;
    }
    public bool ActionPointsLeft() {
        if (actionPoints == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Debug = UnityEngine.Debug;


public class Character : MonoBehaviour, IClickable
{
    public Seeker mySeeker;
    public GameObject virtCharObjPrefab;
    public int totalActionPoints;
    public int actionPoints;
    public Queue<IEnumerator> actions = new Queue<IEnumerator>();
    public Coroutine myCoroutine;
    public Cell currentCell;
    public NavMeshAgent myAgent;
    public int coneSize = 5;
    public List<Cell> visionCone;

    [HideInInspector]
    public Transform myTransform;
    [Range(0,1)]
    public int viewType;


    public bool isMoving;
    public bool myTurn;
    public LayerMask solidLayer;


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

    public Vector3 GetClampedDirection(Vector3 desiredDirection)
    {
        float angle = Vector3.Angle(myTransform.forward, desiredDirection);
        if (angle > 135)
        {
            myOrientation = orientation.Backwards;
            return myTransform.forward * -1;
        }
        if (angle < 45)
        {
            myOrientation = orientation.Forward;
            return myTransform.forward;
        }
        if (angle > 45 && angle < 135 && AngleDir(myTransform.forward, desiredDirection, myTransform.up) > 0)
        {
            myOrientation = orientation.Right;
            return myTransform.right;
        }
        if (angle > 45 && angle < 135 && AngleDir(myTransform.forward, desiredDirection, myTransform.up) < 1)
        {
            myOrientation = orientation.Left;
            return myTransform.right * -1;
        }
        Debug.Log("No valid Direction Found!");
        return Vector3.zero;
    }

    public virtual void Initialize()
    {
        myTransform = transform;
        currentCell = CellHelper.GetCurrentCell(myTransform);
    }

    public Vector3[] GetPathfindingVector3Array(Vector3 targetPosition)
    {
        myAgent.enabled = true;
        myAgent.Resume();
        NavMeshPath tempPath = new NavMeshPath();
        myAgent.CalculatePath(targetPosition, tempPath);
        myAgent.path = tempPath;
        Vector3[] tempArray = new Vector3[tempPath.corners.Length];
        for (int i = 0; i < tempArray.Length; i++)
        {
            tempArray[i] = tempPath.corners[i];
        }
        myAgent.Stop();
        myAgent.enabled = false;
        
        return tempArray;
    }


    public Cell GetClosestCell(Transform endPosition)
    {
        //RaycastHit hit;
        Vector3[] cornerArray = GetPathfindingVector3Array(endPosition.position);
        if (cornerArray.Length < 2)
            return null;
        Vector3 cornerDirection = cornerArray[1] - myTransform.position;
        Vector3 clampedDirection = GetClampedDirection(cornerDirection);
        //Vector3 tempPosVector3 = myTransform.position + clampedDirection * currentCell.myTransform.localScale.z;
        //Debug.DrawRay(tempPosVector3, clampedDirection, Color.red,1f);
        //Debug.DrawRay(tempPosVector3, Vector3.down, Color.blue,1f);

        return CellHelper.GetCellFromDirection(myTransform.position,clampedDirection, 1, solidLayer);
        //return CellHelper.GetCellAtVector(tempPosVector3);
    }


    public void AddActionToQueue(IEnumerator action)
    {
        actions.Enqueue(action);
    }

    public void StartActions()
    {
        myCoroutine = StartCoroutine(ExecuteActions());
    }

    public virtual void CancelActions()
    {
        StopCoroutine(ExecuteActions());
        myCoroutine = null;
    }

    public bool allCurrentActionsComplete;
    public virtual IEnumerator ExecuteActions()
    {
        while (true)
        {
            if (actions.Count > 0)
            {
                visualizeViewRange(false);
                isMoving = true;
                yield return StartCoroutine(actions.Dequeue());
            }
            else
            {
                visualizeViewRange(true);
                isMoving = false;
                //Debug.Log("All Actions Complete");
                CancelActions();
                break;
            }
        }
    }

    public bool destinationReached;
    public virtual IEnumerator QueuedMove(Transform finalDestination)
    {
        visualizeViewRange(false);
        Vector3 tempfinalDestination = new Vector3(finalDestination.position.x, myTransform.position.y, finalDestination.position.z);
        while (Vector3.Distance(myTransform.position, tempfinalDestination) > 0.1f && ActionPointsLeft())
        {
            Cell closestCell = GetClosestCell(finalDestination);
            if (closestCell)
            {
                yield return StartCoroutine(Move(closestCell.myTransform));
            }
            else
            {
                Debug.Log("Stop movement");
                actionPoints = 0;
                CancelActions();
            }
        }
        destinationReached = currentCell.myTransform == finalDestination;
        visualizeViewRange(true);
    }

    public virtual IEnumerator Move(Transform destination)
    {


        Vector3 tempDestination = new Vector3(destination.position.x, myTransform.position.y, destination.position.z);
        Vector3 relativePos = tempDestination - myTransform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        while (Vector3.Angle(myTransform.forward, tempDestination - myTransform.position) > 0.1f)
        {
            myTransform.rotation = Quaternion.Lerp(myTransform.rotation, rotation, 0.2f);
            yield return new WaitForEndOfFrame();
        }

        while (Vector3.Distance(myTransform.position, tempDestination) > 0.1f)
        {
            tempDestination = new Vector3(destination.position.x, myTransform.position.y, destination.position.z);
            myTransform.position = Vector3.MoveTowards(myTransform.position, tempDestination, 0.2f);
            yield return new WaitForEndOfFrame();
        }

        myTransform.position = tempDestination;
        ChangeCurrentCell(destination);
        yield return new WaitForEndOfFrame();

    }
    public virtual void ChangeCurrentCell(Transform destination)
    {
        currentCell.isOccupied = false;
        currentCell.occupier = null;
        currentCell = destination.GetComponent<Cell>();
        currentCell.occupier = this;
        currentCell.isOccupied = true;
        actionPoints--;
    }


    public virtual Cell[] GetVisionConeTransforms(int _coneSize)
    {
        List<Cell> tempViewConeList = new List<Cell>();

        for (int i = 0; i < _coneSize; i++)
        {
            Cell temp = CellHelper.GetCellFromDirection(currentCell.myTransform.position, myTransform.forward, i, solidLayer);
            if (temp != null)
                tempViewConeList.Add(temp);

        }
        int coneWidth = 1;
        int tempListLength = tempViewConeList.Count;
        for (int index = 0; index < tempListLength; index++)
        {
            var cell = tempViewConeList[index];
            
            for (int i = 0; i < coneWidth + 1; i++)
            {
                Cell tempLeft = CellHelper.GetCellFromDirection(cell.myTransform.position, myTransform.right * -1, i,solidLayer);
                Cell tempRight = CellHelper.GetCellFromDirection(cell.myTransform.position, myTransform.right, i, solidLayer);
                if (tempRight)
                    tempViewConeList.Add(tempRight);
                if (tempLeft)
                    tempViewConeList.Add(tempLeft);
            }
            coneWidth++;
        }
        return tempViewConeList.ToArray();
    }

 
    public virtual void visualizeViewRange(bool isWithinView)
    {
        visionCone= GetVisionConeTransforms(coneSize).ToList();
        foreach (var visionConeCell in visionCone)
        {
            visionConeCell.SetActiveViewEdge(viewType, isWithinView);
        }
    }

    public virtual void LeftClicked()
    {
        throw new NotImplementedException();
    }

    public virtual void RightClicked()
    {
        throw new NotImplementedException();
    }
    public virtual void newActions()
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

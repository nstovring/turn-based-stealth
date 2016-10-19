using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Guard : Character
{
    private Animator myAnimator;

    public enum GuardState {Conscious, Unconscious, Stunned, Alert};
    public GuardState MyGuardState = GuardState.Conscious;
    struct Inventory
    {
        int coin;
        bool key;
    }

    public Transform[] patrolTransforms;
    public Transform[] visionCone;
    private Transform currentTarget;
	// Use this for initialization
	IEnumerator Start ()
	{
        GameManager.Instance.AddGuardCharacters(this);
        GetCurrentCell();
        newActions();
        myAnimator = GetComponentInChildren<Animator>();
        myAnimator.SetBool("Conscious", true);
        AddActionToQueue(IterateThroughPatrolRoutes());
	    yield return new WaitForSeconds(0.1f);
        StartActions();
    }

    public int patrolint = 0;
    IEnumerator IterateThroughPatrolRoutes()
    {
        currentTarget = patrolTransforms[patrolint % patrolTransforms.Length];

        while (Vector3.Distance(transform.position, currentTarget.position) > 0.1)
        {
            currentTarget = patrolTransforms[patrolint % patrolTransforms.Length];
            if (Vector3.Distance(transform.position, currentTarget.position) < 1)
            {
                patrolint++;
                visualizeViewRange(Color.red);
            }
            yield return StartCoroutine(QueuedMove(currentTarget));
        }
    }

    public Transform[] GetVisionConeTransforms(int _coneSize)
    {
        List<Transform> tempViewConeList = new List<Transform>();

        for (int i = 0; i < _coneSize; i++)
        {
            Transform temp = GetCellFromDirection(currentCell.transform.position, transform.forward, i);
            if(temp != null)
            tempViewConeList.Add(temp);

        }
        int coneWidth = 1;
        int tempListLength = tempViewConeList.Count;
        for (int index = 0; index < tempListLength; index++)
        {
            var cell = tempViewConeList[index];
            //if (coneWidth > 0)
            //{
                for (int i = 0; i < coneWidth + 1; i++)
                {
                    Transform tempLeft = GetCellFromDirection(cell.transform.position, transform.right*-1, i);
                    Transform tempRight = GetCellFromDirection(cell.transform.position, transform.right, i);
                    if(tempRight)
                        tempViewConeList.Add(tempRight);
                    if (tempLeft)
                        tempViewConeList.Add(tempLeft);
                }
            //}
            coneWidth++;
        }
        return tempViewConeList.ToArray();
    }

    Transform GetCellFromDirection(Vector3 startPosition, Vector3 direction, int distance)
    {
        RaycastHit hit;
        if (!Physics.Raycast(startPosition + transform.up * 1, direction,  distance * 2, LayerMask.GetMask("ViewObstacle")))
        {
            if (Physics.Raycast(startPosition + transform.up * 2 + direction * 2 * distance, transform.up * -1, out hit, LayerMask.GetMask("Ground")))
            {
                MonoBehaviour monohit = hit.transform.GetComponent<MonoBehaviour>();
                var cell = monohit as Cell;
                if (cell != null)
                {
                    return hit.transform;
                }
            }
        }
        return null;
    }

    public void visualizeViewRange(Color color)
    {
        foreach (var visionConeTransform in GetVisionConeTransforms(coneSize))
        {
            visionConeTransform.GetComponent<Renderer>().material.color = color;
        }
    }

    // Update is called once per frame
    void Update () {
        if (!ActionPointsLeft())
        {
           // GameManager.Instance.givePlayerActions();
        }
    }


    void ChangeState(GuardState newState)
    {
        MyGuardState = newState;
    }

    public virtual IEnumerator GetBlackJacked()
    {
        if (GameManager.Instance.PlayerCharacters[0].ActionPointsLeft())
        {
            ChangeState(GuardState.Unconscious);
            myAnimator.SetBool("Conscious", false);
            yield return new WaitForSeconds(2f);
            Debug.Log("Guard Unconscious");
            GameManager.Instance.PlayerCharacters[0].actionPoints--;
        }

    }

    IEnumerator GetPickPocketed()
    {
        return null;
    }
}

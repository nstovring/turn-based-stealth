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
                visualizeViewRange();
                patrolint++;
            }
            yield return StartCoroutine(QueuedMove(currentTarget));
        }

      
    }

    public Transform[] getVisionConeTransforms(int _coneSize)
    {
        List<Transform> tempViewConeList = new List<Transform>();
        RaycastHit hit;
        Vector3 searchDirection = transform.forward;
        int sideDirection = 0;

        for (int i = 0; i < _coneSize; i++)
        {
            if (Physics.Raycast(currentCell.transform.position + transform.up * 2, searchDirection, LayerMask.GetMask("ViewObstacle")))
            {
                Debug.Log("Wall not hit");
                if (Physics.Raycast(currentCell.transform.position + transform.up * 2 + searchDirection * 2 * i, transform.up * -1, out hit, LayerMask.GetMask("Ground")))
                {
                    tempViewConeList.Add(hit.transform);
                }
            }
        }
        return tempViewConeList.ToArray();
    }

    public void visualizeViewRange()
    {
        foreach (var visionConeTransform in getVisionConeTransforms(coneSize))
        {
            visionConeTransform.GetComponent<Renderer>().material.color = Color.red;
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

    public IEnumerator GetBlackJacked()
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

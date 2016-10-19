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
            }
            yield return StartCoroutine(QueuedMove(currentTarget));
        }
    }

   

    // Update is called once per frame
    void Update ()
    {
        CheckForPlayerInView();
    }

    public override void visualizeViewRange(Color color)
    {
        Debug.Log("Getting Visual Range");
        CheckForPlayerInView();
        base.visualizeViewRange(color);
    }

    void ChangeState(GuardState newState)
    {
        MyGuardState = newState;
    }

    public void CheckForPlayerInView()
    {
        Debug.Log("Checking if player is wthin visual range");
        foreach (var cell in visionCone)
        {
            Cell tempCell = cell.GetComponent<Cell>();
            if (tempCell.occupier is PlayerCharacter)
            {
                GameManager.Instance.GameOver();
                //GameOVER MaN!
            }
        }
    }

    public virtual IEnumerator GetBlackJacked()
    {
        if (GameManager.Instance.PlayerCharacters[0].ActionPointsLeft())
        {
            ChangeState(GuardState.Unconscious);
            myAnimator.SetBool("Conscious", false);
            CancelActions();
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

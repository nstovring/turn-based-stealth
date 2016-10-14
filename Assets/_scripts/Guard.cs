using System;
using UnityEngine;
using System.Collections;

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

    private void GetCurrentCell()
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

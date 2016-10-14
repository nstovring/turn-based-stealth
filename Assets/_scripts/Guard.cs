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
	void Start ()
	{
        GetCurrentCell();
        myAnimator = GetComponentInChildren<Animator>();
        myAnimator.SetBool("Conscious", true);
        //AddActionToQueue(IterateThroughPatrolRoutes());
        AddActionToQueue(IterateThroughPatrolRoutes());
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

    private int i = 0;
    IEnumerator IterateThroughPatrolRoutes()
    {
        currentTarget = patrolTransforms[i % patrolTransforms.Length];

        while (Vector3.Distance(transform.position, currentTarget.position) > 0.1)
        {
            currentTarget = patrolTransforms[i % patrolTransforms.Length];
            yield return StartCoroutine(QueuedMove(currentTarget));
            i++;
        }
    }

    // Update is called once per frame
    void Update () {
      
    }


    void ChangeState(GuardState newState)
    {
        MyGuardState = newState;
    }

    public IEnumerator GetBlackJacked()
    {
        ChangeState(GuardState.Unconscious);
        myAnimator.SetBool("Conscious", false);
        yield return new WaitForSeconds(2f);
        Debug.Log("Guard Unconscious");
    }

    IEnumerator GetPickPocketed()
    {
        return null;
    }
}

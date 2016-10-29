using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Guard : Character
{
    public VirtualGuardCharacter virtualCharacter;
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
        //if (virtCharObjPrefab != null)
        //{
        //    GameObject tempVirtChar = Instantiate(virtCharObjPrefab, transform.position, Quaternion.identity) as GameObject;
        //    virtualCharacter = tempVirtChar.GetComponent<VirtualGuardCharacter>();
        //    virtualCharacter.super = this;
        //}
        GameManager.Instance.AddGuardCharacters(this);
	    currentCell = CellHelper.GetCurrentCell(transform);

	    for (int index = 0; index < patrolTransforms.Length; index++)
	    {
	        var patrolTransform = patrolTransforms[index];
            patrolTransforms[index] = CellHelper.GetCurrentCell(patrolTransform).transform;
	    }

        myAnimator = GetComponentInChildren<Animator>();
        myAnimator.SetBool("Conscious", true);
        visualizeViewRange(false);

        currentTarget = patrolTransforms[patrolint % patrolTransforms.Length];

        AddActionToQueue(QueuedMove(currentTarget));

        yield return new WaitForSeconds(0.1f);

        StartActions();
    }

    public int patrolint = 0;


    public void EvaluateNextGoal()
    {
        if (destinationReached)
        {
            patrolint++;
            currentTarget = patrolTransforms[patrolint%patrolTransforms.Length];
            destinationReached = false;
            AddActionToQueue(QueuedMove(currentTarget));
        }
        else
        {
            AddActionToQueue(QueuedMove(currentTarget));
        }
    }

    public override IEnumerator ExecuteActions()
    {
        while (true)
        {
            if (actions.Count > 0)
            {
                mySeeker.ResetPosition();
                isMoving = true;
                visualizeViewRange(false);
                yield return StartCoroutine(actions.Dequeue());
            }
            else
            {
                visualizeViewRange(true);
                isMoving = false;
                if (destinationReached)
                {
                    EvaluateNextGoal();
                    StartActions();
                }
                else
                {
                    mySeeker.SetPathToDestination(currentTarget);
                    CancelActions();
                }
                break;
            }
        }
    }

    public override void newActions()
    {
        base.newActions();
        EvaluateNextGoal();
        StartActions();
    }


    // Update is called once per frame
    void Update ()
    {
        CheckForPlayerInView();
        mySeeker.transform.position += new Vector3(0, 0, Input.GetAxis("Horizontal"));
    }

    public override void visualizeViewRange(bool isWithinView)
    {
        base.visualizeViewRange(isWithinView);
        CheckForPlayerInView();
    }

    void ChangeState(GuardState newState)
    {
        MyGuardState = newState;
    }

    public void CheckForPlayerInView()
    {
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

    public override void LeftClicked()
    {
        PlayerCharacter player = GameManager.Instance.PlayerCharacters[GameManager.Instance.currentPlayer];
        if (player.ActionPointsLeft() && Vector3.Distance(player.transform.position, transform.position) < 4)
        {
            player.AddActionToQueue(GetBlackJacked());
        }
        else
        {
            player.AddActionToQueue(player.QueuedMove(transform));
            player.AddActionToQueue(GetBlackJacked());
        }
        player.StartActions();
    }

    public override void CancelActions()
    {
        StopAllCoroutines();
        base.CancelActions();
    }

    public virtual IEnumerator GetBlackJacked()
    {
        if (GameManager.Instance.PlayerCharacters[GameManager.Instance.currentPlayer].ActionPointsLeft())
        {
            ChangeState(GuardState.Unconscious);
            myAnimator.SetBool("Conscious", false);
            CancelActions();
            yield return new WaitForSeconds(2f);
            Debug.Log("Guard Unconscious");
            GameManager.Instance.PlayerCharacters[GameManager.Instance.currentPlayer].actionPoints--;
        }

    }

    IEnumerator GetPickPocketed()
    {
        return null;
    }
}

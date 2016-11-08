using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Guard : Character
{
    public VirtualGuardCharacter virtualCharacter;
    private Animator myAnimator;
    public enum GuardState {Conscious, Unconscious, Stunned, Alert};
    public GuardState MyGuardState = GuardState.Conscious;
    public int maxUnconsciousTime;
    int unconsciousTime;
    public bool turnDone;
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
        Initialize();
        if(maxUnconsciousTime == 0)
        {
            maxUnconsciousTime = 3;
        }
        //if (virtCharObjPrefab != null)
        //{
        //    GameObject tempVirtChar = Instantiate(virtCharObjPrefab, transform.position, Quaternion.identity) as GameObject;
        //    virtualCharacter = tempVirtChar.GetComponent<VirtualGuardCharacter>();
        //    virtualCharacter.super = this;
        //}
        GameManager.Instance.AddGuardCharacters(this);
        yield return new WaitForSeconds(0.1f);
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

        StartActions();
    }

    public int patrolint = 0;

    bool doorInPath = false;

  
    public override IEnumerator ExecuteActions()
    {
        turnDone = false;
        if (MyGuardState == GuardState.Conscious || MyGuardState == GuardState.Alert || unconsciousTime == maxUnconsciousTime)
        {
            unconsciousTime = 0;
            while (true)
            {
                if (actions.Count > 0)
                {
                    mySeeker.ResetPosition();
                    isMoving = true;
                    visualizeViewRange(false);
                    //Debug.Log("stuff");
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
                        //Debug.Log("helloo2");
                        turnDone = true;
                        CancelActions();
                    }
                    
                    break;
                }
            }
            
        }
        else
        {
            EndTurn();
            turnDone = true;
            unconsciousTime++;
        }
    }
    public void EvaluateNextGoal()
    {
        //mySeeker.SetPathToDestination(currentTarget);
        int doorInt = 0;
        Cell doorCell = null;
        foreach (var cell in mySeeker.path)
        {
            //We need to change this if the first cell in the path has a door
            if (cell.door)
            {
                doorCell = cell;
                break;
            }
            doorInt++;
        }
        if (doorCell && !doorCell.door.opened)
        {
            AddActionToQueue(QueuedMove(doorCell.myTransform));
            AddActionToQueue(mySeeker.path[doorInt].door.Action());
            AddActionToQueue(QueuedMove(currentTarget));
        }
        else if (destinationReached)
        {
            patrolint++;
            currentTarget = patrolTransforms[patrolint % patrolTransforms.Length];
            destinationReached = false;
            AddActionToQueue(QueuedMove(currentTarget));
        }
        else
        {
            AddActionToQueue(QueuedMove(currentTarget));
        }
    }

    IEnumerator delayActions(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
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
            //Cell tempCell = cell.GetComponent<Cell>();
            if (cell.occupier is PlayerCharacter)
            {
                GameManager.Instance.GameOver();
                //GameOVER MaN!
            }
        }
    }

    public override void LeftClicked()
    {
        PlayerCharacter player = GameManager.Instance.PlayerCharacters[GameManager.Instance.currentPlayer];
        if (player.ActionPointsLeft() && Vector3.Distance(player.transform.position, myTransform.position) < 4)
        {
            player.AddActionToQueue(GetBlackJacked());
        }
        else
        {
            player.AddActionToQueue(player.QueuedMove(myTransform));
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
    public bool GetTurnDone()
    {
        return turnDone;
    }
}

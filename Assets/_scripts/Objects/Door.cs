using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour, IClickable
{
    public Lock Lock;
    [SerializeField]
    public bool opened = false;
    private Animator myAnimator;
    private Queue<IEnumerator> queue = new Queue<IEnumerator>();
    public Cell[] myCells;
    private NavMeshObstacle myObstacle;

    public AudioClip[] doorSounds;
    private AudioSource audioSource;
    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        myObstacle = GetComponent<NavMeshObstacle>();
        myAnimator = transform.parent.GetComponent<Animator>();
        opened = false;
        myAnimator.SetBool("opened", opened);
        myCells = CellHelper.GetFrontBackCells(transform);
        foreach (var myCell in myCells)
        {
            myCell.door = this;
        }
        if (Lock.locked)
        {
            queue.Enqueue(Lock.PickLock());
            queue.Enqueue(Open());
        }
    }

    IEnumerator Open()
    {
        myAnimator.SetBool("opened", true);
        audioSource.PlayOneShot(doorSounds[0]);
        opened = true;
       
        yield return new WaitForSeconds(1f);
        myObstacle.carving = true;
        //Debug.Log("Door Opened");
        opened = true;
    }

    IEnumerator Close()
    {
        audioSource.PlayOneShot(doorSounds[1]);
        yield return new WaitForSeconds(0.2f);
        myAnimator.SetBool("opened", false);
        //yield return new WaitForSeconds(0.1f);
        opened = false;
        //yield return new WaitForSeconds(0.2f);
        yield return new WaitForSeconds(1f);
        myObstacle.carving = false;
        //Debug.Log("Door Closed");
        opened = false;
    }


    public IEnumerator Action()
    {
        if (Lock.locked)
        {
            //Debug.Log("Door is locked proceed unlocking");
            yield return StartCoroutine(queue.Dequeue());
        }
        if (opened)
        {
            yield return StartCoroutine(Close());
        }
        else
        {
            yield return StartCoroutine(Open());
        }
    }

    public void LeftClicked()
    {
        Character character = GameManager.Instance.PlayerCharacters[GameManager.Instance.currentPlayer];
        //If player next to door open/picklock door
        if (character.currentCell == myCells[0] || character.currentCell == myCells[1])
        {
            character.AddActionToQueue(Action());
            //return;
        }
        else
        {
            //else queue movement to nearest grid
            character.AddActionToQueue(character.QueuedMove(closestCellBetweenMeAndChar(character).transform));
            character.AddActionToQueue(Action());
        }
        character.StartActions();
    }

    Cell closestCellBetweenMeAndChar(Character character)
    {
        if (Vector3.Distance(myCells[0].transform.position, character.transform.position) >
            Vector3.Distance(myCells[1].transform.position, character.transform.position))
        {
            return myCells[1];
        }
        return myCells[0];
    }

    public void RightClicked()
    {
        throw new System.NotImplementedException();
    }
}

[System.Serializable]
public class Lock
{
    public bool locked;
    public int difficulty;
    public float unlockTime;

    public IEnumerator PickLock()
    {
        yield return new WaitForSeconds(unlockTime);
        //Debug.Log("Door Unlocked");
        locked = false;
    }
}

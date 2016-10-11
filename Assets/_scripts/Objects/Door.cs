using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour, IClickable
{
    public Lock Lock;
    [SerializeField]
    private bool opened = false;
    private Animator myAnimator;
    private Queue<IEnumerator> queue = new Queue<IEnumerator>();

    // Use this for initialization
    void Start()
    {
        myAnimator = transform.parent.GetComponent<Animator>();
        opened = false;
        myAnimator.SetBool("opened", opened);
        if (Lock.locked)
        {
            queue.Enqueue(Lock.PickLock());
            queue.Enqueue(Open());
        }
    }

    IEnumerator Open()
    {
        myAnimator.SetBool("opened", true);
        opened = false;
       
        yield return new WaitForSeconds(1f);
        //Debug.Log("Door Opened");
        opened = true;
    }

    IEnumerator Close()
    {
        myAnimator.SetBool("opened", false);
        opened = true;

        yield return new WaitForSeconds(1f);
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
        throw new System.NotImplementedException();
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

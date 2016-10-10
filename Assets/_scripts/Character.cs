using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Character : MonoBehaviour
{
    public int actionPoints;

    public Queue<IEnumerator> actions = new Queue<IEnumerator>();
    public Coroutine myCoroutine;
    public Cell currentCell;

    // Update is called once per frame
    void Update () {
	  
	}

    public void AddActionToQueue(IEnumerator action)
    {
        actions.Enqueue(action);
    }

    public void StartActions()
    {
        myCoroutine = StartCoroutine(ExecuteActions());
    }

    public void CancelActions()
    {
        StopCoroutine(ExecuteActions());
        myCoroutine = null;
    }
    IEnumerator ExecuteActions()
    {
        while (true)
        {
            if (actions.Count > 0)
                yield return StartCoroutine(actions.Dequeue());
            else
                yield return null;
        }
    }
    public IEnumerator Move(Transform destination)
    {
        Vector3 tempDestination = new Vector3(destination.position.x, transform.position.y, destination.position.z);
        while (Vector3.Distance(transform.position, tempDestination) > 0.1f)
        {
            tempDestination = new Vector3(destination.position.x, transform.position.y, destination.position.z);
            transform.position = Vector3.Lerp(transform.position, tempDestination, 0.1f);
            if (Vector3.Distance(transform.position, tempDestination) <= 0.1f)
            {
                transform.position = tempDestination;
                currentCell.isOccupied = false;
                currentCell = destination.GetComponent<Cell>();
                currentCell.isOccupied = true;

            }
            yield return new WaitForEndOfFrame();
        }
    }
}

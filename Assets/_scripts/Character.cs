using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Character : MonoBehaviour
{
    public int actionPoints;

    public Queue<IEnumerator> actions = new Queue<IEnumerator>();
    public Coroutine myCoroutine;
   

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
}

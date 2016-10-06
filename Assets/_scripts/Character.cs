using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Character : MonoBehaviour
{
    public int actionPoints;

    public Queue<IEnumerator> actions = new Queue<IEnumerator>();
    public Coroutine myCoroutine;
    public Door TestDoor;
    public Guard TestGuard;
	// Use this for initialization
	void Start ()
	{
        actions.Enqueue(TestDoor.Action());
        actions.Enqueue(TestGuard.GetBlackJacked());
        actions.Enqueue(TestDoor.Action());
        actions.Enqueue(TestDoor.Action());
    }

    // Update is called once per frame
    void Update () {
	    if (Input.anyKeyDown)
	    {
            StartActions();
	    }
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

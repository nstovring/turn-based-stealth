using UnityEngine;
using System.Collections;

public abstract class Action
{
    private float duration;
    private int requiredActionPoints;
    public bool complete;
    public virtual IEnumerator Execute()
    {
        yield return new WaitForEndOfFrame();
    }

    public void Cancel()
    {

    }
}

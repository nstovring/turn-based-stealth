using UnityEngine;
using System.Collections;

public abstract class Objective : MonoBehaviour
{
    bool completed = false;
    public IWinable[] objectiveChain;

    void ObjectiveIsSpawned()
    {
        //GameManager.Instance.objectives.Add(this);
    }

    void ObjectiveIsCompleted()
    {
        GameManager.Instance.AllObjectivesComplete();
    }
    public bool IsObjectiveComplete()
    {
        return completed;
    }
    public bool HasObjectiveChain()
    {
        if (objectiveChain.Length > 0) return true;
        return false;
    }

}


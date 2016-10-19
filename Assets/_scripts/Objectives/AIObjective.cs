using UnityEngine;
using System.Collections;

public class AIObjective : Guard, IWinable {

    bool completed = false;
    public IWinable[] objectiveChain;
    public bool active;
    void Start()
    {
        ObjectiveIsSpawned();
    }

    public void ObjectiveIsSpawned()
    {
        GameManager.Instance.objectives.Add(this);
    }

    public void ObjectiveIsCompleted()
    {
        completed = true;
        if (HasObjectiveChain())
        {
            AddObjectiveChain();
        }
        GameManager.Instance.AllObjectivesComplete();

    }
    public bool IsObjectiveComplete()
    {
        return completed;
    }
    public bool HasObjectiveChain()
    {
        //if (objectiveChain.Length > 0) return true;
        return false;
    }
    public override IEnumerator GetBlackJacked()
    {
        if (GameManager.Instance.PlayerCharacters[0].ActionPointsLeft())
        {
            ObjectiveIsCompleted();
        }
        yield return base.GetBlackJacked();
    }
    public void AddObjectiveChain()
    {
        foreach(IWinable objective in objectiveChain)
        {
            GameManager.Instance.objectives.Add(objective);
        }
    }
}

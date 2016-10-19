using UnityEngine;
using System.Collections;

public class EscapeObjective : MonoBehaviour, IWinable {


    bool completed = false;
    public IWinable[] objectiveChain;
    public Cell[] ObjectivePlace;
    public bool active;

    public void ObjectiveIsSpawned()
    {
        GameManager.Instance.objectives.Add(this);
    }

    public void ObjectiveIsCompleted()
    {
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
        if (objectiveChain.Length > 0) return true;
        return false;
    }

    public void IsPlayerInCells()
    {
            foreach (Cell cell in ObjectivePlace)
            {
                if (cell.isOccupied) {
                    if (cell.occupier == GameManager.Instance.PlayerCharacters[0]) {
                    ObjectiveIsCompleted();
                        if (!GameManager.Instance.AllObjectivesComplete()) completed = false;
                    }
                }
            }

        
    }
    public void AddObjectiveChain()
    {
        foreach (IWinable objective in objectiveChain)
        {
            GameManager.Instance.objectives.Add(objective);
        }
    }
}

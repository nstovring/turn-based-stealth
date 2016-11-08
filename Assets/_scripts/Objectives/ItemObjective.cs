using UnityEngine;
using System.Collections;

public class ItemObjective : Item, IWinable {

    bool completed = false;
    public IWinable[] objectiveChain;

    void Start()
    {
        //objectiveChain = new IWinable[0];
        //currentCell = CellHelper.GetCurrentCell(transform);
        currentCells = CellHelper.GetCellsAround(transform);
        GetCurrentCell();
        
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
    public override IEnumerator GetStolen(Transform playerTransform) {
        if (GameManager.Instance.PlayerCharacters[0].ActionPointsLeft())
        {
            ObjectiveIsCompleted();
        }
        yield return base.GetStolen(playerTransform);
    }
    public void AddObjectiveChain()
    {
        foreach (IWinable objective in objectiveChain)
        {
            GameManager.Instance.objectives.Add(objective);
        }
    }

}

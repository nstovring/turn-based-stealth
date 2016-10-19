using UnityEngine;
using System.Collections;

public interface IWinable
{
    void ObjectiveIsSpawned();
    void ObjectiveIsCompleted();
    bool IsObjectiveComplete();
    bool HasObjectiveChain();
    void AddObjectiveChain();
}

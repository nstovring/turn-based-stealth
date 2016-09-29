using UnityEngine;
using System.Collections;

public interface IActionable
{
    IEnumerator Action();
    void Execute(int actionPoints, float duration);
    void Cancel();
}

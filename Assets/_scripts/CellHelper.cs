using UnityEngine;
using System.Collections;

public static class CellHelper {

    public static Cell GetCurrentCell(Transform transform)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 1, transform.up * -1, out hit, LayerMask.NameToLayer("Ground")))
        {
            MonoBehaviour monohit = hit.transform.GetComponent<MonoBehaviour>();
            var cell = monohit as Cell;
            if (cell != null)
            {
                return cell;
            }
        }
        return null;
    }

    public static Cell[] GetFrontBackCells(Transform transform)
    {
        RaycastHit hit;
        Cell[] tempCells = new Cell[2];
        if (Physics.Raycast(transform.position + transform.up * 1 + transform.forward, transform.up * -1, out hit, LayerMask.NameToLayer("Ground")))
        {
            MonoBehaviour monohit = hit.transform.GetComponent<MonoBehaviour>();
            var cell = monohit as Cell;
            if (cell != null)
            {
                tempCells[0]  = cell;
            }
        }
        if (Physics.Raycast(transform.position + transform.up * 1 + transform.forward*-1, transform.up * -1, out hit, LayerMask.NameToLayer("Ground")))
        {
            MonoBehaviour monohit = hit.transform.GetComponent<MonoBehaviour>();
            var cell = monohit as Cell;
            if (cell != null)
            {
                tempCells[1] = cell;
            }
        }
        return tempCells;
    }
}

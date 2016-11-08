using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CellHelper {

    public static Cell GetCurrentCell(Transform transform)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 1, Vector3.down, out hit, LayerMask.GetMask("Ground")))
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

    public static Cell GetCellAtVector(Vector3 startPos)
    {
        RaycastHit hit;
        if (Physics.Raycast(startPos + Vector3.up * 1, Vector3.down, out hit, LayerMask.GetMask("Ground")))
        {
            Cell cell = hit.transform.GetComponent<Cell>();
            return cell;
        }
        return null;
    }

    public static Cell[] GetFrontBackCells(Transform transform)
    {
        RaycastHit hit;
        Cell[] tempCells = new Cell[2];
        Vector3[] directions = { transform.forward*-1, transform.forward};

        for (int i = 0; i < directions.Length; i++)
        {
            if (Physics.Raycast(transform.position + transform.up * 1 + directions[i], transform.up * -1, out hit, LayerMask.GetMask("Ground")))
            {
                MonoBehaviour monohit = hit.transform.GetComponent<MonoBehaviour>();
                var cell = monohit as Cell;
                if (cell != null)
                {
                    tempCells[i] = cell;
                }
            }
        }
        return tempCells;
    }

    public static Cell[] GetCellsAround(Transform transform)
    {
        RaycastHit hit;
        List<Cell> tempCells = new List<Cell>();
        Vector3[] directions = {Vector3.back, Vector3.forward, Vector3.left, Vector3.right};

        for (int i = 0; i < directions.Length; i++)
        {
            if (Physics.Raycast(transform.position + transform.up * 1 + directions[i]*2, transform.up * -1, out hit, LayerMask.GetMask("Ground")))
            {
                MonoBehaviour monohit = hit.transform.GetComponent<MonoBehaviour>();
                var cell = monohit as Cell;
                if (cell != null)
                {
                    tempCells.Add(cell);
                }
            }
        }
        return tempCells.ToArray();
    }


    public static Cell GetCellFromDirection(Vector3 startPosition, Vector3 direction, int distance, int solidLayerMask)
    {
        if (!Physics.Raycast(startPosition + Vector3.up * 1, direction, distance * 2, solidLayerMask))
        {
            RaycastHit hit;
            if (Physics.Raycast(startPosition + Vector3.up * 2 + direction * 2 * distance, Vector3.down, out hit, LayerMask.GetMask("Ground"))) //LayerMask.GetMask("Ground")))
            {
                Cell cell = hit.transform.GetComponent<Cell>();
                return cell;
            }
        }
        return null;
    }
}

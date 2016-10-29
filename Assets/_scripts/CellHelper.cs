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
}

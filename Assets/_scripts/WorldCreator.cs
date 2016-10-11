using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class WorldCreator : MonoBehaviour {

    public GameObject gridCellPrefab;
    public GameObject worldCreator;
    Transform cellTransform;
    public int gridSize;
    public int gridMidPos;
    bool gridIsCreated = false;

    // Use this for initialization
    void Start () {
        //cellTransform = gridCellPrefab.transform;
        //CreateGrid(gridMidPos, gridMidPos, gridSize, gridSize);
	}
    public void CreateGrid()
    {
        if (!gridIsCreated)
        {
            cellTransform = gridCellPrefab.transform;
            if (gridSize % 2 != 0 && gridSize % 2 != 0)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    for (int z = 0; z < gridSize; z++)
                    {
                        GameObject temp = Instantiate(gridCellPrefab, new Vector3(gridMidPos - (gridSize / 2 * cellTransform.localScale.x) + x * cellTransform.localScale.x, 0, gridMidPos - (gridSize / 2 * cellTransform.localScale.z) + z * cellTransform.localScale.z), Quaternion.identity) as GameObject;
                        temp.transform.name += " :" + x+"; "+z;
                        temp.transform.parent = worldCreator.transform;
                    }
                }
                gridIsCreated = true;
            }
            else
            {
                for (int x = -gridSize / 2; x < gridSize / 2; x++)
                {
                    for (int z = -gridSize / 2; z < gridSize / 2; z++)
                    {
                        GameObject temp = Instantiate(gridCellPrefab, new Vector3(gridMidPos + x * cellTransform.localScale.x + cellTransform.localScale.x / 2, 0, gridMidPos + z * cellTransform.localScale.z + cellTransform.localScale.z / 2), Quaternion.identity) as GameObject;
                        temp.transform.parent = worldCreator.transform;
                    }
                }
                gridIsCreated = true;
            }

        }
        else{
            for(int i = transform.childCount-1; i > -1; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
            gridIsCreated = false;
            
        }
    }
}

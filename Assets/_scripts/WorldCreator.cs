using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class WorldCreator : MonoBehaviour
{
    public static WorldCreator Instance;
    public Transform[,] gridCellMatrix; 
    public GameObject gridCellPrefab;
    public GameObject worldCreator;
    Transform cellTransform;
    public int gridSizeZ;
    public int gridSizeX;
    public int gridMidPos;
    bool gridIsCreated = false;

    // Use this for initialization
    void Start ()
    {
    }
    public void CreateGrid()
    {
        Instance = this;
        gridCellMatrix = new Transform[gridSizeZ, gridSizeX];
        if (!gridIsCreated)
        {
            cellTransform = gridCellPrefab.transform;
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    GameObject temp = Instantiate(gridCellPrefab, new Vector3(gridMidPos - (gridSizeX / 2 * cellTransform.localScale.x) + x * cellTransform.localScale.x, 0, gridMidPos - (gridSizeZ / 2 * cellTransform.localScale.z) + z * cellTransform.localScale.z), Quaternion.identity) as GameObject;
                    temp.transform.name += " :" + x+"; "+z;
                    temp.transform.parent = worldCreator.transform;
                    gridCellMatrix[x, z] = temp.transform;
                }
            }
            gridIsCreated = true;
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

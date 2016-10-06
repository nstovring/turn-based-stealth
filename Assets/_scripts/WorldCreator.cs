using UnityEngine;
using System.Collections;

public class WorldCreator : MonoBehaviour {

    public GameObject gridCellPrefab;
    public GameObject worldCreator;
    Transform cellTransform;
    public int gridSize;
    public int gridMidPos;

    // Use this for initialization
    void Start () {
        cellTransform = gridCellPrefab.transform;
        
        CreateGrid(gridMidPos, gridMidPos, gridSize, gridSize);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void CreateGrid(float PosX, float PosZ, int LengthX, int LengthZ)
    {
        if (LengthX % 2 != 0 && LengthZ % 2 != 0)
        {
            for (int x = 0; x < LengthX; x++)
            {
                for (int z = 0; z < LengthZ; z++)
                {
                    GameObject temp = Instantiate(gridCellPrefab, new Vector3(PosX -(LengthX/2 * cellTransform.localScale.x) + x * cellTransform.localScale.x, 0, PosZ - (LengthZ / 2 * cellTransform.localScale.z) + z * cellTransform.localScale.z), Quaternion.identity) as GameObject;
                    temp.transform.parent = worldCreator.transform;
                }
            }
        }
        else{
            for (int x = -LengthX / 2; x < LengthX / 2; x++)
            {
                for (int z = -LengthZ / 2; z < LengthZ / 2; z++)
                {
                    GameObject temp = Instantiate(gridCellPrefab, new Vector3(PosX + x * cellTransform.localScale.x + cellTransform.localScale.x/2, 0, PosZ + z * cellTransform.localScale.z + cellTransform.localScale.z / 2), Quaternion.identity) as GameObject;
                    temp.transform.parent = worldCreator.transform;
                }
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour, IClickable {
    public bool isOccupied;
    public bool isWithinViewRange;
    public Character occupier;
    public Transform[] viewCellEdges = new Transform[4];
    public Transform myTransform;

    public Door door;
    // Use this for initialization
    void Awake()
    {
        myTransform = transform;

        for (int i = 0; i < viewCellEdges.Length; i++)
        {
            viewCellEdges[i] = Instantiate(viewCellEdges[i], myTransform.position + new Vector3(0, 1, 0), Quaternion.identity) as Transform;
            viewCellEdges[i].parent = myTransform;
            viewCellEdges[i].gameObject.SetActive(false);
        }

	}

    public void SetActiveViewEdge(int viewCellType, bool active)
    {
        if(viewCellType< viewCellEdges.Length)
        viewCellEdges[viewCellType].gameObject.SetActive(active);
    }

	// Update is called once per frame
  
    public void LeftClicked()
    {
        Character character = GameManager.Instance.PlayerCharacters[GameManager.Instance.currentPlayer];
        if (character.currentCell == this)
        {
            return;
        }
        //else queue movement to nearest grid
        if (character.mySeeker.currentCell == this)
        {
            character.AddActionToQueue(character.QueuedMove(transform));
            character.StartActions();
        }
        //throw new System.NotImplementedException();
    }

    public void RightClicked()
    {
        throw new System.NotImplementedException();
    }
}

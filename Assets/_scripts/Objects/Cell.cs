using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour, IClickable {
    public bool isOccupied;
    public bool isWithinViewRange;
    public Character occupier;
    public Transform[] viewCellEdges = new Transform[2];

    // Use this for initialization
    void Start ()
	{
        //yield return new WaitForSeconds(0.1f);
        viewCellEdges[0] = Instantiate(viewCellEdges[0], transform.position + new Vector3(0,1,0), Quaternion.identity) as Transform;
        viewCellEdges[1] = Instantiate(viewCellEdges[1], transform.position + new Vector3(0, 1, 0), Quaternion.identity) as Transform;

        viewCellEdges[0].parent = transform;
        viewCellEdges[1].parent = transform;

        viewCellEdges[0].gameObject.SetActive(false);
        viewCellEdges[1].gameObject.SetActive(false);

	}

    public void SetActiveViewEdge(int viewCellType, bool active)
    {
        viewCellEdges[viewCellType].gameObject.SetActive(active);
    }

	// Update is called once per frame
	
    void OnMouseEnter()
    {
       
    }

    void OnMouseExit()
    {
        
    
    }
    public void LeftClicked()
    {
        Character character = GameManager.Instance.PlayerCharacters[GameManager.Instance.currentPlayer];
        if (character.currentCell == this)
        {
            return;
        }
        //else queue movement to nearest grid
        character.AddActionToQueue(character.QueuedMove(transform));
        
        character.StartActions();
        //throw new System.NotImplementedException();
    }

    public void RightClicked()
    {
        throw new System.NotImplementedException();
    }
}

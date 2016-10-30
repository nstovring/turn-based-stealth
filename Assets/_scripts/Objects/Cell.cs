using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour, IClickable {
    public bool isOccupied;
    public bool isWithinViewRange;
    public Character occupier;
    public Transform[] viewCellEdges = new Transform[4];

    // Use this for initialization
    void Start ()
	{
        //yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < viewCellEdges.Length; i++)
        {
            viewCellEdges[i] = Instantiate(viewCellEdges[i], transform.position + new Vector3(0, 1, 0), Quaternion.identity) as Transform;
            viewCellEdges[i].parent = transform;
            viewCellEdges[i].gameObject.SetActive(false);
        }

	}

    public void SetActiveViewEdge(int viewCellType, bool active)
    {
        if(viewCellType< viewCellEdges.Length)
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

using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour, IClickable {
    public bool isOccupied;
    public bool isWithinViewRange;
    public Character occupier;
    private Renderer myRenderer;
    public Transform viewCellEdge;
	// Use this for initialization
	void Start ()
	{
	    viewCellEdge = Instantiate(viewCellEdge, transform.position + new Vector3(0,1,0), Quaternion.identity) as Transform;

	    viewCellEdge.parent = transform;
        viewCellEdge.gameObject.SetActive(false);
	    myRenderer = GetComponent<Renderer>();
	}

    public void SetActiveViewEdge(bool active)
    {
        viewCellEdge.gameObject.SetActive(active);
    }

	// Update is called once per frame
	
    void OnMouseEnter()
    {
        //if(GameManager.Instance.PlayerTurn)
        //    GameManager.Instance.PlayerCharacters[0].mySeeker.SetPathToDestination(transform);
    }

    void OnMouseExit()
    {
        
        //myRenderer.materials[0].SetColor("_Color", Color.white);

        //if (GameManager.Instance.PlayerTurn)
        //    GameManager.Instance.PlayerCharacters[0].mySeeker.ResetPosition();
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

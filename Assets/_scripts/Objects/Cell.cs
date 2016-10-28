using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour, IClickable {
    public bool isOccupied;
    public bool isWithinViewRange;
    public Character occupier;
    private Renderer myRenderer;
	// Use this for initialization
	void Start ()
	{
	    myRenderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseEnter()
    {
        myRenderer.materials[0].SetColor("_Color", Color.blue);
    }

    void OnMouseExit()
    {
        if (isWithinViewRange)
        {
            myRenderer.materials[0].SetColor("_Color", Color.red);
        }
        else
        {
            myRenderer.materials[0].SetColor("_Color", Color.white);
        }
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

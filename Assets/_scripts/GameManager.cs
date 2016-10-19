using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<Character> PlayerCharacters;
    public List<Character> GuardCharacters;

    public bool instantTurnBased;

    public AIManager aiManager;
    public UserInterfaceManager uiManager;
    public InputManager inputManager;

    public int currentPlayer = 0;
    private int valueStolen;
    public List<IWinable> objectives = new List<IWinable>();
    public List<IWinable> escapeObjective = new List<IWinable>();

    public 
    
	void Start ()
	{
        PlayerCharacters = new List<Character>();
	    Instance = this;
	}

    public void AddPlayerCharacters(Character character)
    {
        if(PlayerCharacters == null)
            PlayerCharacters = new List<Character>();
        PlayerCharacters.Add(character);
    }

    public void AddGuardCharacters(Character character)
    {
        if (GuardCharacters == null)
            GuardCharacters = new List<Character>();
        GuardCharacters.Add(character);
    }

    public int ValueStolen
    {
        get { return valueStolen; }
        set { valueStolen = value; }
    }

    private bool playerTurn = true;
    private bool guardTurn = false;

    public void giveGuardsActions()
    {
        foreach (var guardCharacter in GuardCharacters)
        {
            guardCharacter.newActions();
        }

    }

    public void givePlayerActions()
    {
        foreach (var playCharacter in PlayerCharacters)
        {
            playCharacter.newActions();
        }
    }

    // Update is called once per frame
    void Update () {
        if (!instantTurnBased)
        {
            CharacterIterator();
        }
    }

    public void CharacterIterator()
    {
        if (AllActionsSpend())
        {
            playerTurn = !playerTurn;
            if (playerTurn)
            {
                givePlayerActions();
            }
            else
            {
                giveGuardsActions();
            }
        }
        
    }

    public bool AllActionsSpend()
    {
        foreach (var player in PlayerCharacters)
        {
            if (player.ActionPointsLeft())
            {
                return false;
            }
        }
        foreach (var guard in GuardCharacters)
        {
            if (guard.ActionPointsLeft())
            {
                return false;
            }
        }
        return true;
    }

    public bool AllObjectivesComplete()
    {
        Debug.Log("complete check calling");
        bool allComplete = true;
        foreach(IWinable objective in objectives)
        {
            if(objective.HasObjectiveChain() && objective.IsObjectiveComplete())
            {
                //update objectives;
            }
            if (!objective.IsObjectiveComplete())
            {
                allComplete = false;
                break;
            }
            
        }
        if (allComplete)
        {
            Debug.Log("YOU WIIIIIIIN");
            //YOU WIIIN
        }
        else
        {
            //update UI 
        }
        return allComplete;
    }

}

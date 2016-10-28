using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<PlayerCharacter> PlayerCharacters;
    public List<Guard> GuardCharacters;

    public bool instantTurnBased;

    public AIManager aiManager;
    public UserInterfaceManager uiManager;
    public InputManager inputManager;

    public int currentPlayer = 0;
    private int valueStolen;
    public List<IWinable> objectives = new List<IWinable>();
    public List<IWinable> escapeObjective = new List<IWinable>();

    public bool PlayerTurn = true;
    
	void Start ()
	{
        PlayerCharacters = new List<PlayerCharacter>();
	    Instance = this;
	}

    public void AddPlayerCharacters(PlayerCharacter character)
    {
        if(PlayerCharacters == null)
            PlayerCharacters = new List<PlayerCharacter>();
        PlayerCharacters.Add(character);
    }

    public void AddGuardCharacters(Guard character)
    {
        if (GuardCharacters == null)
            GuardCharacters = new List<Guard>();
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
    public void PlotGuardRoutes()
    {
        foreach (var guardCharacter in GuardCharacters)
        {
            guardCharacter.virtualCharacter.VisualizeRoute();

        }

    }

    public void givePlayerActions()
    {
        foreach (var playCharacter in PlayerCharacters)
        {
            playCharacter.newActions();
        }
    }

    public void GameOver()
    {
        Debug.Log("You Lost FOO!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
            
            if (playerTurn)
            {
                givePlayerActions();
                PlotGuardRoutes();
            }
            else
            {
                giveGuardsActions();
            }
            playerTurn = !playerTurn;
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

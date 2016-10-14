using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<Character> PlayerCharacters;

    public AIManager aiManager;
    public UserInterfaceManager uiManager;
    public InputManager inputManager;

    public int currentPlayer = 0;
    private int valueStolen;
    
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
	
    public int ValueStolen
    {
        get { return valueStolen; }
        set { valueStolen = value; }
    }

	// Update is called once per frame
	void Update () {
	
	}
}

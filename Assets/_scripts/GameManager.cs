using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public AIManager aiManager;
    public UserInterfaceManager uiManager;
    public InputManager inputManager;
	// Use this for initialization
    private int valueStolen;
    
	void Start ()
	{
	    instance = this;
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

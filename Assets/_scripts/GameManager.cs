using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public AIManager aiManager;
    public UserInterfaceManager uiManager;
    public InputManager inputManager;
	// Use this for initialization
	void Start ()
	{
	    instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

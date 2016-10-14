using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character
{
    public Guard TestGuard;
    // Use this for initialization
    void Start()
    {
        Initialize();
        totalActionPoints = 3;
        newActions();
    }

    void Initialize()
    {
        GameManager.Instance.AddPlayerCharacters(this);
    }
    // Update is called once per frame
    void Update ()
	{
        if (Input.GetKeyUp(KeyCode.S))
        {
            StartActions();
        }
    }
}

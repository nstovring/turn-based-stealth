﻿using UnityEngine;
using System.Collections;

public class PlayerCharacter : Character
{
    public Guard TestGuard;
    // Use this for initialization
    void Start()
    {
        Initialize();
        //newActions();
    }
    public override void ChangeCurrentCell(Transform destination)
    {
        base.ChangeCurrentCell(destination);
        foreach (EscapeObjective obj in GameManager.Instance.escapeObjective)
        {
            obj.IsPlayerInCells();
        }
        Debug.Log("calling new method");
    }
    public override IEnumerator Move(Transform destination)
    {
        if(GameManager.Instance.instantTurnBased)
        GameManager.Instance.giveGuardsActions();
        return base.Move(destination);
    }

    void Initialize()
    {
        GameManager.Instance.AddPlayerCharacters(this);
        GetCurrentCell();
    }
    // Update is called once per frame
    void Update ()
	{
        if (!ActionPointsLeft())
        {
            //GameManager.Instance.giveGuardsActions();
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            newActions();
        }
    }
}

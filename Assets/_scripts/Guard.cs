using System;
using UnityEngine;
using System.Collections;

public class Guard : Character
{
    private bool conscious = true;
    private bool stunned = false;

    public enum GuardState {Conscious, Unconscious, Stunned, Alert};
    public GuardState MyGuardState = GuardState.Conscious;
    struct Inventory
    {
        int coin;
        bool key;

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void ChangeState(GuardState newState)
    {
        MyGuardState = newState;
    }

    public IEnumerator GetBlackJacked()
    {
        yield return new WaitForSeconds(1f);
        ChangeState(GuardState.Unconscious);
    }

    IEnumerator GetPickPocketed()
    {
        return null;
    }
}

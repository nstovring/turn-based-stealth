using System;
using UnityEngine;
using System.Collections;

public class Guard : Character
{
    private Animator myAnimator;

    public enum GuardState {Conscious, Unconscious, Stunned, Alert};
    public GuardState MyGuardState = GuardState.Conscious;
    struct Inventory
    {
        int coin;
        bool key;
    }

	// Use this for initialization
	void Start ()
	{
        myAnimator = transform.parent.GetComponent<Animator>();
        myAnimator.SetBool("Conscious", true);
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
        ChangeState(GuardState.Unconscious);
        myAnimator.SetBool("Conscious", false);
        yield return new WaitForSeconds(2f);
        Debug.Log("Guard Unconscious");
    }

    IEnumerator GetPickPocketed()
    {
        return null;
    }
}

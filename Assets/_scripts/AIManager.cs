using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIManager : MonoBehaviour {
    List<Guard> GuardCharacters;
    // Use this for initialization
    void Start () {
        GuardCharacters = GameManager.Instance.GuardCharacters;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public IEnumerator GiveGuardsTurns()
    {
        foreach(Guard guard in GuardCharacters)
        {
            guard.newActions();
            while(!guard.GetTurnDone())
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }
}

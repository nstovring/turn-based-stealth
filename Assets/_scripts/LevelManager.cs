using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class LevelManager : MonoBehaviour
{
    private GameObject[] Walls;
	// Use this for initialization
	void Start () {
	    GetWalls();
	    StartCoroutine(FeedWallsPlayerPos());
	}

    public void GetWalls()
    {
        Walls = GameObject.FindGameObjectsWithTag("Wall");
    }

    IEnumerator FeedWallsPlayerPos()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            foreach (var wall in Walls)
            {
                wall.GetComponent<Renderer>().material.SetVector("_PlayerPos", GameManager.Instance.PlayerCharacters[GameManager.Instance.currentPlayer].transform.position);
            }
        }
    }
    // Update is called once per frame
    void Update () {
	
	}
}

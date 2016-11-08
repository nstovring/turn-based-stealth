using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class LevelManager : MonoBehaviour
{
    private GameObject[] walls;
    private List<Renderer> wallRenderers;
    // Use this for initialization
    void Start () {
	    GetWalls();
	    StartCoroutine(FeedWallsPlayerPos());
	}

    public void GetWalls()
    {
        wallRenderers = new List<Renderer>();
        walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (var wall in walls)
        {
            wallRenderers.Add(wall.GetComponent<Renderer>());
        }
    }



    IEnumerator FeedWallsPlayerPos()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

            if (wallRenderers != null)
                wallRenderers[0].sharedMaterial.SetVector("_PlayerPos", GameManager.Instance.PlayerCharacters[GameManager.Instance.currentPlayer].transform.position);

            //foreach (var wall in wallRenderers)
            //{
            //    wall.sharedMaterial.SetVector("_PlayerPos", GameManager.Instance.PlayerCharacters[GameManager.Instance.currentPlayer].transform.position);
            //}
        }
    }
    // Update is called once per frame
    void Update () {
	
	}
}

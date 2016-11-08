using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Item : MonoBehaviour, IClickable, IStealable
{
    public int value;

    public Cell currentCell;
    public Cell[] currentCells;

    void Start()
    {
       // currentCell = CellHelper.GetCurrentCell(transform);
        currentCells = CellHelper.GetCellsAround(transform);
        GetCurrentCell();
    }

    public void GetCurrentCell()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up*-1, out hit,LayerMask.GetMask("Ground")))
        {
            MonoBehaviour monohit = hit.transform.GetComponent<MonoBehaviour>();
            var cell = monohit as Cell;
            if (cell != null)
            {
                currentCell = cell;
            }
        }
    }

    public void LeftClicked()
    {
        Debug.Log("Im being clicked!!");
        Character character = GameManager.Instance.PlayerCharacters[GameManager.Instance.currentPlayer];
        //If player next to object getstolen
        if (Vector3.Distance(character.currentCell.myTransform.position, currentCell.myTransform.position) <= 2)
        {
            character.AddActionToQueue(GetStolen(character.transform));
        }
        else //else queue movement to nearest grid
        {
            character.AddActionToQueue(character.QueuedMove(getNearestGrid(currentCell.myTransform)));
            character.AddActionToQueue(GetStolen(character.transform));
        }
        character.StartActions();
    }

    Transform getNearestGrid(Transform trsfm)
    {
        List<float> distances = new List<float>();
        for (int i = 0; i < currentCells.Length; i++)
        {
            distances.Add(Vector3.Distance(currentCells[i].myTransform.position, trsfm.position));
        }
        float minDistance = distances.Min();
        for (int i = 0; i < currentCells.Length; i++)
        {
            if (distances[i] <= minDistance + 0.1f && distances[i] >= minDistance - 0.1f)
            {
                return currentCells[i].myTransform;
            }
        }
        return null;
    }

    public void RightClicked()
    {
        throw new System.NotImplementedException();
    }

    public virtual IEnumerator GetStolen(Transform playerTransform)
    {
        if (GameManager.Instance.PlayerCharacters[0].ActionPointsLeft())
        {
            Debug.Log("calling getStolen");
            //Run animation
            while (Vector3.Distance(transform.position, playerTransform.position) > 0.1)
            {
                transform.position = Vector3.Lerp(transform.position, playerTransform.position, 0.1f);
                yield return new WaitForEndOfFrame();
            }
            //Add value to score
            GameManager.Instance.ValueStolen += value;

            //Destroy Prefab
            GameManager.Instance.PlayerCharacters[0].actionPoints--;
            Destroy(gameObject);
        }
    }
}

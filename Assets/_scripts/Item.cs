﻿using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour, IClickable, IStealable
{
    public int value;

    public Cell CurrentCell;


    void Start()
    {
        GetCurrentCell();
    }

    private void GetCurrentCell()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up*-1, out hit,LayerMask.NameToLayer("Ground")))
        {
            MonoBehaviour monohit = hit.transform.GetComponent<MonoBehaviour>();
            var cell = monohit as Cell;
            if (cell != null)
            {
                CurrentCell = cell;
            }
        }
    }

    public void LeftClicked()
    {
        Character character = GameManager.Instance.PlayerCharacters[GameManager.Instance.currentPlayer];
        //If player next to object getstolen
        if (character.currentCell == CurrentCell)
        {
            character.AddActionToQueue(GetStolen(character.transform));
        }
        else //else queue movement to nearest grid
        {
            character.AddActionToQueue(character.QueuedMove(transform));
            character.AddActionToQueue(GetStolen(character.transform));
        }
        character.StartActions();
    }

    public void RightClicked()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator GetStolen(Transform playerTransform)
    {
        //Run animation
        while (Vector3.Distance(transform.position, playerTransform.position) > 0.1)
        {
            transform.position = Vector3.Lerp(transform.position, playerTransform.position, 0.1f);
            yield return new WaitForEndOfFrame();
        }
        //Add value to score
        GameManager.Instance.ValueStolen += value;
        //Destroy Prefab
        Destroy(gameObject);
    }
}

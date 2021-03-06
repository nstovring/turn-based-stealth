﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerCharacter : Character
{
    // Use this for initialization
    void Start()
    {
        Initialize();
    }
    public override void ChangeCurrentCell(Transform destination)
    {
        base.ChangeCurrentCell(destination);
        foreach (EscapeObjective obj in GameManager.Instance.escapeObjective)
        {
            obj.IsPlayerInCells();
        }
    }
    public override IEnumerator Move(Transform destination)
    {
        if(GameManager.Instance.instantTurnBased)
        GameManager.Instance.giveGuardsActions();
        return base.Move(destination);
    }

    public override void Initialize()
    {
        base.Initialize();
        GameManager.Instance.AddPlayerCharacters(this);
        visualizeViewRange(false);
    }


    public override Cell[] GetVisionConeTransforms(int _coneSize)
    {
        IEnumerable<Cell> tempViewConeList = new List<Cell>();

        Vector3[] viewDirections = {Vector3.forward, Vector3.right, Vector3.forward*-1, Vector3.right * -1};
        Vector3[] orthogonalDirections = { Vector3.right, Vector3.forward * -1, Vector3.right * -1, Vector3.forward};

        Cell[][] originCellArray = new Cell[4][];

        for (int i = 0; i < viewDirections.Length; i++)
        {
            Cell[] transforms = GetCellArrayFromDirection(currentCell.transform.position, viewDirections[i],
                actionPoints);
            originCellArray[i] = transforms;
        }
        List<Cell> sideCellArray = new List<Cell>();

        for (int i = 0; i < originCellArray.Length; i++)
        {
            for (int j = 0; j < originCellArray[i].Length; j++)
            {
                Cell cell = originCellArray[i][j];
                //Minus j because we want the size to decrease the further along the line we progress
                for (int k = actionPoints - j; k > 0; k--)
                {
                    Cell leftCell = CellHelper.GetCellFromDirection(cell.transform.position, orthogonalDirections[i], k,solidLayer);
                    if (leftCell && !sideCellArray.Contains(leftCell))
                    {
                        sideCellArray.Add(leftCell);
                    }
                    Cell rightCell = CellHelper.GetCellFromDirection(cell.transform.position, orthogonalDirections[(i+2)%4], k, solidLayer);
                    if (rightCell && !sideCellArray.Contains(rightCell))
                    {
                        sideCellArray.Add(rightCell);
                    }
                }
            }

        }
        for (int i = 0; i < viewDirections.Length; i++)
        {
            tempViewConeList = tempViewConeList.Concat(originCellArray[i]);
        }
        tempViewConeList = tempViewConeList.Concat(sideCellArray);

        return tempViewConeList.ToArray();
    }

    Cell[] GetCellArrayFromDirection(Vector3 startPosition, Vector3 direction, int distance)
    {
        List<Cell> tempList = new List<Cell>();
        for (int i = 0; i < distance; i++)
        {
            Cell cell = CellHelper.GetCellFromDirection(startPosition, direction, i,solidLayer);
            if (cell)
            tempList.Add(cell);
            //tempList.Add(CellHelper.GetCellFromDirection(startPosition, direction, i,solidLayer));
        }
        return tempList.ToArray();
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

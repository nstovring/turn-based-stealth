using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

    public override Transform[] GetVisionConeTransforms(int _coneSize)
    {
        _coneSize = actionPoints;
        IEnumerable<Transform> tempViewConeList = new List<Transform>();

        Vector3[] viewDirections = {Vector3.forward, Vector3.right, Vector3.forward*-1, Vector3.right * -1};
        Vector3[] orthogonalDirections = { Vector3.right, Vector3.forward * -1, Vector3.right * -1, Vector3.forward};

        Transform[][] originCellArray = new Transform[4][];

        for (int i = 0; i < viewDirections.Length; i++)
        {
            Transform[] transforms = GetCellArrayFromDirection(currentCell.transform.position, viewDirections[i],
                actionPoints);
            originCellArray[i] = transforms;
            
        }
        List<Transform> sideCellArray = new List<Transform>();

        for (int i = 0; i < originCellArray.Length; i++)
        {
            for (int j = 0; j < originCellArray[i].Length; j++)
            {
                Transform cell = originCellArray[i][j];
                for (int k = originCellArray[i].Length - j; k > 0; k--)
                {
                    Transform leftCell = GetCellFromDirection(cell.transform.position, orthogonalDirections[i], k);
                    if (leftCell && !sideCellArray.Contains(leftCell))
                    {
                        sideCellArray.Add(leftCell);
                    }
                }
            }

        }
        tempViewConeList = tempViewConeList.Concat(originCellArray[0]);
        tempViewConeList = tempViewConeList.Concat(originCellArray[1]);
        tempViewConeList = tempViewConeList.Concat(originCellArray[2]);
        tempViewConeList = tempViewConeList.Concat(originCellArray[3]);
        tempViewConeList = tempViewConeList.Concat(sideCellArray);

        return tempViewConeList.ToArray();
    }

    Transform[] GetCellArrayFromDirection(Vector3 startPosition, Vector3 direction, int distance)
    {
        List<Transform> tempList = new List<Transform>();
        for (int i = 0; i < distance; i++)
        {
            Transform cell = GetCellFromDirection(startPosition, direction, i);
            if (cell)
            tempList.Add(GetCellFromDirection(startPosition, direction, i));
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

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VirtualGuardCharacter : Guard {
    public Guard super;
    public struct Route
    {
        Transform destiantion;
        Vector3 direction;
    }
    public Transform destination;
    public List<Transform> route = new List<Transform>();

	// Use this for initialization
	void Start () {
        patrolTransforms = super.patrolTransforms;
        actionPoints = super.actionPoints;
        StartCoroutine(IterateThroughPatrolRoutes());
    }

    // Update is called once per frame
    public override void Update()
    {

    }
    public void GiveActionpoints()
    {
        if(super.actionPoints == 0)
        {
            actionPoints = super.totalActionPoints;
        }
        else
        {
            actionPoints = super.actionPoints;
        }
    }
    public void GetActions()
    {
        myAgent.path = super.myAgent.path;
    }
    public override IEnumerator Move(Transform destination)
    {
        route.Add(destination);
        yield return base.Move(destination);
    }

    public override void visualizeViewRange(Color color)
    {
        
    }
    public void VisualizeRoute()
    {
        route = new List<Transform>();
        patrolint = super.patrolint;
        transform.position = super.transform.position;
        transform.rotation = super.transform.rotation;
        GiveActionpoints();
        GetCurrentCell();
        
    }
}

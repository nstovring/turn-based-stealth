using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        LevelManager myLevelManager = (LevelManager)target;
        if (GUILayout.Button("Get Walls"))
        {
            myLevelManager.GetWalls();
        }
    }
   
}

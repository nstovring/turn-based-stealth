using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(WorldCreator))]
public class WorldCreatorEditor : Editor {
    SerializedProperty worldCreator;
    // Use this for initialization
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WorldCreator myWorldCrweator = (WorldCreator)target;
        if(GUILayout.Button("Create Grid"))
        {
            myWorldCrweator.CreateGrid();
        }
    }
    void OnEnable()
    {
        worldCreator = serializedObject.FindProperty("WorldCreator");
    }
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

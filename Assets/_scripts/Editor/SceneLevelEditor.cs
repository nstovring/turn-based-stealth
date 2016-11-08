using UnityEngine;
using System.Collections;
using UnityEditor;

[InitializeOnLoad]
public class SceneLevelEditor : Editor
{
    private static Vector3 _cameraPositionBeforeEditMode;
    private static bool _cameraOrthoBeforeEditMode;
    private static Quaternion _cameraRotBeforeEditMode;
    private static SceneView currentSceneView;
    private static bool levelEditMode = false;
    // [MenuItem("Test/Move Scene View Camera")]
    static SceneLevelEditor()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;

        EditorApplication.update -= OnUpdate;
        EditorApplication.update += OnUpdate;
    }

  

    void OnDestroy () {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        EditorApplication.update -= OnUpdate;
    }
	
	
    [MenuItem("LevelEdit/Activate Level Edit Mode")]
    static void GoIntoLevelEditMode()
    {
        levelEditMode = true;
    }

    [MenuItem("LevelEdit/Deactivate Level Edit Mode")]
    static void GoOutOfLevelEditMode()
    {
        levelEditMode = false;
    }

    [MenuItem("LevelEdit/Load Prefabs")]
    static void LoadPrefabs()
    {
        spawnablePrefabs = AssetDatabase.LoadAllAssetsAtPath("Assets/_prefabs");
        Debug.Log(spawnablePrefabs.Length);
    }
    private static int prefabSelector;
    private static Object[] spawnablePrefabs;
    private static GameObject currentSpawnedPrefab;
    private static void OnSceneGUI(SceneView sceneview)
    {

        if (levelEditMode)
        {
            Event e = Event.current;
            prefabSelector = e.numeric ? e.character:0;
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (currentSpawnedPrefab != spawnablePrefabs[prefabSelector])
                {
                    Destroy(currentSpawnedPrefab);
                    currentSpawnedPrefab = Instantiate(spawnablePrefabs[prefabSelector]) as GameObject;
                }
                if (!currentSpawnedPrefab)
                {
                    currentSpawnedPrefab = Instantiate(spawnablePrefabs[prefabSelector]) as GameObject;
                }
                currentSpawnedPrefab.transform.position = hit.transform.position;
                Selection.activeGameObject = currentSpawnedPrefab;
            }

            Vector3 position = SceneView.lastActiveSceneView.pivot;
           
            position += EvaluateCameraMovement(Event.current.keyCode);
            SceneView.lastActiveSceneView.pivot = position;
            SceneView.lastActiveSceneView.Repaint();
        }
        //currentSceneView = sceneview;
    }

    static Vector3 EvaluateCameraMovement(KeyCode key)
    {
        if (key == KeyCode.A)
        {
            return new Vector3(-1,0,0);
        }
        if (key == KeyCode.D)
        {
            return new Vector3(1, 0, 0);
        }
        if (key == KeyCode.S)
        {
            return new Vector3(0, 0, -1);
        }
        if (key == KeyCode.W)
        {
            return new Vector3(0, 0, 1);
        }
        return Vector3.zero;
    }
    static void OnUpdate()
    {
       

        //if (_cameraPositionBeforeEditMode == Vector3.zero)
        //{
        //    _cameraOrthoBeforeEditMode = SceneView.lastActiveSceneView.orthographic;
        //    _cameraRotBeforeEditMode = SceneView.lastActiveSceneView.rotation;
        //    _cameraPositionBeforeEditMode = SceneView.lastActiveSceneView.pivot;
        //}


        //Vector3 position = SceneView.lastActiveSceneView.pivot;
        //position +=  new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
        //SceneView.lastActiveSceneView.pivot = position;
        //SceneView.lastActiveSceneView.Repaint();
    }
}

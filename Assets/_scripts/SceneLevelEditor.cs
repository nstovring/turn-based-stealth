using UnityEngine;
using System.Collections;
using UnityEditor;

[InitializeOnLoad]
[ExecuteInEditMode]
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
        spawnablePrefabs = Resources.LoadAll("_prefabs/_spawnablePrefabs");
        levelEditMode = true;
    }

    [MenuItem("LevelEdit/Deactivate Level Edit Mode")]
    static void GoOutOfLevelEditMode()
    {
        DestroyImmediate(currentSpawnedPrefab);
        levelEditMode = false;
    }

    [MenuItem("LevelEdit/Load Prefabs")]
    static void LoadPrefabs()
    {
        spawnablePrefabs = Resources.LoadAll("_prefabs/_spawnablePrefabs");
        Debug.Log(spawnablePrefabs.Length);
    }
    private static int prefabSelector;
    private static int OldprefabSelector;
    private static Object[] spawnablePrefabs;
    private static GameObject currentSpawnedPrefab;


    private static void OnSceneGUI(SceneView sceneview)
    {

        if (levelEditMode)
        {
            Event e = Event.current;
            int controlID = GUIUtility.GetControlID(FocusType.Keyboard);
            Debug.Log(controlID);
            switch (e.GetTypeForControl(controlID))//e.type)
            {
                case EventType.keyDown:
                    {
                        Debug.Log("Rotating prefab");

                        if (e.keyCode == (KeyCode.D))
                        {
                            currentSpawnedPrefab.transform.Rotate(Vector3.up * 90);
                            break;
                        }
                        if (e.keyCode == (KeyCode.A))
                        {
                            currentSpawnedPrefab.transform.Rotate(Vector3.down * 90);
                            break;
                        }
                        if (e.keyCode == (KeyCode.E))
                        {
                            prefabSelector ++;
                            prefabSelector = prefabSelector % spawnablePrefabs.Length;
                            break;
                        }
                        if (e.keyCode == (KeyCode.Q))
                        {
                            prefabSelector--;
                            prefabSelector = prefabSelector % spawnablePrefabs.Length;
                            break;
                        }
                        if (e.keyCode == (KeyCode.Escape))
                        {
                            DestroyImmediate(currentSpawnedPrefab);
                            GoOutOfLevelEditMode();
                        }
                        break;
                    }
                case EventType.ScrollWheel:
                    {
                        //int scrollDelta = (int)e.delta.magnitude / 3;
                        //prefabSelector += scrollDelta;
                        //prefabSelector = prefabSelector % spawnablePrefabs.Length;
                        break;
                    }
            }

            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Ground")))
            {
                if (!currentSpawnedPrefab)
                {
                    SpawnPrefab();
                }
                else if (OldprefabSelector != prefabSelector)
                {
                    DestroyImmediate(currentSpawnedPrefab);
                    SpawnPrefab();
                }
                else
                {
                    Vector3 hitPos = hit.transform.position;
                    hitPos.y ++;
                    currentSpawnedPrefab.transform.position = hitPos;
                    if (e.type == EventType.MouseUp && e.button == 1)
                    {
                        currentSpawnedPrefab = null;
                    }
                }
            }
        }
    }

    static void SpawnPrefab()
    {
        currentSpawnedPrefab = Instantiate(spawnablePrefabs[prefabSelector]) as GameObject;
        Selection.activeGameObject = currentSpawnedPrefab;
        OldprefabSelector = prefabSelector;
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

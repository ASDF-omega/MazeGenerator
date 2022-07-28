using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class MazeParent1 : MonoBehaviour
{
    private GameObject maze;
    private GameObject wallParent;
    private GameObject floorParent;
    private bool isStartMethodCalled = false;
    private bool isCombined = false;
    private bool isSaved;
    private MazeGenerator mazeGenerator;

    public bool isdoneGenerating = false;

    [SerializeField] private Material floorMaterial;
    [SerializeField] private Material wallMaterial;

    // Start is called before the first frame update
    void Start()
    {
        mazeGenerator = GameObject.FindGameObjectWithTag("MazeGenerator").GetComponent<MazeGenerator>();
        isStartMethodCalled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Mazes"))
        {
            AssetDatabase.CreateFolder("Assets", "Mazes");
        }

        if (!AssetDatabase.IsValidFolder("Assets/Meshes"))
        {
            AssetDatabase.CreateFolder("Assets", "Meshes");
        }

        if(isCombined && floorParent.GetComponent<MeshFilter>().sharedMesh == null || isCombined && wallParent.GetComponent<MeshFilter>().sharedMesh == null)
        {
            DestroyImmediate(gameObject);
        }
    }

    public void combineMaze()
    {
        if(!isCombined)
        {
            floorParent = new GameObject();
            floorParent.AddComponent<MeshFilter>();
            floorParent.AddComponent<MeshRenderer>();
            floorParent.GetComponent<MeshRenderer>().material = floorMaterial;

            wallParent = new GameObject();
            wallParent.AddComponent<MeshFilter>();
            wallParent.AddComponent<MeshRenderer>();
            wallParent.GetComponent<MeshRenderer>().material = wallMaterial;

            #region floor merge
        combineFloors();
        #endregion
            #region wall merge
        combineWalls();
        #endregion

            isCombined = true;
        }
    }

    void combineFloors()
    {
        floorParent.name = "Floors";
        floorParent.transform.parent = this.transform;

        MeshFilter[] floorMeshFilters = GetComponentsInChildrenWithTag<MeshFilter>(floorParent.transform, "Floor");

        CombineInstance[] floorCombineInstance = new CombineInstance[floorMeshFilters.Length];

        int i = 0;
        while (i < floorMeshFilters.Length)
        {
            floorCombineInstance[i].mesh = floorMeshFilters[i].sharedMesh;
            floorCombineInstance[i].transform = floorMeshFilters[i].transform.localToWorldMatrix;
            DestroyImmediate(floorMeshFilters[i].gameObject);
      /*      floorMeshFilters[i].gameObject.SetActive(false);
            floorMeshFilters[i].transform.parent = floorParent.transform; */
            i++;
        }

        floorParent.transform.GetComponent<MeshFilter>().sharedMesh = new Mesh();
        floorParent.GetComponent<MeshFilter>().sharedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        floorParent.transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(floorCombineInstance);
        floorParent.transform.gameObject.SetActive(true);
    }

    void combineWalls()
    {
        wallParent.name = "Walls";
        wallParent.transform.parent = this.transform;

        MeshFilter[] wallMeshFilters = GetComponentsInChildrenWithTag<MeshFilter>(wallParent.transform, "Wall");
        CombineInstance[] wallCombineInstance = new CombineInstance[wallMeshFilters.Length];

        int j = 0;
        while (j < wallMeshFilters.Length)
        {
            wallCombineInstance[j].mesh = wallMeshFilters[j].sharedMesh;
            wallCombineInstance[j].transform = wallMeshFilters[j].transform.localToWorldMatrix;
            DestroyImmediate(wallMeshFilters[j].gameObject);
    /*        wallMeshFilters[j].gameObject.SetActive(false);
            wallMeshFilters[j].transform.parent = wallParent.transform; */
            j++;
        }

        wallParent.transform.GetComponent<MeshFilter>().sharedMesh = new Mesh();
        wallParent.GetComponent<MeshFilter>().sharedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        wallParent.transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(wallCombineInstance);
        wallParent.transform.gameObject.SetActive(true);
    }

    public void saveMaze()
    {
        if (isCombined)
        {
            #region save floor Mesh
            string floorMeshPath = "Assets/Meshes/" + floorParent.name + ".asset";
            floorMeshPath = AssetDatabase.GenerateUniqueAssetPath(floorMeshPath);
            AssetDatabase.CreateAsset(floorParent.GetComponent<MeshFilter>().sharedMesh, floorMeshPath);
            var floorMesh = AssetDatabase.LoadMainAssetAtPath(floorMeshPath);
            floorParent.GetComponent<MeshFilter>().sharedMesh = EditorUtility.InstanceIDToObject(floorMesh.GetInstanceID()) as Mesh;
            #endregion

            #region save wall Mesh
            string wallMeshPath = "Assets/Meshes/" + wallParent.name + ".asset";
            wallMeshPath = AssetDatabase.GenerateUniqueAssetPath(wallMeshPath);
            AssetDatabase.CreateAsset(wallParent.GetComponent<MeshFilter>().sharedMesh, wallMeshPath);
            var wallMesh = AssetDatabase.LoadMainAssetAtPath(wallMeshPath);
            wallParent.GetComponent<MeshFilter>().sharedMesh = EditorUtility.InstanceIDToObject(wallMesh.GetInstanceID()) as Mesh;
            #endregion
        }
        else if (EditorUtility.DisplayDialog("Are you sure?", "You won't be able to combine the meshes after saving", "yes", "no"))
        {     
            #region save maze prefab
            string savePath = "Assets/Mazes/" + gameObject.name + ".prefab";
            savePath = AssetDatabase.GenerateUniqueAssetPath(savePath);
            maze = PrefabUtility.SaveAsPrefabAsset(this.gameObject, savePath, out bool outbool);
            #endregion

            isSaved = true;
            GameObject newMaze = (GameObject)PrefabUtility.InstantiatePrefab(EditorUtility.InstanceIDToObject(maze.GetInstanceID()));
            mazeGenerator.mazeParent = newMaze;
            newMaze.gameObject.name = "Maze";
            DestroyImmediate(gameObject);
        }
    }

    public static T[] GetComponentsInChildrenWithTag<T>(Transform parent, string Tag) where T : Component
    {
        GameObject[] allGameObjectsWithTag = GameObject.FindGameObjectsWithTag(Tag);
        List<T> allComponents = new List<T>();

        for (int i = 0; i < allGameObjectsWithTag.Length; i++)
        {
            if (allGameObjectsWithTag[i].transform.parent = parent)
            {
                allComponents.Add(allGameObjectsWithTag[i].GetComponent<T>());
            }
        }

        return allComponents.ToArray();
    }

    public static GameObject[] findChildrenWithTag(Transform parent, string tag)
    {
        GameObject[] allGameObjectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        List<GameObject> childrenWithTag = new List<GameObject>();

        for (int i = 0; i < allGameObjectsWithTag.Length; i++)
        {
            if (allGameObjectsWithTag[i].transform.parent == parent)
            {
                childrenWithTag.Add(allGameObjectsWithTag[i]);
            }
        }

        return childrenWithTag.ToArray();
    }
}

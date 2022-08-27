using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class MazeParent : MonoBehaviour
{
    private GameObject maze;
    private GameObject wallParent;
    private GameObject floorParent;

    private delegate void CombineAsTwoMesh();
    private CombineAsTwoMesh combineAsTwoMesh;

    private bool isCombined = false;

    public bool isdoneGenerating = false;

    [SerializeField] private Material floorMaterial;
    [SerializeField] private Material wallMaterial;

    private MazeGenerator mazeGenerator;

    private void Awake()
    {
        mazeGenerator = GameObject.FindGameObjectWithTag("MazeGenerator").GetComponent<MazeGenerator>();
        combineAsTwoMesh += combineFloors;
        combineAsTwoMesh += combineWalls;
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
    }

    public void combineMaze()
    {
        if(!isCombined)
        {
            if(mazeGenerator.CombineAs == MazeGenerator.CombineOptions.FloorMeshAndWallMesh)
            {
                #region combine as two mesh
                floorParent = new GameObject();
                floorParent.AddComponent<MeshFilter>();
                floorParent.AddComponent<MeshRenderer>();
                floorParent.GetComponent<MeshRenderer>().material = floorMaterial;
                floorParent.tag = ("FloorParent");

                wallParent = new GameObject();
                wallParent.AddComponent<MeshFilter>();
                wallParent.AddComponent<MeshRenderer>();
                wallParent.GetComponent<MeshRenderer>().material = wallMaterial;
                wallParent.tag = ("WallParent");

                combineAsTwoMesh();
                #endregion
            }
            else if(mazeGenerator.CombineAs == MazeGenerator.CombineOptions.SingleMesh)
            {
                #region combine as two mesh first
                floorParent = new GameObject();
                floorParent.AddComponent<MeshFilter>();
                floorParent.AddComponent<MeshRenderer>();
                floorParent.GetComponent<MeshRenderer>().material = floorMaterial;
                floorParent.tag = ("FloorParent");

                wallParent = new GameObject();
                wallParent.AddComponent<MeshFilter>();
                wallParent.AddComponent<MeshRenderer>();
                wallParent.GetComponent<MeshRenderer>().material = wallMaterial;
                wallParent.tag = ("WallParent");

                combineAsTwoMesh();
                #endregion

                #region combine into one mesh

                gameObject.AddComponent<MeshFilter>();
                gameObject.AddComponent<MeshRenderer>();

                List<MeshFilter> filters = new List<MeshFilter>();
                List<CombineInstance> finalCombine = new List<CombineInstance>();
                Material[] materialArray = { floorMaterial, wallMaterial};
                filters.Add(floorParent.GetComponent<MeshFilter>());
                filters.Add(wallParent.GetComponent<MeshFilter>());

                for(int i = 0; i < filters.Count; i++)
                {
                    CombineInstance ci = new CombineInstance();

                    ci.mesh = filters[i].sharedMesh;
                    ci.subMeshIndex = 0;
                    ci.transform = filters[i].transform.localToWorldMatrix;

                    finalCombine.Add(ci);
                }

                GetComponent<MeshFilter>().mesh = new Mesh();
                GetComponent<MeshFilter>().sharedMesh.CombineMeshes(finalCombine.ToArray(), false, false);
                GetComponent<MeshRenderer>().sharedMaterials = materialArray;
                GetComponent<MeshFilter>().sharedMesh.name = "Maze";
                DestroyImmediate(floorParent.gameObject);
                DestroyImmediate(wallParent.gameObject);

                #endregion
            }

            isCombined = true;
        }
        else
        {
            EditorUtility.DisplayDialog("Error!", "Maze is already combined", "ok");
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

            if(mazeGenerator.__ == MazeGenerator.CombinedMeshes.Destroy)
            {
                DestroyImmediate(floorMeshFilters[i].gameObject);
            }
            else if(mazeGenerator.__ == MazeGenerator.CombinedMeshes.Disable)
            {
                floorMeshFilters[i].gameObject.SetActive(false);
                floorMeshFilters[i].transform.parent = floorParent.transform;
            }
            i++;
        }

        floorParent.transform.GetComponent<MeshFilter>().sharedMesh = new Mesh();
        floorParent.GetComponent<MeshFilter>().sharedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        floorParent.transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(floorCombineInstance);
        floorParent.GetComponent<MeshFilter>().sharedMesh.name = "Floor";
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

            if (mazeGenerator.__ == MazeGenerator.CombinedMeshes.Destroy)
            {
                DestroyImmediate(wallMeshFilters[j].gameObject);
            }
            else if (mazeGenerator.__ == MazeGenerator.CombinedMeshes.Disable)
            {
                wallMeshFilters[j].gameObject.SetActive(false);
                wallMeshFilters[j].transform.parent = wallParent.transform;
            }

            j++;
        }

        wallParent.transform.GetComponent<MeshFilter>().sharedMesh = new Mesh();
        wallParent.GetComponent<MeshFilter>().sharedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        wallParent.transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(wallCombineInstance);
        wallParent.GetComponent<MeshFilter>().sharedMesh.name = "Wall";
        wallParent.transform.gameObject.SetActive(true);
    }

    public void saveMaze()
    {
        if (isCombined)
        {
            if(mazeGenerator.CombineAs == MazeGenerator.CombineOptions.FloorMeshAndWallMesh)
            {
                #region save floor Mesh
                string floorMeshPath = "Assets/Meshes/" + floorParent.name + ".asset";
                floorMeshPath = AssetDatabase.GenerateUniqueAssetPath(floorMeshPath);
                var floorMesh = floorParent.GetComponent<MeshFilter>().sharedMesh;
                AssetDatabase.CreateAsset(floorMesh, floorMeshPath);
                #endregion

                #region save wall Mesh
                string wallMeshPath = "Assets/Meshes/" + wallParent.name + ".asset";
                wallMeshPath = AssetDatabase.GenerateUniqueAssetPath(wallMeshPath);
                var wallMesh = wallParent.GetComponent<MeshFilter>().sharedMesh;
                AssetDatabase.CreateAsset(wallMesh, wallMeshPath);
                #endregion
            }
            else if(mazeGenerator.CombineAs == MazeGenerator.CombineOptions.SingleMesh)
            {
                #region save maze Mesh
                string saveMazePath = "Assets/Meshes/" + gameObject.name + ".asset";
                saveMazePath = AssetDatabase.GenerateUniqueAssetPath(saveMazePath);
                var mazeMesh = GetComponent<MeshFilter>().sharedMesh;
                AssetDatabase.CreateAsset(mazeMesh, saveMazePath);
                #endregion
            }

            #region save maze prefab
            string savePath = "Assets/Mazes/" + gameObject.name + ".prefab";
            savePath = AssetDatabase.GenerateUniqueAssetPath(savePath);
            maze = PrefabUtility.SaveAsPrefabAsset(this.gameObject, savePath, out bool outbool);
            
            if(mazeGenerator.CombineAs == MazeGenerator.CombineOptions.FloorMeshAndWallMesh)
            {
                floorParent.GetComponent<MeshFilter>().sharedMesh = Instantiate(floorParent.GetComponent<MeshFilter>().sharedMesh);
                wallParent.GetComponent<MeshFilter>().sharedMesh = Instantiate(wallParent.GetComponent<MeshFilter>().sharedMesh);
            }
            else if(mazeGenerator.CombineAs == MazeGenerator.CombineOptions.SingleMesh)
            {
                GetComponent<MeshFilter>().sharedMesh = Instantiate(GetComponent<MeshFilter>().sharedMesh);
            }

            #endregion

            return;
        }
        
        if (!isCombined && EditorUtility.DisplayDialog("Are you sure?", "You won't be able to combine the sharedMeshes after saving", "yes", "no"))
        {     
            #region save maze prefab
            string savePath = "Assets/Mazes/" + gameObject.name + ".prefab";
            savePath = AssetDatabase.GenerateUniqueAssetPath(savePath);
            maze = PrefabUtility.SaveAsPrefabAsset(this.gameObject, savePath, out bool outbool);
            #endregion

            return;
        }
    }

    #region finding things with tag methods
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
    #endregion
}

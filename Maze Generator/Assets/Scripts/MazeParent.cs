using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MazeParent : MonoBehaviour
{
    private GameObject maze;
    private GameObject wallParent;
    private GameObject floorParent;
    private bool isStartMethodCalled = false;
    private bool isCombined = false;

    public bool isdoneGenerating = false;

    [SerializeField] private Material floorMaterial;
    [SerializeField] private Material wallMaterial;

    // Start is called before the first frame update
    void Start()
    {
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

        if(isdoneGenerating && isStartMethodCalled && !isCombined)
        {
            combineMaze();
        }
    }

    public void combineMaze()
    {
        #region floor merge
        combineFloors();
        #endregion

        #region wall merge
        combineWalls();
        #endregion

        isCombined = true;
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
            Destroy(floorMeshFilters[i].gameObject);
      /*      floorMeshFilters[i].gameObject.SetActive(false);
            floorMeshFilters[i].transform.parent = floorParent.transform; */
            i++;
        }

        floorParent.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        floorParent.GetComponent<MeshFilter>().mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        floorParent.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(floorCombineInstance);
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
            Destroy(wallMeshFilters[j].gameObject);
    /*        wallMeshFilters[j].gameObject.SetActive(false);
            wallMeshFilters[j].transform.parent = wallParent.transform; */
            j++;
        }

        wallParent.transform.GetComponent<MeshFilter>().mesh = new Mesh();
        wallParent.GetComponent<MeshFilter>().mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        wallParent.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(wallCombineInstance);
        wallParent.transform.gameObject.SetActive(true);
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

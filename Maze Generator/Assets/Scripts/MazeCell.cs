using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public GameObject northwall;
    public GameObject eastwall;
    public GameObject southwall;
    public GameObject westwall;
    public MazeCell northcell;
    public MazeCell eastcell;
    public MazeCell southcell;
    public MazeCell westcell;
    public MazeCell nextcell;
    public bool isVisited = false;
}

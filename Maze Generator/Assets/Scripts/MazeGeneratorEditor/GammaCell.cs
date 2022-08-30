using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GammaCell : MonoBehaviour
{
    public GameObject northwall;
    public GameObject eastwall;
    public GameObject southwall;
    public GameObject westwall;
    public GammaCell northcell;
    public GammaCell eastcell;
    public GammaCell southcell;
    public GammaCell westcell;
    public GammaCell nextcell;
    public GammaCell previouscell;
    public int RowIndex;
    public int ColumnIndex;
    public int Rows;
    public int Columns;
    public int links;
    public bool isEnabled = true;
    public bool isVisited = false;

    public GameObject[] AvailableWalls()
    {
        List<GameObject> availableWalls = new List<GameObject>();

        if (RowIndex > 0)
        {
            if(northwall != null)
            {
                availableWalls.Add(northwall);
            }
        }

        if (ColumnIndex < Columns - 1)
        {
            if(eastwall != null)
            {
                availableWalls.Add(eastwall);
            }
        }

        if (RowIndex < Rows - 1)
        {
            if(southwall != null)
            {
                availableWalls.Add(southwall);
            }
        }

        if (ColumnIndex > 0)
        {
            if (westwall != null)
            {
                availableWalls.Add(westwall);
            }
         }

        return availableWalls.ToArray();
    }
}

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
    public GammaCell pathFindingParentCell;
    public int RowIndex;
    public int ColumnIndex;
    public int Rows;
    public int Columns;
    public int links;
    public int gCost;//distance from start cell
    public int hCost;//distance to end cell
    public int fCost;//sum ofo gCost and hCost
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

    public GammaCell[] VisitableCells()
    {
        List<GammaCell> visitableCells = new List<GammaCell>();

        if (RowIndex > 0)
        {
            if (northwall == null)
            {
                visitableCells.Add(northcell);
            }
        }

        if (ColumnIndex < Columns - 1)
        {
            if (eastwall == null)
            {
                visitableCells.Add(eastcell);
            }
        }

        if (RowIndex < Rows - 1)
        {
            if (southwall == null)
            {
                visitableCells.Add(southcell);
            }
        }

        if (ColumnIndex > 0)
        {
            if (westwall == null)
            {
                visitableCells.Add(westcell);
            }
        }

        return visitableCells.ToArray();
    }
}

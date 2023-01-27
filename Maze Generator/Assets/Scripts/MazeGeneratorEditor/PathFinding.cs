using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public GammaCell StartCell;
    public GammaCell EndCell;

    private List<GammaCell> Open;
    private List<GammaCell> Close;
    private GammaCell currentCell;

    public void FindPath()
    {
        Open = new List<GammaCell>();
        Close = new List<GammaCell>();

        Open.Add(StartCell);

        

        Close.Add(currentCell);
    }

    private void EvaluateAdjacentCells()
    {
        for (int i = 0; i < currentCell.VisitableCells().Length; i++)
        {

        }
    }

    private GammaCell CellWithLowestFCost()
    {
        GammaCell cell = new GammaCell();
        return cell;
    }
}

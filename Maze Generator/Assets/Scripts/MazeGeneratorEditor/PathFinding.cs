using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MazeAlgorithm
{
    public GammaCell StartCell;
    public GammaCell EndCell;

    private List<GammaCell> Open;//List of cells to be evaluated
    private List<GammaCell> Closed;//List of cells already evaluated

    public void FindPath()
    {
        if(StartCell == null)
        {
            StartCell = maze[0, 0];
        }

        if(EndCell == null)
        {
            EndCell = maze[rows-1, columns-1];
        }

        Open = new List<GammaCell>();
        Closed = new List<GammaCell>();
        currentCell = StartCell;
        Open.Add(StartCell);
        for (int i = 0; i < mazeGenerator.Rows; i++)
        {
            for (int j = 0; j < mazeGenerator.Columns; j++)
            {
                maze[i, j].isVisited = false;
                maze[i, j].nextcell = null;
                
                if(maze[i, j] != initialCell)
                {
                    maze[i, j].previouscell = null;
                }

                if(maze[i, j] != finalCell)
                {
                    maze[i, j].nextcell = null;
                }
            }
        }

        while(currentCell != EndCell)
        {
            CalculateCosts();
            CellToVisit().previouscell = currentCell;
            currentCell = CellToVisit();
            Closed.Add(currentCell);
            Open.Remove(currentCell);

            for (int i = 0; i < adjacentVisitableCellsOf(currentCell).Length; i++)
            {
                if(!Open.Contains(adjacentVisitableCellsOf(currentCell)[i]))
                {
                    Open.Add(adjacentVisitableCellsOf(currentCell)[i]);
                }
            }
        }

    }

    private void CalculateCosts()
    {
        for (int i = 0; i < mazeGenerator.Rows; i++)
        {
            for (int j = 0; j < mazeGenerator.Columns; j++)
            {


                maze[i, j].fCost = maze[i, j].gCost + maze[i, j].hCost;
            }
        }
    }

    private GammaCell CellToVisit()
    {
        GammaCell cell;

        if (CellsWithLowest_fCost().Length > 1)
        {
            cell = CellsWithLowest_fCost()[Random.Range(0, CellsWithLowest_fCost().Length)];
        }
        else
        {
            Debug.Log(CellsWithLowest_fCost().Length);
            cell = CellsWithLowest_fCost()[0];
        }

        return cell;
    }

    private GammaCell[] CellsWithLowest_fCost()//bug in this function, it returns a list with nothing in it
    {
        List<GammaCell> cells_WithLowest_fCost = new List<GammaCell>();
        List<GammaCell> cells_WithLowest_hCost_inCellsWithLowest_fCost = new List<GammaCell>();
        int fCost = new int();
        int hCost = new int();

        for (int i = 0; i < Open.Count; i++)
        {
            fCost = Open[i].fCost;
            
            if(Open[i].fCost < fCost)
            {
                fCost = Open[i].fCost;
            }
        }

        for (int i = 0; i < Open.Count; i++)
        {
            if(Open[i].fCost == fCost)
            {
                cells_WithLowest_fCost.Add(Open[i]);
            }
        }

        for (int i = 0; i < cells_WithLowest_fCost.Count; i++)
        {
            hCost = cells_WithLowest_fCost[i].hCost;

            if (cells_WithLowest_fCost[i].hCost < hCost)
            {
                hCost = cells_WithLowest_fCost[i].hCost;
            }
        }

        for (int i = 0; i < cells_WithLowest_fCost.Count; i++)
        {
            if (cells_WithLowest_fCost[i].hCost == hCost)
            {
                cells_WithLowest_hCost_inCellsWithLowest_fCost.Add(cells_WithLowest_fCost[i]);
            }
        }

        return cells_WithLowest_hCost_inCellsWithLowest_fCost.ToArray();
    }


    private void EvaluateAdjacentCells()
    {
        for (int i = 0; i < currentCell.VisitableCells().Length; i++)
        {

        }
    }
}

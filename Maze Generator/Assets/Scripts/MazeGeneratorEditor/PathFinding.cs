using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MazeAlgorithm
{
    public GammaCell StartCell;
    public GammaCell EndCell;

    private List<GammaCell> Open;//List of cells to be evaluated
    private List<GammaCell> Closed;//List of cells already evaluated
    private Vector2 startCellPosition;
    private Vector2 endCellPosition;

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

        int loop = 0;
        while(currentCell != EndCell)
        {
            ++loop;

            if(loop > 10000)
            {
                Debug.Log(loop);
                return;
            }

            CalculateCosts();
            CellToVisit().previouscell = currentCell;
            Closed.Add(currentCell);
            Open.Remove(currentCell);
            currentCell = CellToVisit();

            for (int i = 0; i < adjacentVisitableCellsOf(currentCell).Length; i++)
            {
                if(!Open.Contains(adjacentVisitableCellsOf(currentCell)[i]))
                {
                    Open.Add(adjacentVisitableCellsOf(currentCell)[i]);
                }
            }
        }

        GammaCell previousCell = EndCell.previouscell;
        List<GammaCell> correctPath = new List<GammaCell>();

        while(previousCell != StartCell)
        {
            correctPath.Add(previousCell);
            previousCell.GetComponent<MeshRenderer>().material.color = Color.green;
            previousCell = previousCell.previouscell;
        }
    }

    private void CalculateCosts()
    {
        for (int i = 0; i < adjacentVisitableCellsOf(currentCell).Length; i++)
        {
            GammaCell cell = adjacentVisitableCellsOf(currentCell)[i];
            
            if(!Closed.Contains(cell))
            {
                cell.gCost = WalkingDistanceBetweenCells(startCellPosition, new Vector2(cell.RowIndex, cell.ColumnIndex));
                cell.hCost = WalkingDistanceBetweenCells(endCellPosition, new Vector2(cell.RowIndex, cell.ColumnIndex));

                cell.fCost = cell.gCost + cell.hCost;
            }
        }
    }

    private int WalkingDistanceBetweenCells(Vector2 from, Vector2 to)
    {
        int distance;
        Vector2 directionVector = to - from;
        int diagonalDistance = Mathf.Abs((int)Mathf.Min(directionVector.x, directionVector.y) * 14);

        distance = diagonalDistance + Mathf.Abs(((int)Mathf.Max(directionVector.x, directionVector.y)-(int)Mathf.Min(directionVector.x, directionVector.y))*10);

        return distance;
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
            fCost = adjacentVisitableCellsOf(currentCell)[i].fCost;
            
            if(Open[i].fCost < fCost)
            {
                fCost = adjacentVisitableCellsOf(currentCell)[i].fCost;
            }
        }

        for (int i = 0; i < Open.Count; i++)
        {
            if(Open[i].fCost == fCost)
            {
                cells_WithLowest_fCost.Add(adjacentVisitableCellsOf(currentCell)[i]);
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

        for (int i = 0; i < cells_WithLowest_hCost_inCellsWithLowest_fCost.Count; i++)
        {
            if(Closed.Contains(cells_WithLowest_hCost_inCellsWithLowest_fCost[i]))
            {
                cells_WithLowest_hCost_inCellsWithLowest_fCost.Remove(cells_WithLowest_hCost_inCellsWithLowest_fCost[i]);
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

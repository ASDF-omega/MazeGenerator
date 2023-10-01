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

    //bug: previous of cells are wrong, e.g. when retraced, some are leading to dead ends, sometimes previous cell is null
    public void FindPath()
    {
        if (StartCell == null)
        {
            StartCell = maze[0, 0];
        }

        if (EndCell == null)
        {
            EndCell = maze[rows - 1, columns - 1];
        }

        startCellPosition = new Vector2(StartCell.RowIndex, StartCell.ColumnIndex);
        endCellPosition = new Vector2(EndCell.RowIndex, EndCell.ColumnIndex);

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

                if (maze[i, j] != initialCell)
                {
                    maze[i, j].previouscell = null;
                }

                if (maze[i, j] != finalCell)
                {
                    maze[i, j].nextcell = null;
                }
            }
        }

        List<GammaCell> correctPath = new List<GammaCell>();

        while (currentCell != EndCell)
        {
            CalculateCosts();
/*            CellToVisit().previouscell = currentCell;*/
            currentCell = CellToVisit();

            Closed.Add(currentCell);
            Open.Remove(currentCell);

            for (int i = 0; i < adjacentVisitableCellsOf(currentCell).Length; i++)
            {
                if (!Open.Contains(adjacentVisitableCellsOf(currentCell)[i]) && !Closed.Contains(adjacentVisitableCellsOf(currentCell)[i]))
                {
                    Open.Add(adjacentVisitableCellsOf(currentCell)[i]);
                    adjacentVisitableCellsOf(currentCell)[i].previouscell = currentCell;
                }
            }
        }

        GammaCell previousCell = EndCell.previouscell;

        var tempMaterial1 = new Material(StartCell.GetComponent<Renderer>().sharedMaterial);
        tempMaterial1.color = Color.red;
        StartCell.GetComponent<Renderer>().sharedMaterial = tempMaterial1;
        EndCell.GetComponent<Renderer>().sharedMaterial = tempMaterial1;

        while (previousCell != StartCell)
        {
            correctPath.Add(previousCell);
            var tempMaterial = new Material(previousCell.GetComponent<Renderer>().sharedMaterial);
            tempMaterial.color = Color.green;
            previousCell.GetComponent<Renderer>().sharedMaterial = tempMaterial;
            previousCell = previousCell.previouscell;
        }
    }

    private void CalculateCosts()
    {
        for (int i = 0; i < Open.Count; i++)
        {
            GammaCell cell = Open[i];
            if (!Closed.Contains(cell))
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

        distance = diagonalDistance + Mathf.Abs(((int)Mathf.Max(directionVector.x, directionVector.y) - (int)Mathf.Min(directionVector.x, directionVector.y)) * 10);

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

    private GammaCell[] CellsWithLowest_fCost()
    {
        List<GammaCell> cells_WithLowest_fCost = new List<GammaCell>();
        List<GammaCell> cells_WithLowest_hCost_inCellsWithLowest_fCost = new List<GammaCell>();
        int fCost = new int();
        int hCost = new int();

        for (int i = 0; i < Open.Count; i++)
        {
            fCost = Open[i].fCost;

            if (Open[i].fCost < fCost)
            {
                fCost = Open[i].fCost;
            }
        }

        for (int i = 0; i < Open.Count; i++)
        {
            if (Open[i].fCost == fCost)
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
}
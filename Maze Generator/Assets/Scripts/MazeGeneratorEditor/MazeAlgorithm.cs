using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeAlgorithm : MonoBehaviour
{
    public GammaCell[,] maze;
    public int rows;
    public int columns;
    public bool isfinished;

    protected int currentRow = 0;
    protected int currentColumn = 0;
    protected GammaCell currentCell;
    protected GammaCell initialCell;
    protected GammaCell finalCell;
    protected bool isEditor = false;

    [SerializeField] protected MazeGenerator mazeGenerator;

    public virtual void CreateMaze()
    {
        isfinished = false;
        rows = mazeGenerator.Rows;
        columns = mazeGenerator.Columns;

        switch(mazeGenerator.tessellation)
        {
            case MazeGenerator.CellType.Gamma:
                mazeGenerator.InstantiateGammaMaze(rows, columns);
                break;
            case MazeGenerator.CellType.Delta:
                break;
            case MazeGenerator.CellType.Sigma:
                break;
            case MazeGenerator.CellType.Theata:
                break;
        }

        currentRow = Random.Range(0, rows);
        currentColumn = Random.Range(0, columns);
        currentCell = maze[currentRow, currentColumn];
        initialCell = currentCell;
        Debug.Log("StartCell: " + initialCell, initialCell);
        currentCell.isVisited = true;
    }

    protected bool hasAdjacentUnvisitedCells()
    {
        if (currentRow > 0 && maze[currentRow - 1, currentColumn].isVisited == false || currentColumn < columns - 1 && maze[currentRow, currentColumn + 1].isVisited == false || currentRow < rows - 1 && maze[currentRow + 1, currentColumn].isVisited == false || currentColumn > 0 && maze[currentRow, currentColumn - 1].isVisited == false)
        {
            return true;
        }

        return false;
    }

    protected void visit()
    {
        int direction = Random.Range(0, 4);

        if (direction == 0)
        {
            if (currentRow > 0 && !currentCell.northcell.isVisited)
            {
                DestroyImmediate(currentCell.northwall);
                --currentRow;
                currentCell.northcell.isVisited = true;
                currentCell.nextcell = currentCell.northcell;
                currentCell.nextcell.previouscell = currentCell;
                currentCell = currentCell.northcell;
                Debug.Log(currentCell, currentCell);
                return;
            }
        }

        if (direction == 1)
        {
            if (currentColumn < columns - 1 && !currentCell.eastcell.isVisited)
            {
                DestroyImmediate(currentCell.eastwall);
                ++currentColumn;
                currentCell.eastcell.isVisited = true;
                currentCell.nextcell = currentCell.eastcell;
                currentCell.nextcell.previouscell = currentCell;
                currentCell = currentCell.eastcell;
                Debug.Log(currentCell, currentCell);
                return;
            }
        }

        if (direction == 2)
        {
            if (currentRow < rows - 1 && !currentCell.southcell.isVisited)
            {
                DestroyImmediate(currentCell.southwall);
                ++currentRow;
                currentCell.southcell.isVisited = true;
                currentCell.nextcell = currentCell.southcell;
                currentCell.nextcell.previouscell = currentCell;
                currentCell = currentCell.southcell;
                Debug.Log(currentCell, currentCell);
                return;
            }
        }

        if (direction == 3)
        {
            if (currentColumn > 0 && !currentCell.westcell.isVisited)
            {
                DestroyImmediate(currentCell.westwall);
                --currentColumn;
                currentCell.westcell.isVisited = true;
                currentCell.nextcell = currentCell.westcell;
                currentCell.nextcell.previouscell = currentCell;
                currentCell = currentCell.westcell;
                Debug.Log(currentCell, currentCell);
                return;
            }
        }
    }

    public GammaCell[] adjacentUnvisitedCellsOf(GammaCell cell)
    {
        List<GammaCell> adjacentUnvisitedCellsList = new List<GammaCell>();

        if(cell.RowIndex > 0)
        {
            if (!cell.northcell.isVisited)
            {
                adjacentUnvisitedCellsList.Add(cell.northcell);
            }
        }

        if(cell.ColumnIndex < columns - 1)
        {
            if (!cell.eastcell.isVisited)
            {
                adjacentUnvisitedCellsList.Add(cell.eastcell);
            }
        }

        if(cell.RowIndex < rows - 1)
        {
            if (!cell.southcell.isVisited)
            {
                adjacentUnvisitedCellsList.Add(cell.southcell);
            }
        }

        if(cell.ColumnIndex > 0)
        {
            if (!cell.westcell.isVisited)
            {
                adjacentUnvisitedCellsList.Add(cell.westcell);
            }
        }

        return adjacentUnvisitedCellsList.ToArray();
    }

    public GammaCell[] adjacentVisitedCellsOf(GammaCell cell)
    {
        List<GammaCell> adjacentVisitedCells = new List<GammaCell>();

        if (cell.RowIndex > 0)
        {
            if (cell.northcell.isVisited)
            {
                adjacentVisitedCells.Add(cell.northcell);
            }
        }

        if (cell.ColumnIndex < columns - 1)
        {
            if (cell.eastcell.isVisited)
            {
                adjacentVisitedCells.Add(cell.eastcell);
            }
        }

        if (cell.RowIndex < rows - 1)
        {
            if (cell.southcell.isVisited)
            {
                adjacentVisitedCells.Add(cell.southcell);
            }
        }

        if (cell.ColumnIndex > 0)
        {
            if (cell.westcell.isVisited)
            {
                adjacentVisitedCells.Add(cell.westcell);
            }
        }

        return adjacentVisitedCells.ToArray();
    }

    public GammaCell[] adjacentVisitableCellsOf(GammaCell cell)
    {
        List<GammaCell> adjacentVisitableCells = new List<GammaCell>();

        if(cell.northwall == null)
        {
            adjacentVisitableCells.Add(cell.northcell);
        }

        if(cell.eastwall == null)
        {
            adjacentVisitableCells.Add(cell.eastcell);
        }

        if (cell.southwall == null)
        {
            adjacentVisitableCells.Add(cell.southcell);
        }

        if (cell.westwall == null)
        {
            adjacentVisitableCells.Add(cell.westcell);
        }

        return adjacentVisitableCells.ToArray();
    }
}

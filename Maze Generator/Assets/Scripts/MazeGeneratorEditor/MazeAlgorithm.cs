using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeAlgorithm : MonoBehaviour
{
    public MazeCell[,] maze;
    public int rows;
    public int columns;
    public bool isfinished;

    protected int currentRow = 0;
    protected int currentColumn = 0;
    protected MazeCell currentCell;
    protected MazeCell initialCell;
    protected bool isEditor = false;

    [SerializeField] protected MazeGenerator mazeGenerator;

    public virtual void CreateMaze()
    {
        isfinished = false;
        DestroyImmediate(GameObject.FindGameObjectWithTag("Maze"));
        mazeGenerator.InstantiateMaze(rows, columns);
        currentRow = Random.Range(0, rows);
        currentColumn = Random.Range(0, columns);
        currentCell = maze[currentRow, currentColumn];
        initialCell = currentCell;
        currentCell.isVisited = true;
    }

    protected bool hasAdjacentUnvisitedCells()
    {
        if (currentRow > 0 && maze[currentRow - 1, currentColumn].isVisited == false)
        {
            return true;
        }

        if (currentColumn < columns - 1 && maze[currentRow, currentColumn + 1].isVisited == false)
        {
            return true;
        }

        if (currentRow < rows - 1 && maze[currentRow + 1, currentColumn].isVisited == false)
        {
            return true;
        }

        if (currentColumn > 0 && maze[currentRow, currentColumn - 1].isVisited == false)
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
                return;
            }
        }
    }

    public MazeCell[] adjacentUnvisitedCellsOf(MazeCell cell)
    {
        List<MazeCell> adjacentUnvisitedCellsList = new List<MazeCell>();

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

    public MazeCell[] adjacentVisitedCellsOf(MazeCell cell)
    {
        List<MazeCell> adjacentVisitedCells = new List<MazeCell>();

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
}

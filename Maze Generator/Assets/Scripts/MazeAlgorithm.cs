using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeAlgorithm : MonoBehaviour
{
    public MazeCell[,] maze;
    public int currentRow = 0;
    public int currentColumn = 0;
    public int rows;
    public int columns;
    public MazeCell currentCell;
    public bool isfinished;

    protected MazeCell previousCell;
    protected MazeCell initialCell;
    protected bool isEditor = false;

    [SerializeField] protected MazeLoader mazeLoader;

    public abstract void CreateMaze();
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
            }
        }
    }
}

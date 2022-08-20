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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntAndKillAlgorithm : MazeAlgorithm
{
    public override void CreateMaze()
    {
        Destroy(GameObject.FindGameObjectWithTag("Maze"));
        mazeLoader.InstantiateMaze(rows, columns);
        currentRow = 0;
        currentColumn = 0;
        currentCell = maze[currentRow, currentColumn];
        currentCell.isVisited = true;
        Kill();
        camera.transform.position = maze[Mathf.RoundToInt(rows / 2), Mathf.RoundToInt(columns / 2)].transform.position;
        GameObject.Destroy(GameObject.Find("New Game Object"));
    }

    public void Hunt()
    {
        isfinished = true;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                currentCell = maze[i, j];
                currentCell.isVisited = true;
                currentRow = i;
                currentColumn = j;

                if(hasAdjacentUnvisitedCells())
                {
                    isfinished = false;
                    Kill();
                    return;
                }
            }
        }

        if(isfinished)
        {
            mazeLoader.mazeParent.GetComponent<MazeParent>().isdoneGenerating = true;
            return;
        }
    }

    public void Kill()
    {
        while(hasAdjacentUnvisitedCells())
        {
            int direction = Random.Range(0, 4);

            if (direction == 0)
            {
                if(currentRow > 0 && !currentCell.northcell.isVisited)
                {
                    Destroy(currentCell.northwall);
                    --currentRow;
                    currentCell.northcell.isVisited = true;
                    currentCell = currentCell.northcell;
                }
            }

            if (direction == 1)
            {
                if(currentColumn < columns - 1 && !currentCell.eastcell.isVisited)
                {
                    Destroy(currentCell.eastwall);
                    ++currentColumn;
                    currentCell.eastcell.isVisited = true;
                    currentCell = currentCell.eastcell;
                }
            }

            if (direction == 2)
            {
                if(currentRow < rows - 1 && !currentCell.southcell.isVisited)
                {
                    Destroy(currentCell.southwall);
                    ++currentRow;
                    currentCell.southcell.isVisited = true;
                    currentCell = currentCell.southcell;
                }
            }

            if (direction == 3)
            {
                if(currentColumn > 0 && !currentCell.westcell.isVisited)
                {
                    Destroy(currentCell.westwall);
                    --currentColumn;
                    currentCell.westcell.isVisited = true;
                    currentCell = currentCell.westcell;
                }
            }
        }

        Hunt();
    }

    public bool hasAdjacentUnvisitedCells()
    {
        if(currentRow > 0 && maze[currentRow - 1, currentColumn].isVisited == false)
        {
            return true;
        }

        if(currentColumn < columns - 1 && maze[currentRow, currentColumn + 1].isVisited == false)
        {
            return true;
        }

        if(currentRow < rows - 1 && maze[currentRow + 1, currentColumn].isVisited == false)
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

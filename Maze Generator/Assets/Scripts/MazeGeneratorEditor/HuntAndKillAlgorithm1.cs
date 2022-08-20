using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HuntAndKillAlgorithm1 : MazeAlgorithm1
{
    public override void CreateMaze()
    {
        isfinished = false;
        DestroyImmediate(GameObject.FindGameObjectWithTag("Maze"));
        mazeGenerator.InstantiateMaze(rows, columns);
        currentRow = Random.Range(0, rows);
        currentColumn = Random.Range(0, columns);
        currentCell = maze[currentRow, currentColumn];
        initialCell = currentCell;
        currentCell.isVisited = true;
        Kill();
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
            mazeGenerator.mazeParent.GetComponent<MazeParent1>().isdoneGenerating = true;
            return;
        }
    }

    public void Kill()
    {
        while (hasAdjacentUnvisitedCells())
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
        Hunt();
    }
}

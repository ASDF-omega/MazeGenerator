using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntAndKillAlgorithm : MazeAlgorithm
{
    public override void CreateMaze()
    {
        base.CreateMaze();
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

                if (hasAdjacentUnvisitedCells())
                {
                    isfinished = false;
                    Kill();
                    return;
                }
            }
        }

        if (isfinished)
        {
            mazeLoader.mazeParent.GetComponent<MazeParent>().isdoneGenerating = true;
            return;
        }
    }

    public void Kill()
    {
        while (hasAdjacentUnvisitedCells())
        {
            visit();
        }

        Hunt();
    }
}

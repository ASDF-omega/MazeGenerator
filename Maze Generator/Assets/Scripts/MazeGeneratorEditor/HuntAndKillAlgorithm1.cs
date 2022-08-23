using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HuntAndKillAlgorithm1 : MazeAlgorithm1
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
            Debug.Log("<color=lime><B>Successfully generated a maze with \"HuntAndKillAlgorithm\"</B></color>");
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

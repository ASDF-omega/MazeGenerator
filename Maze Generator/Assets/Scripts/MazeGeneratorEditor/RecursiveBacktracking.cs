using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveBacktracking : MazeAlgorithm
{
    public override void CreateMaze()
    {
        base.CreateMaze();
        Generation();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                maze[i, j].isVisited = false;
            }
        }
        finalCell = currentCell;
    }

    private void Generation()
    {
        while (hasAdjacentUnvisitedCells())
        {
            visit();
        }

        backtrack();
    }

    private void backtrack()
    {
        while(!hasAdjacentUnvisitedCells())
        {
            currentCell = currentCell.previouscell;

            if (currentCell == initialCell)
            {
                isfinished = true;
                mazeGenerator.mazeParent.GetComponent<MazeParent>().isdoneGenerating = true;
                Debug.Log("<color=lime><B>Successfully generated a maze with \"RecursiveBacktrackingAlgorithm\"</B></color>");
                return;
            }

            currentRow = currentCell.RowIndex;
            currentColumn = currentCell.ColumnIndex;
        }

        Generation();
    }
}

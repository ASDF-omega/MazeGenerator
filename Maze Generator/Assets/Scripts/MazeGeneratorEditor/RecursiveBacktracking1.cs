using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveBacktracking1 : MazeAlgorithm1
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
        Generation();
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
                mazeGenerator.mazeParent.GetComponent<MazeParent1>().isdoneGenerating = true;
                Debug.Log("<color=lime><B>Successfully generated a maze with \"RecursiveBacktrackingAlgorithm\"</B></color>");
                return;
            }

            currentRow = currentCell.RowIndex;
            currentColumn = currentCell.ColumnIndex;
        }

        Generation();
    }
}

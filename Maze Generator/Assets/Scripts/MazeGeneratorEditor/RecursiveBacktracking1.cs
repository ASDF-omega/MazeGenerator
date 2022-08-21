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
        visit();
    }

    private void visit()
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

        visit();
    }
}

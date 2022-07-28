using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HuntAndKillAlgorithm1 : MazeAlgorithm1
{
    public override void CreateMaze()
    {
        totalCells = rows * columns;
        cellsGenerated = 1;
        isfinished = false;
        DestroyImmediate(GameObject.FindGameObjectWithTag("Maze"));
        mazeGenerator.InstantiateMaze(rows, columns);
        currentRow = 0;
        currentColumn = 0;
        percentGenerated = 0;
        currentCell = maze[currentRow, currentColumn];
        currentCell.isVisited = true;
        EditorUtility.DisplayProgressBar("Generating Maze", (percentGenerated * 100 + "% Completed").ToString(), percentGenerated);
        Kill();
        camera.transform.position = maze[Mathf.RoundToInt(rows / 2), Mathf.RoundToInt(columns / 2)].transform.position;
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
            startedGenerating = false;
            EditorUtility.ClearProgressBar();
            return;
        }
    }

    public void Kill()
    {
        startedGenerating = true;
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
                    currentCell = currentCell.northcell;
                    ++cellsGenerated;
                }
            }

            if (direction == 1)
            {
                if (currentColumn < columns - 1 && !currentCell.eastcell.isVisited)
                {
                    DestroyImmediate(currentCell.eastwall);
                    ++currentColumn;
                    currentCell.eastcell.isVisited = true;
                    currentCell = currentCell.eastcell;
                    ++cellsGenerated;
                }
            }

            if (direction == 2)
            {
                if (currentRow < rows - 1 && !currentCell.southcell.isVisited)
                {
                    DestroyImmediate(currentCell.southwall);
                    ++currentRow;
                    currentCell.southcell.isVisited = true;
                    currentCell = currentCell.southcell;
                    ++cellsGenerated;
                }
            }

            if (direction == 3)
            {
                if (currentColumn > 0 && !currentCell.westcell.isVisited)
                {
                    DestroyImmediate(currentCell.westwall);
                    --currentColumn;
                    currentCell.westcell.isVisited = true;
                    currentCell = currentCell.westcell;
                    ++cellsGenerated;
                }
            }

            currentCellsGenerated = cellsGenerated;

            for (int i = 0; i < currentCellsGenerated-initialCellsGenerated; i++)
            {
                percentGenerated = cellsGenerated / totalCells;
                EditorUtility.DisplayProgressBar("Generating Maze", (percentGenerated * 100 + "% Completed").ToString(), percentGenerated);
                initialCellsGenerated = currentCellsGenerated;
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

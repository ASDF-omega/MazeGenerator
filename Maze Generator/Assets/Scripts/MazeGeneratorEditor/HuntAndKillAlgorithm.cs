using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HuntAndKillAlgorithm : MazeAlgorithm
{
    public override void CreateMaze()
    {
        base.CreateMaze();
        Kill();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                maze[i, j].isVisited = false;
            }
        }
        finalCell = currentCell;
    }

    public void Hunt()
    {
        isfinished = true;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                currentCell = maze[i, j];
                currentRow = i;
                currentColumn = j;
                //the hasAdjacentUnvisitedCells is wrong because currentCell is already set
                if(adjacentUnvisitedCellsOf(currentCell).Length > 0 && adjacentVisitedCellsOf(currentCell).Length > 0)
                {
                    #region break the wall between new found cell and a visited cell
                    int random = new int();

                    if(adjacentVisitedCellsOf(currentCell).Length > 1)
                    {
                        random = Random.Range(0, adjacentVisitedCellsOf(currentCell).Length);
                    }
                    else
                    {
                        random = 0;
                    }
                    Debug.Log(currentCell, currentCell);
                    Debug.Log("adjacentVisitedCellsOf: ");

                    for (int k = 0; k < adjacentVisitedCellsOf(currentCell).Length; k++)
                    {
                        Debug.Log( "   -" + adjacentVisitedCellsOf(currentCell)[k], adjacentVisitedCellsOf(currentCell)[k]);
                    }    

                    if(adjacentVisitedCellsOf(currentCell)[random] == currentCell.northcell)
                    {
                        DestroyImmediate(currentCell.northwall);
                    }
                    else if (adjacentVisitedCellsOf(currentCell)[random] == currentCell.eastcell)
                    {
                        DestroyImmediate(currentCell.eastwall);
                    }
                    else if (adjacentVisitedCellsOf(currentCell)[random] == currentCell.southcell)
                    {
                        DestroyImmediate(currentCell.southwall);
                    }
                    else if (adjacentVisitedCellsOf(currentCell)[random] == currentCell.westcell)
                    {
                        DestroyImmediate(currentCell.westwall);
                    }
                    #endregion
                    currentCell.isVisited = true;
                    isfinished = false;
                    Kill();
                    return;
                }
            }
        }

        if(isfinished)
        {
            mazeGenerator.mazeParent.GetComponent<MazeParent>().isdoneGenerating = true;
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

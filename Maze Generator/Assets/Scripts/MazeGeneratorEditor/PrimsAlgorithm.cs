using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimsAlgorithm : MazeAlgorithm1
{
    private List<MazeCell> FrontTierCells = new List<MazeCell>();

    public override void CreateMaze()
    {
        isfinished = false;
        FrontTierCells.Clear();
        DestroyImmediate(GameObject.FindGameObjectWithTag("Maze"));
        mazeGenerator.InstantiateMaze(rows, columns);
        currentRow = Random.Range(0, rows);
        currentColumn = Random.Range(0, columns);
        currentCell = maze[currentRow, currentColumn];
        initialCell = currentCell;
        currentCell.isVisited = true;
        AddAdjacents(adjacentUnvisitedCellsOf(initialCell));
        Generate();
    }

    private void Generate()
    {
        while(FrontTierCells.Count > 0)
        {
            Carve();
        }

        isfinished = true;
        mazeGenerator.mazeParent.GetComponent<MazeParent1>().isdoneGenerating = true;
        Debug.Log("<color=lime><B>Successfully generated a maze with \"Prim's Algorithm\"</B></color>");
    }

    private void Carve()
    {
        int direction;
        MazeCell cellToGo;

        for (int i = 0; i < FrontTierCells.Count; i++)
        {
            currentCell = FrontTierCells[i];
            currentRow = currentCell.RowIndex;
            currentColumn = currentCell.ColumnIndex;

            if(adjacentVisitedCellsOf(FrontTierCells[i]).Length == 0)
            {
                FrontTierCells.Remove(FrontTierCells[i]);
                return;
            }

            if(adjacentVisitedCellsOf(FrontTierCells[i]).Length > 1)
            {
                direction = Random.Range(0, adjacentVisitedCellsOf(FrontTierCells[i]).Length);
            }
            else
            {
                direction = 0;
            }

            cellToGo = adjacentVisitedCellsOf(FrontTierCells[i])[direction];

            if(cellToGo == currentCell.northcell)
            {
                DestroyImmediate(currentCell.northwall);
            }

            if(cellToGo == currentCell.eastcell)
            {
                DestroyImmediate(currentCell.eastwall);
            }

            if(cellToGo == currentCell.southcell)
            {
                DestroyImmediate(currentCell.southwall);
            }

            if (cellToGo == currentCell.westcell)
            {
                DestroyImmediate(currentCell.westwall);
            }

            currentCell.isVisited = true;
            FrontTierCells.Remove(currentCell);
            
            if(adjacentUnvisitedCellsOf(currentCell).Length > 0)
            {
                AddAdjacents(adjacentUnvisitedCellsOf(currentCell));
            }
        }
    }

    private void AddAdjacents(MazeCell[] cells)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if(!FrontTierCells.Contains(cells[i]))
            {
                FrontTierCells.Add(cells[i]);
            }
        }
    }
}

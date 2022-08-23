using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimsAlgorithm : MazeAlgorithm
{
    private List<MazeCell> FrontTierCells = new List<MazeCell>();

    public override void CreateMaze()
    {
        base.CreateMaze();
        AddAdjacents(adjacentUnvisitedCellsOf(initialCell));
        Generate();
    }

    private void Generate()
    {
        while (FrontTierCells.Count > 0)
        {
            Carve();
        }

        isfinished = true;
        mazeLoader.mazeParent.GetComponent<MazeParent>().isdoneGenerating = true;
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

            if (adjacentVisitedCellsOf(FrontTierCells[i]).Length == 0)
            {
                FrontTierCells.Remove(FrontTierCells[i]);
                return;
            }

            if (adjacentVisitedCellsOf(FrontTierCells[i]).Length > 1)
            {
                direction = Random.Range(0, adjacentVisitedCellsOf(FrontTierCells[i]).Length);
            }
            else
            {
                direction = 0;
            }

            cellToGo = adjacentVisitedCellsOf(FrontTierCells[i])[direction];

            if (cellToGo == currentCell.northcell)
            {
                Destroy(currentCell.northwall);
            }

            if (cellToGo == currentCell.eastcell)
            {
                Destroy(currentCell.eastwall);
            }

            if (cellToGo == currentCell.southcell)
            {
                Destroy(currentCell.southwall);
            }

            if (cellToGo == currentCell.westcell)
            {
                Destroy(currentCell.westwall);
            }

            currentCell.isVisited = true;
            FrontTierCells.Remove(currentCell);

            if (adjacentUnvisitedCellsOf(currentCell).Length > 0)
            {
                AddAdjacents(adjacentUnvisitedCellsOf(currentCell));
            }
        }
    }

    private void AddAdjacents(MazeCell[] cells)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (!FrontTierCells.Contains(cells[i]))
            {
                FrontTierCells.Add(cells[i]);
            }
        }
    }
}

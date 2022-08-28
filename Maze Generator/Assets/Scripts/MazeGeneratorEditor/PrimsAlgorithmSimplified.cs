using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimsAlgorithmSimplified : MazeAlgorithm
{
    private List<OrthogonalMazeCell> FrontTierCells = new List<OrthogonalMazeCell>();

    public override void CreateMaze()
    {
        base.CreateMaze();
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
        mazeGenerator.mazeParent.GetComponent<MazeParent>().isdoneGenerating = true;
        Debug.Log("<color=lime><B>Successfully generated a maze with \"Prim's Algorithm(Simplified)\"</B></color>");
    }

    private void Carve()
    {
        int direction;
        OrthogonalMazeCell cellToGo;

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

    private void AddAdjacents(OrthogonalMazeCell[] cells)
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

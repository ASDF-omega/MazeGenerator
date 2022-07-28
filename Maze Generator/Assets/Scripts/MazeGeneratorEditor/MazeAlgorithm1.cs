using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeAlgorithm1 : MonoBehaviour
{
    public MazeCell[,] maze;
    public int currentRow = 0;
    public int currentColumn = 0;
    public int rows;
    public int columns;
    public int cellsGenerated = 1;
    public MazeGenerator mazeGenerator;
    public MazeCell currentCell;
    public bool isfinished;
    public int percentGenerated;

    protected int initialCellsGenerated;
    protected int currentCellsGenerated;
    protected CombineInstance[] combineInstances;
    protected bool startedGenerating;

    [SerializeField] protected int totalCells;

    [SerializeField] protected Camera camera;

    public abstract void CreateMaze();
}

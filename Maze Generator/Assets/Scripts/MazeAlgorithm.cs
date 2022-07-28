using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazeAlgorithm : MonoBehaviour
{
    public MazeCell[,] maze;
    public int currentRow = 0;
    public int currentColumn = 0;
    public int rows;
    public int columns;
    public MazeCell currentCell;

    protected bool isfinished;
    protected CombineInstance[] combineInstances;

    [SerializeField] protected MazeLoader mazeLoader;
    [SerializeField] protected Camera camera;

    public abstract void CreateMaze();
}

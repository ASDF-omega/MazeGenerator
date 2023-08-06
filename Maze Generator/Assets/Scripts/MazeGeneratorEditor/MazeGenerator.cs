using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class MazeGenerator : MonoBehaviour
{
    public int Rows;
    public int Columns;
    public GameObject mazeParentObject;
    public GameObject mazeParent;
    public MazeAlgorithm algorithm;
    public GammaCell[,] maze;
    public GameObject floor;
    public GameObject wall;
    public PathFinding pathFinding;

    [Header("Options")]
    public CombineOptions CombineAs;
    public CombinedMeshes __;
    public MazeAlgorithms Algorithm;
    public Routes Route;
    public int percent;
    public CellType tessellation;

    public enum CombineOptions { SingleMesh, FloorMeshAndWallMesh };
    public enum CombinedMeshes { Destroy, Disable };
    public enum MazeAlgorithms { HuntAndKillAlgorithm, RecursiveBackTracking, PrimsAlgorithm};
    public enum Routes { Braid, Perfect, Sparse };
    public enum CellType { Gamma, Delta, Sigma, Theata };
    public void InstantiateGammaMaze(int rows, int columns)
    {
        DestroyImmediate(GameObject.FindGameObjectWithTag("Maze"));
        mazeParent = Instantiate(mazeParentObject, new Vector3(0, 0, 0), Quaternion.identity);
        mazeParent.name = "Maze";

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                float size = floor.transform.localScale.x;
                GameObject Floor = Instantiate(floor, new Vector3(j * size, 0, -i * size), Quaternion.identity);
                maze[i, j] = Floor.GetComponent<GammaCell>();
                Floor.name = "Floor_" + i + ", " + j;
                Floor.transform.parent = mazeParent.transform;

                maze[i, j].RowIndex = i;
                maze[i, j].ColumnIndex = j;
            }
        }

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                float sizeX = floor.transform.localScale.x;
                float sizeY = floor.transform.localScale.y;
                float sizeZ = floor.transform.localScale.z;

                if (maze[i, j].northwall == null)
                {
                    maze[i, j].northwall = Instantiate(wall, new Vector3(j * sizeX, wall.transform.localScale.y/2, -i * sizeZ + sizeZ / 2), Quaternion.identity);
                    maze[i, j].northwall.transform.parent = mazeParent.transform;

                    if(i > 0)
                    {
                        maze[i - 1, j].southwall = maze[i, j].northwall;
                    }
                }

                if (maze[i, j].eastwall == null)
                {
                    maze[i, j].eastwall = Instantiate(wall, new Vector3(j * sizeX + sizeX/2, wall.transform.localScale.y / 2, -i * sizeZ), Quaternion.Euler(0, 90, 0));
                    maze[i, j].eastwall.transform.parent = mazeParent.transform;
                    
                    if(j < columns - 1)
                    {
                        maze[i, j + 1].westwall = maze[i, j].eastwall;
                    }
                }

                if (maze[i, j].southwall == null)
                {
                    maze[i, j].southwall = Instantiate(wall, new Vector3(j * sizeX, wall.transform.localScale.y / 2, -i * sizeZ - sizeZ / 2), Quaternion.identity);
                    maze[i, j].southwall.transform.parent = mazeParent.transform;

                    if(i < rows - 1)
                    {
                        maze[i + 1, j].northwall = maze[i, j].southwall;
                    }
                }

                if (maze[i, j].westwall == null)
                {
                    maze[i, j].westwall = Instantiate(wall, new Vector3(j * sizeX - sizeX/2, wall.transform.localScale.y / 2, -i * sizeZ), Quaternion.Euler(0, 90, 0));
                    maze[i, j].westwall.transform.parent = mazeParent.transform;

                    if(j > 0)
                    {
                        maze[i, j - 1].eastwall = maze[i, j].westwall;
                    }
                }
            }
        }

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (i > 0)
                {
                    maze[i, j].northcell = maze[i - 1, j];
                }

                if (j < Columns - 1)
                {
                    maze[i, j].eastcell = maze[i, j + 1];
                }

                if (i < Rows - 1)
                {
                    maze[i, j].southcell = maze[i + 1, j];
                }

                if (j > 0)
                {
                    maze[i, j].westcell = maze[i, j - 1];
                }
            }
        }
    }
}


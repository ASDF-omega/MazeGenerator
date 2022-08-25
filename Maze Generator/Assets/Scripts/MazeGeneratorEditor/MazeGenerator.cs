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
    public MazeAlgorithm1 algorithm;
    public MazeCell[,] maze;
    public GameObject floor;
    public GameObject wall;

    [Header("Options")]
    public CombineOptions combineOptions;
    public _ __;
    public MazeAlgorithms Algorithm;
    public Routes Route;

    public enum CombineOptions { combine_Into_One_Mesh, combine_Into_Floors_And_Walls };
    public enum _ { Destroy_Combined_Meshes, Disable_Combined_Meshes };
    public enum MazeAlgorithms { HuntAndKillAlgorithm, RecursiveBackTracking, KruskalsAlgorithm, PrimsAlgorithm, Unicursal};
    public enum Routes { Braid, PartialBraid, Perfect, Sparse };
    public void InstantiateMaze(int rows, int columns)
    {
        mazeParent = Instantiate(mazeParentObject, new Vector3(0, 0, 0), Quaternion.identity);
        mazeParent.name = "Maze";

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject Floor = Instantiate(floor, new Vector3(j, 0, -i), Quaternion.identity);
                maze[i, j] = Floor.GetComponent<MazeCell>();
                Floor.name = "Floor_" + i + ", " + j;
                Floor.transform.parent = mazeParent.transform;


                GameObject NorthWall = Instantiate(wall, new Vector3(j, 0.5f, -i + Floor.transform.localScale.z / 2), Quaternion.identity);
                maze[i, j].northwall = NorthWall;
                NorthWall.transform.parent = mazeParent.transform;
                GameObject WestWall = Instantiate(wall, new Vector3(j - Floor.transform.localScale.z / 2, 0.5f, -i), Quaternion.Euler(0, 90, 0));
                maze[i, j].westwall = WestWall;
                WestWall.transform.parent = mazeParent.transform;

                if (i == rows - 1)
                {
                    //instantiates walls on the southern edge of the maze
                    GameObject SouthEdgeWall = Instantiate(wall, new Vector3(j, 0.5f, -i - Floor.transform.localScale.z / 2), Quaternion.identity);
                    maze[i, j].southwall = SouthEdgeWall;
                    SouthEdgeWall.transform.parent = mazeParent.transform;
                }

                if (j == columns - 1)
                {
                    //instantiates walls on the eastern edge of the maze
                    GameObject EastEdgeWall = Instantiate(wall, new Vector3(j + Floor.transform.localScale.z / 2, 0.5f, -i), Quaternion.Euler(0, 90, 0));
                    maze[i, j].eastwall = EastEdgeWall;
                    EastEdgeWall.transform.parent = mazeParent.transform;
                }

                maze[i, j].RowIndex = i;
                maze[i, j].ColumnIndex = j;
            }
        }

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
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

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                //sets the north wall
                if (i < Rows - 1)
                {
                    maze[i, j].southwall = maze[i + 1, j].northwall;
                }

                //sets the east wall
                if (j < Columns - 1)
                {
                    maze[i, j].eastwall = maze[i, j + 1].westwall;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MazeGenerator))]
public class EditorMazeGenerator : Editor
{
    public override void OnInspectorGUI()
    {
        MazeGenerator mazeGenerator = target as MazeGenerator;

        #region inspector display
        EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        mazeGenerator.Rows = EditorGUILayout.IntSlider("Rows", mazeGenerator.Rows, 2, 100);
        mazeGenerator.Columns = EditorGUILayout.IntSlider("Columns", mazeGenerator.Columns, 2, 100);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("References", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        mazeGenerator.mazeParentObject = (GameObject)EditorGUILayout.ObjectField("Maze Parent Object", mazeGenerator.mazeParentObject, typeof(GameObject), false);
        mazeGenerator.floor = (GameObject)EditorGUILayout.ObjectField("Floor", mazeGenerator.floor, typeof(GameObject), false);
        mazeGenerator.wall = (GameObject)EditorGUILayout.ObjectField("Wall", mazeGenerator.wall, typeof(GameObject), false);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Options", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        mazeGenerator.combineOptions = (MazeGenerator.CombineOptions)EditorGUILayout.EnumPopup("Combine Options", mazeGenerator.combineOptions);
        mazeGenerator.__ = (MazeGenerator._)EditorGUILayout.EnumPopup(" ", mazeGenerator.__);
        mazeGenerator.Algorithm = (MazeGenerator.MazeAlgorithms)EditorGUILayout.EnumPopup("Algorithm", mazeGenerator.Algorithm);
        mazeGenerator.Route = (MazeGenerator.Routes)EditorGUILayout.EnumPopup("Route", mazeGenerator.Route);

        switch (mazeGenerator.Route)
        {
            case MazeGenerator.Routes.Braid:
                mazeGenerator.percent = EditorGUILayout.IntSlider("Braid Percent", mazeGenerator.percent, 0, 100);
                break;
            case MazeGenerator.Routes.Perfect:
                break;
            case MazeGenerator.Routes.Sparse:
                mazeGenerator.percent = EditorGUILayout.IntSlider("Sparse Percent", mazeGenerator.percent, 0, 100);
                break;
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Buttons", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if (GUILayout.Button("Generate New Maze"))
        {
            mazeGenerator.algorithm.CreateMaze();

            switch (mazeGenerator.Route)
            {
                #region Braid
                case MazeGenerator.Routes.Braid:

                    List<MazeCell> DeadEndCells = new List<MazeCell>();
                    int AmountOfCellsToRemoveFromDeadEndCells = 0;

                    #region checking for each cell's links
                    for (int i = 0; i < mazeGenerator.Rows; i++)
                    {
                        for (int j = 0; j < mazeGenerator.Columns; j++)
                        {
                            if (mazeGenerator.maze[i, j].northwall == null)
                            {
                                ++mazeGenerator.maze[i, j].links;
                            }

                            if (mazeGenerator.maze[i, j].eastwall == null)
                            {
                                ++mazeGenerator.maze[i, j].links;
                            }

                            if (mazeGenerator.maze[i, j].southwall == null)
                            {
                                ++mazeGenerator.maze[i, j].links;
                            }

                            if (mazeGenerator.maze[i, j].westwall == null)
                            {
                                ++mazeGenerator.maze[i, j].links;
                            }

                            if(mazeGenerator.maze[i, j].links == 1)
                            {
                                DeadEndCells.Add(mazeGenerator.maze[i, j]);
                            }
                        }
                    }
                    #endregion

                    AmountOfCellsToRemoveFromDeadEndCells = Mathf.FloorToInt(DeadEndCells.Count * ((100 - mazeGenerator.percent) * 0.01f));

                    for (int i = 0; i < AmountOfCellsToRemoveFromDeadEndCells; i++)
                    {
                        DeadEndCells.Remove(DeadEndCells[Random.Range(0, DeadEndCells.Count)]);
                    }

                    for (int i = 0; i < DeadEndCells.Count; i++)
                    {
                        int randomCellToGoTo = 0;

                        if (DeadEndCells[i].AvailableWalls().Length > 0)
                        {
                            randomCellToGoTo = Random.Range(0, DeadEndCells[i].AvailableWalls().Length);
                        }

                        DestroyImmediate(DeadEndCells[i].AvailableWalls()[randomCellToGoTo]);
                    }

                    break;
                #endregion
                case MazeGenerator.Routes.Perfect:
                    break;
                case MazeGenerator.Routes.Sparse:
                    break;
            }
        }

        if(GUILayout.Button("Combine Maze"))
        {
            if(mazeGenerator.mazeParent != null)
            {
                mazeGenerator.mazeParent.GetComponent<MazeParent>().combineMaze();
            }
            else
            {
                EditorUtility.DisplayDialog("Error!", "A maze must be generated before it's meshes can be combined!", "ok");
            }
        }

        if (GUILayout.Button("Save Maze") && mazeGenerator.algorithm.isfinished)
        {
            if(mazeGenerator.mazeParent != null)
            {
                mazeGenerator.mazeParent.GetComponent<MazeParent>().saveMaze();
            }
            else
            {
                EditorUtility.DisplayDialog("Error!", "A maze must be generated before it can be saved!", "ok");
            }
        }
        #endregion

        mazeGenerator.maze = new MazeCell[mazeGenerator.Rows, mazeGenerator.Columns];
        mazeGenerator.algorithm.maze = mazeGenerator.maze;
        mazeGenerator.algorithm.rows = mazeGenerator.Rows;
        mazeGenerator.algorithm.columns = mazeGenerator.Columns;

        switch(mazeGenerator.Algorithm)
        {
            case MazeGenerator.MazeAlgorithms.HuntAndKillAlgorithm:
                mazeGenerator.algorithm = mazeGenerator.GetComponent<HuntAndKillAlgorithm>();
                break;
            case MazeGenerator.MazeAlgorithms.RecursiveBackTracking:
                mazeGenerator.algorithm = mazeGenerator.GetComponent<RecursiveBacktracking>();
                break;
            case MazeGenerator.MazeAlgorithms.PrimsAlgorithm:
                mazeGenerator.algorithm = mazeGenerator.GetComponent<PrimsAlgorithmSimplified>();
                break;
        }
    }
}

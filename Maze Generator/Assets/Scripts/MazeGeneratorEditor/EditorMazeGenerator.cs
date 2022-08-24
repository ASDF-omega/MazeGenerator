using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MazeGenerator))]
public class EditorMazeGenerator : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MazeGenerator mazeGenerator = target as MazeGenerator;

        mazeGenerator.maze = new MazeCell[mazeGenerator.Rows, mazeGenerator.Columns];
        mazeGenerator.algorithm.maze = mazeGenerator.maze;
        mazeGenerator.algorithm.rows = mazeGenerator.Rows;
        mazeGenerator.algorithm.columns = mazeGenerator.Columns;


        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Buttons", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        switch(mazeGenerator.Algorithm)
        {
            case MazeGenerator.MazeAlgorithms.HuntAndKillAlgorithm:
                mazeGenerator.algorithm = mazeGenerator.GetComponent<HuntAndKillAlgorithm1>();
                break;
            case MazeGenerator.MazeAlgorithms.RecursiveBackTracking:
                mazeGenerator.algorithm = mazeGenerator.GetComponent<RecursiveBacktracking1>();
                break;
            case MazeGenerator.MazeAlgorithms.PrimsAlgorithm:
                mazeGenerator.algorithm = mazeGenerator.GetComponent<PrimsAlgorithm1>();
                break;
        }

        if (GUILayout.Button("Generate New Maze"))
        {
            mazeGenerator.algorithm.CreateMaze();

            switch(mazeGenerator.Route)
            {
                case MazeGenerator.Routes.Braid:
                    break;
                case MazeGenerator.Routes.PartialBraid:
                    break;
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
                mazeGenerator.mazeParent.GetComponent<MazeParent1>().combineMaze();
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
                mazeGenerator.mazeParent.GetComponent<MazeParent1>().saveMaze();
            }
            else
            {
                EditorUtility.DisplayDialog("Error!", "A maze must be generated before it can be saved!", "ok");
            }
        }
    }
}

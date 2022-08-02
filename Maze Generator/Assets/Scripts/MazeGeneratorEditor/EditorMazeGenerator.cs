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

        if (GUILayout.Button("Generate New Maze"))
        {
            mazeGenerator.algorithm.CreateMaze();
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
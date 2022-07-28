using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MazeAlgorithm1))]
public class EditorMazeAlgorithm : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MazeAlgorithm1 algorithm1 = target as MazeAlgorithm1;
    }
}

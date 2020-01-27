using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (GridManager))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GridManager gridManager = target as GridManager;

        gridManager.GenerateGrid(true);
    }
}

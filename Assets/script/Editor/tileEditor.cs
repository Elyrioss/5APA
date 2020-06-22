using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(tileMapManager))]
public class tileEditor : Editor
{
    // Start is called before the first frame update
    private tileMapManager map { get { return target as tileMapManager; } }
    
    public override void OnInspectorGUI()
    {
        if (DrawDefaultInspector())
        {
            if (false)
            {
                map.Generate();
            }
        }

        if (GUILayout.Button("Generate Map")) // BOUTON GENERATE MAP
        {
            map.Generate();
        }
        
        if (GUILayout.Button("Clear")) // BOUTON GENERATE MAP
        {
            map.Clear();
        } 
        
        if (GUILayout.Button("Path")) // BOUTON GENERATE MAP
        {
            map.ShowDijtra();
        } 
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(tileMapManager))]
public class tileEditor : Editor
{
    // Start is called before the first frame update
    private tileMapManager map { get { return target as tileMapManager; } }

    private int i=0;
    
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
       
        
        if (GUILayout.Button("Save")) // BOUTON GENERATE MAP
        {
            map.SaveManager.SaveGame(map);
        } 
        
        if (GUILayout.Button("DeleteSave")) // BOUTON GENERATE MAP
        {
            map.SaveManager.DeleteSave();
        } 
        
    }
}

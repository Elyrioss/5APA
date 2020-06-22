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
        
        if (GUILayout.Button("Path")) // BOUTON GENERATE MAP
        {
            map.ShowDijtra();
        } 
        
        if (GUILayout.Button("nextBiome")) // BOUTON GENERATE MAP
        {
            if (i >= map.Biomes.Count)
                i = 0;
            if (map.Biomes.Count != 0)
            {
                map.ShowBiome(i);
                i++;
            }
                      
        } 

        if (GUILayout.Button("Reset")) // BOUTON GENERATE MAP
        {
            map.ResetBiome();
        } 

        if (GUILayout.Button("Combine")) // BOUTON GENERATE MAP
        {
            map.HeightCombine();
        } 
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextureModifier))]
[CanEditMultipleObjects]
[ExecuteInEditMode]
public class TextureModifierEditor : Editor
{
    private TextureModifier creator;

    private void OnEnable()
    {
        creator = target as TextureModifier;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();

        TextureModifier textureModifier = (TextureModifier)target;
        /*
        //Params
        GUILayout.Label("Params");
        serializedObject.Update();
        textureModifier.treeObject = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Source", "Add object to fracture"), textureModifier.treeObject, typeof(GameObject), true);
        serializedObject.ApplyModifiedProperties();
        textureModifier.age = EditorGUILayout.IntField("Age", textureModifier.age);
        textureModifier.agingValue = EditorGUILayout.FloatField("Aging value", textureModifier.agingValue);
        textureModifier.nbOrigin = EditorGUILayout.IntField("Origin number", textureModifier.nbOrigin);
        textureModifier.maxRadiusOrigin = EditorGUILayout.IntField("Max radius origin", textureModifier.maxRadiusOrigin);
        textureModifier.variationValue = EditorGUILayout.FloatField("Variation value", textureModifier.variationValue);
        textureModifier.colorRef = EditorGUILayout.ColorField("Color reference", textureModifier.colorRef);
        */
        Texture2D savedTex = AssetPreview.GetAssetPreview(textureModifier.savedTex);
        Texture2D tex = AssetPreview.GetAssetPreview(textureModifier.tex);
        
        GUILayout.Label("Saved texture");
        GUILayout.Label(savedTex);
        GUILayout.Label("Actual texture");
        GUILayout.Label(tex);

        if (GUILayout.Button("Save Material"))
        {
            textureModifier.SaveMaterial();
        }

        if (GUILayout.Button("Restore Material"))
        {
            textureModifier.RestoreMaterial();
        }

        if (GUILayout.Button("Apply Aging"))
        {
            textureModifier.LaunchAgingTexture();
        }

        /*if (GUILayout.Button("Clear Tree"))
        {
            textureModifier.ClearTree();
        }*/
    }
}
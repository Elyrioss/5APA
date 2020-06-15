﻿//Florent WASSEN
/**
 * Représente les lieux sur la carte.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct ActionChoice
{
    public string actionName;
    public UnityEvent actionEvent;
}

[RequireComponent(typeof(BoxCollider))]
public class Waypoint : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> Neighbors = new List<GameObject>(6){null,null,null,null,null,null};

    public GameObject left = null;
    public GameObject right = null;
    public GameObject leftBot = null;
    public GameObject rightBot = null;
    public GameObject leftTop = null;
    public GameObject rightTop = null;
    
    public GameObject spriteContainer;
    public bool visited = false;
    [SerializeField]
    public SpriteRenderer[] spriteRenderer;// left , leftbot , lefttop, right , rightbot, rightop
    [HideInInspector]
    public int X;
    [HideInInspector]
    public int Y;
    [HideInInspector]
    public int i;
    [HideInInspector]
    public int j;

    public bool odd;

    public float noiseValue;
    public float elevation=0;
    public type type;
    
    public float Food;
    public float Production;
    public float Gold;
    
    [SerializeField] public Color CivColor = Color.blue;
    [SerializeField]
    Color Deactivated = Color.clear;

    

    void Awake()
    {
        DisableWaypoint();
    }

    public void DisableWaypoint() // make Waypoint unreachable
    {
        foreach (SpriteRenderer sprite in spriteRenderer)
        {
            sprite.color = Deactivated;
        }
    }

    public void EnableWaypoint() // make Waypoint reachable
    {
        foreach (SpriteRenderer sprite in spriteRenderer)
        {
            sprite.color = CivColor;
        }
    }

    [HideInInspector]
    public List<string> names;
    [HideInInspector]
    public string tilemapname;
    [HideInInspector]
    public int tileid;
    public Waypoint(SavedWaypoint w)
    {
        names = w.Names;
        type = w.type;
        elevation = w.elevation;
        X = w.X;
        Y = w.Y;
        i = w.i;
        j = w.j;
        tileid = w.tileid;
    }
    public void Clicked()
    {
        
    }
}

//Florent WASSEN
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
    [SerializeField]
    List<GameObject> neighbors = new List<GameObject>();
    [HideInInspector]
    public bool coastal= false;
    [HideInInspector]
    public bool visited = false;
    [SerializeField]
    public SpriteRenderer spriteRenderer = null;
    BoxCollider boxCollider = null;
    [HideInInspector]
    public int X;
    [HideInInspector]
    public int Y;
    [HideInInspector]
    public int i;
    [HideInInspector]
    public int j;
    [HideInInspector]
    public Tilemap tilemap;
    [HideInInspector]
    public bool odd;
    
    public float elevation=0;
    public tileType.type type;
    
    [SerializeField]
    Color CivColor = Color.blue;
    [SerializeField]
    Color Deactivated = Color.clear;

    public List<GameObject> Neighbors
    {
        get {
            return neighbors;
        }
    }


    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        DisableWaypoint();
    }

    public void DisableWaypoint() // make Waypoint unreachable
    {
        /*BoxCollider bc = GetComponent<BoxCollider>();
        if (bc)
            bc.enabled = false;
        */
        spriteRenderer.color = Deactivated;
    }

    public void EnableWaypoint() // make Waypoint reachable
    {
        BoxCollider bc = GetComponent<BoxCollider>();
        if (bc)
            bc.enabled = true;

        spriteRenderer.color = CivColor;
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
        tilemapname = w.tileMap;
        tileid = w.tileid;
    }
    public void Clicked()
    {
        
    }
}

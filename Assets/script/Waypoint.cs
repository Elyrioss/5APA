//Florent WASSEN
/**
 * Représente les lieux sur la carte.
 */

using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



[RequireComponent(typeof(BoxCollider))]
public class Waypoint : MonoBehaviour
{
    [HideInInspector]
    public List<Waypoint> Neighbors = new List<Waypoint>(6){null,null,null,null,null,null};
    
    [HideInInspector]

    public Waypoint left = null;

    [HideInInspector]

    public Waypoint right = null;

    [HideInInspector]

    public Waypoint leftBot = null;

    [HideInInspector]
    
    public Waypoint rightBot = null;
   
    [HideInInspector]

    public Waypoint leftTop = null;
    
    [HideInInspector]
    public Waypoint rightTop = null;
    
    public GameObject spriteContainer;
    
    [HideInInspector]
    public bool visitedDijstra = false;
    [HideInInspector]
    public bool visited = false;
    
    [SerializeField]
    public SpriteRenderer[] spriteRenderer;// left , leftbot , lefttop, right , rightbot, rightop
    [HideInInspector]
    public int X;
    [HideInInspector]
    public int Y;

    [HideInInspector]
    public bool odd;
 
    public TileMapPos Chunk;
   
    public Waypoint Twin;
    [HideInInspector]
    public bool AsTwin=false;
    [HideInInspector]
    public bool IsTwin=false;
    [HideInInspector]
    public float noiseValue;
    
    public float elevation=0;
    public BiomeType BiomeType;
    public HeightType HeightType;
    public HeatType HeatType;
    public MoistureType MoistureType;

    public Animator Animator;
    
    public float Food;
    public float Production;
    public float Gold;

    public GameObject LOD;

    public GameObject prop;

    public string Prop;
    

    public MeshFilter TileFilter;
    [HideInInspector]

    public Material mat;
    [SerializeField] public Color CivColor = Color.blue;
    [SerializeField]
    Color Deactivated = Color.clear;

    public bool UsedTile=false;
    
    public int mouvCost=1;
    public int MinCostToStart;
    public Waypoint NearestToStart;
    public float HeuristicDist;
    public SpriteRenderer highlight;

    void Awake()
    {
        DisableWaypoint();
        DisableHighlight();
    }

    public void DisableWaypoint() // make Waypoint unreachable
    {
        foreach (SpriteRenderer sprite in spriteRenderer)
        {
            sprite.color = Deactivated;
        }
        highlight.color = Deactivated;
    }

    public void EnableWaypoint() // make Waypoint reachable
    {
        foreach (SpriteRenderer sprite in spriteRenderer)
        {
            sprite.color = CivColor;
        }
    }

    public void DisableHighlight()
    {
        highlight.color = Deactivated;
        Animator.SetBool("shine",false);
    }

    public void EnableHighlight() // highlight Waypoint
    {
        highlight.color = CivColor;
        Animator.SetBool("shine",true);
    }

    
}

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
    public List<Waypoint> Neighbors = new List<Waypoint>(6){null,null,null,null,null,null};

    public Waypoint left = null;
    public Waypoint right = null;
    public Waypoint leftBot = null;
    public Waypoint rightBot = null;
    public Waypoint leftTop = null;
    public Waypoint rightTop = null;
    
    public GameObject spriteContainer;
    
    public bool visited = false;
    
    [SerializeField]
    public SpriteRenderer[] spriteRenderer;// left , leftbot , lefttop, right , rightbot, rightop
    public int X;
    public int Y;
    [HideInInspector]
    public int i;
    [HideInInspector]
    public int j;

    public bool odd;

    public TileMapPos Chunk;
    
    public float noiseValue;
    public float elevation=0;
    public BiomeType BiomeType;
    public HeightType HeightType;
    public HeatType HeatType;
    public MoistureType MoistureType;
    
    public float Food;
    public float Production;
    public float Gold;
    
    [SerializeField] public Color CivColor = Color.blue;
    [SerializeField]
    Color Deactivated = Color.clear;

    public int mouvCost=1;
    public int MinCostToStart;
    public Waypoint NearestToStart;
    
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

    public void BuildShortestPath(List<Waypoint> list, Waypoint node)
    {
        if (node.NearestToStart == null)
            return;
        list.Add(node.NearestToStart);
        BuildShortestPath(list, node.NearestToStart);
    }
    
    public void DijkstraSearch(Waypoint Start,Waypoint End)
    {

        Start.MinCostToStart = 0;
        var prioQueue = new List<Waypoint>();
        prioQueue.Add(Start);
        
        do {
            prioQueue = prioQueue.OrderBy(x => x.MinCostToStart).ToList();
            var node = prioQueue.First();
            prioQueue.Remove(node);
            
            foreach (var cnn in node.Neighbors.OrderBy(x => x.GetComponent<Waypoint>().mouvCost))
            {
                var childNode = cnn;
                if (childNode.visited)
                    continue;
                Debug.Log(prioQueue.Count);
                if (node.MinCostToStart + cnn.mouvCost < childNode.MinCostToStart)
                {
                    childNode.MinCostToStart = node.MinCostToStart + cnn.mouvCost;
                    childNode.NearestToStart = node;
                    if (!prioQueue.Contains(childNode))
                        prioQueue.Add(childNode);
                }
            }            
            
            node.visited = true;
            if (node == End)
                return;           
        } while (prioQueue.Any());
        
    }
    
    
}

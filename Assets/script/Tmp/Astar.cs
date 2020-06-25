using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Astar : MonoBehaviour
{
    public List<Waypoint> Path  = new List<Waypoint>();

    List<Waypoint> PQueue = new List<Waypoint>();
    List<Waypoint> CameFrom = new List<Waypoint>();

    //
    private Camera ThisCamera;
    private bool GetStart = true;
    public Waypoint StartWay;
    private bool GetEnd;
    public Waypoint EndWay;
    private void Start()
    {
        ThisCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = ThisCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) 
            {
                if (!hit.transform.parent.gameObject.GetComponent<Waypoint>())
                    return;

                if (GetStart)
                {
                    StartWay = hit.transform.parent.gameObject.GetComponent<Waypoint>();
                    GetStart = false;
                    GetEnd = true;
                }
                else if (GetEnd)
                {
                    EndWay = hit.transform.parent.gameObject.GetComponent<Waypoint>();
                    GetEnd = false;
                }

            }
        }
    }

    public void AStarCreate()
    {
        Waypoint Start = StartWay;
        Waypoint End = EndWay;
        if (Start == null || End == null)
        {
            return ;
        }
        Start.MinCostToStart = 0;
        PQueue.Add(Start);
        while (PQueue.Any())
        {
            Debug.Log("IN");
            var current = PQueue.First();
            PQueue.Remove(current);
            if(current == End)
            {
                break;
            }
            foreach(Waypoint w in current.Neighbors)
            {
                if (w.visitedDijstra)
                {
                    continue;
                }
                var nextCost = current.MinCostToStart + w.mouvCost;
                if (nextCost < w.MinCostToStart)
                {            
                    w.MinCostToStart = nextCost;
                    w.NearestToStart = current;
                    w.HeuristicDist = Heuristic(w, End);
                    if (!PQueue.Contains(w))
                    {
                        PQueue.Add(w);
                    } 
                }
            }
            current.visitedDijstra = true;
            PQueue = PQueue.OrderBy(x => x.HeuristicDist).ToList();
        }
        Debug.Log("OUUUUUUT");
        CreatePath(Path, End);
        Path.Reverse();
        DrawPath(Path);
    }


    private void CreatePath(List<Waypoint> TmpPath,Waypoint W)
    {
        if(W.NearestToStart == null)
        {
            return;
        }
        TmpPath.Add(W.NearestToStart);
        CreatePath(TmpPath, W.NearestToStart);
    }


    private void DrawPath(List<Waypoint> DPath)
    {
        foreach(Waypoint w in DPath)
        {
            w.CivColor = Color.red;
            w.EnableWaypoint();
        }
        
    }

    private float Heuristic(Waypoint a, Waypoint b)
    {
        float dist = Vector3.Distance(a.transform.position,b.transform.position);
        return dist;
    }

}

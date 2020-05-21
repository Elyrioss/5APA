using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City
{
    public int Population;
    public Waypoint Position;
    public int TurnUntilGrowth;
    
    public List<Waypoint> ControlArea=new List<Waypoint>();

    public float Food;
    public float Production;
    public float Science;
    public float Culture;
    public float Faith;
    public float Gold;

    public City(Waypoint position)
    {
        Position = position;
        Position.EnableWaypoint();
        foreach (GameObject w in Position.Neighbors)
        {
            ControlArea.Add(w.GetComponent<Waypoint>());
            Debug.Log(w);
            w.GetComponent<Waypoint>().EnableWaypoint();
        }
        Population = 1;
        
    }
    
}

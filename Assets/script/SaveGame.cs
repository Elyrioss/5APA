using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame
{

    //public string LastWaypoint;
    public string[] Waypoints;
    public int seed;
    
    public SaveGame(tileMapManager tileMapMan)
    {   

        //Waypoints
        //LastWaypoint =  JsonUtility.ToJson(lastWaypoint);
        List<Waypoint> waypoints = tileMapMan.ChunkOrder;
        Waypoints = new string[waypoints.Count];
        for (int i = 0; i < waypoints.Count; ++i)
        {
            Waypoints[i] = JsonUtility.ToJson(new SavedWaypoint(waypoints[i]));
        }

        seed = tileMapMan.seed;
    }
 
}

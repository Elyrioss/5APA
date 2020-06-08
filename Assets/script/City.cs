using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class City
{
    
    public int population;
    public Waypoint position;
    
    public List<Waypoint> controlArea=new List<Waypoint>();

    public float food = 0;
    public float production = 0;
    public float gold = 0;
    
    public Construction construction;
       
    public City(Waypoint position)
    {
        position = position;
        position.EnableWaypoint();
        food += position.Food;
        production += position.Production;
        gold += position.Gold;
        foreach (GameObject w in position.Neighbors)
        {
            Waypoint W = w.GetComponent<Waypoint>();
            controlArea.Add(W);
            W.EnableWaypoint();
            food += W.Food;
            production += W.Production;
            gold += W.Gold;
        }
        population = 1;
        
    }
    
}

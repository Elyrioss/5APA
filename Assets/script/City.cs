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

    public bool ExtendTown(Waypoint w)
    {
        if (!controlArea.Contains(w))
        {
            controlArea.Add(w);
            ClearFrontiers(w);
            w.EnableWaypoint();
            return true;
        }

        return false;
    }

    public void ClearFrontiers()
    {
        foreach (Waypoint waypoint in controlArea)
        {
            if (waypoint.left)
            {
                if (controlArea.Contains(waypoint.left.GetComponent<Waypoint>()))
                {
                    waypoint.left.SetActive(false);
                }
            }
            if (waypoint.right)
            {
                if (controlArea.Contains(waypoint.right.GetComponent<Waypoint>()))
                {
                    waypoint.right.SetActive(false);
                }
            }
            if (waypoint.leftTop)
            {
                if (controlArea.Contains(waypoint.leftTop.GetComponent<Waypoint>()))
                {
                    waypoint.leftTop.SetActive(false);
                }
            }
            if (waypoint.rightTop)
            {
                if (controlArea.Contains(waypoint.rightTop.GetComponent<Waypoint>()))
                {
                    waypoint.rightTop.SetActive(false);
                }
            }
            if (waypoint.leftBot)
            {
                if (controlArea.Contains(waypoint.leftBot.GetComponent<Waypoint>()))
                {
                    waypoint.leftBot.SetActive(false);
                }
            }
            if (waypoint.rightBot)
            {
                if (controlArea.Contains(waypoint.rightBot.GetComponent<Waypoint>()))
                {
                    waypoint.rightBot.SetActive(false);
                }
            }
            
        }
    }

    public void ClearFrontiers(Waypoint waypoint)
    {
        if (waypoint.left)
        {
            if (controlArea.Contains(waypoint.left.GetComponent<Waypoint>()))
            {
                waypoint.left.SetActive(false);
            }
        }
        if (waypoint.right)
        {
            if (controlArea.Contains(waypoint.right.GetComponent<Waypoint>()))
            {
                waypoint.right.SetActive(false);
            }
        }
        if (waypoint.leftTop)
        {
            if (controlArea.Contains(waypoint.leftTop.GetComponent<Waypoint>()))
            {
                waypoint.leftTop.SetActive(false);
            }
        }
        if (waypoint.rightTop)
        {
            if (controlArea.Contains(waypoint.rightTop.GetComponent<Waypoint>()))
            {
                waypoint.rightTop.SetActive(false);
            }
        }
        if (waypoint.leftBot)
        {
            if (controlArea.Contains(waypoint.leftBot.GetComponent<Waypoint>()))
            {
                waypoint.leftBot.SetActive(false);
            }
        }
        if (waypoint.rightBot)
        {
            if (controlArea.Contains(waypoint.rightBot.GetComponent<Waypoint>()))
            {
                waypoint.rightBot.SetActive(false);
            }
        }
    }
    
}

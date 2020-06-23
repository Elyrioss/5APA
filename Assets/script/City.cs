using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class City
{
    
    public int population;
    public Waypoint position;
    public Color civColor;
    public List<Waypoint> controlArea= new List<Waypoint>();

    public float food = 0;
    public float production = 0;
    public float gold = 0;
    
    public Construction construction = new Construction();

    //Construction Var
    public bool ThisCityAction; // Empeche le joueur d'effectuer plus d'une action sur la ville par tour.
       
    public City(Waypoint pos,Color color)
    {

        civColor = color;
        position = pos;
        position.CivColor = civColor;
        position.EnableWaypoint();
        controlArea.Add(position);
        
        food += position.Food;
        production += position.Production;
        gold += position.Gold;
        
        foreach (Waypoint w in position.Neighbors)
        {
            controlArea.Add(w);
            w.CivColor = civColor;
            w.EnableWaypoint();
            food += w.Food;
            production += w.Production;
            gold += w.Gold;
        }
        ClearFrontiers();
        population = 1;
        
    }

    public bool ExtendTown(Waypoint w)
    {
        if (!controlArea.Contains(w))
        {
            controlArea.Add(w);
            w.CivColor = civColor;
            w.EnableWaypoint();
            food += w.Food;
            production += w.Production;
            gold += w.Gold;
            ClearFrontiers(w);
            w.EnableWaypoint();
            return true;
        }

        return false;
    }

    // left , leftbot , lefttop, right , rightbot, rightop
    
    public void ClearFrontiers()
    {
        foreach (Waypoint waypoint in controlArea)
        {
            if (waypoint.left)
            {
                if (controlArea.Contains(waypoint.left.GetComponent<Waypoint>()))
                {
                    waypoint.spriteRenderer[0].gameObject.SetActive(false);
                }
            }
            if (waypoint.right)
            {
                if (controlArea.Contains(waypoint.right.GetComponent<Waypoint>()))
                {
                    waypoint.spriteRenderer[3].gameObject.SetActive(false);
                }
            }
            if (waypoint.leftTop)
            {
                if (controlArea.Contains(waypoint.leftTop.GetComponent<Waypoint>()))
                {
                    waypoint.spriteRenderer[2].gameObject.SetActive(false);
                }
            }
            if (waypoint.rightTop)
            {
                if (controlArea.Contains(waypoint.rightTop.GetComponent<Waypoint>()))
                {
                    waypoint.spriteRenderer[5].gameObject.SetActive(false);
                }
            }
            if (waypoint.leftBot)
            {
                if (controlArea.Contains(waypoint.leftBot.GetComponent<Waypoint>()))
                {
                    waypoint.spriteRenderer[1].gameObject.SetActive(false);
                }
            }
            if (waypoint.rightBot)
            {
                if (controlArea.Contains(waypoint.rightBot.GetComponent<Waypoint>()))
                {
                    waypoint.spriteRenderer[4].gameObject.SetActive(false);
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
                waypoint.spriteRenderer[0].gameObject.SetActive(false);
            }
        }
        if (waypoint.right)
        {
            if (controlArea.Contains(waypoint.right.GetComponent<Waypoint>()))
            {
                waypoint.spriteRenderer[3].gameObject.SetActive(false);
            }
        }
        if (waypoint.leftTop)
        {
            if (controlArea.Contains(waypoint.leftTop.GetComponent<Waypoint>()))
            {
                waypoint.spriteRenderer[2].gameObject.SetActive(false);
            }
        }
        if (waypoint.rightTop)
        {
            if (controlArea.Contains(waypoint.rightTop.GetComponent<Waypoint>()))
            {
                waypoint.spriteRenderer[5].gameObject.SetActive(false);
            }
        }
        if (waypoint.leftBot)
        {
            if (controlArea.Contains(waypoint.leftBot.GetComponent<Waypoint>()))
            {
                waypoint.spriteRenderer[1].gameObject.SetActive(false);
            }
        }
        if (waypoint.rightBot)
        {
            if (controlArea.Contains(waypoint.rightBot.GetComponent<Waypoint>()))
            {
                waypoint.spriteRenderer[4].gameObject.SetActive(false);
            }
        }
    }



    public void EndOfTurn()
    {
        //On récupère les resources naturelles
        food += position.Food;
        production += position.Production;
        gold += position.Gold;
        foreach (Waypoint w in position.Neighbors)
        {
            Waypoint W = w.GetComponent<Waypoint>();
            controlArea.Add(W);
            W.CivColor = civColor;
            W.EnableWaypoint();
            food += W.Food;
            production += W.Production;
            gold += W.Gold;
        }
        //Resources Batiments interieurs
        if (construction.buildings != null)
        {
            foreach (Buildings b in construction.buildings)
            {
                if (b.BuildType == Buildings.BuildingType.Ressource)
                {
                    switch (b.RessourceType)
                    {
                        case 0:
                            break;
                        case 1:
                            food += 50;
                            break;
                        case 2:
                            production += 50;
                            break;
                        case 3:
                            gold += 50;
                            break;
                    }
                }
            }
        }

        //On regarde la file de construction de la ville.
        construction.ConstructionProcess();
        ThisCityAction = false;
    }
    
}

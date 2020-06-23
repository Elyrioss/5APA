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
    public List<Waypoint> controlAreaClone = new List<Waypoint>();

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
        //TWIN
        if (position.AsTwin || position.IsTwin)
        {
            position.Twin.CivColor = civColor;
            position.Twin.EnableWaypoint();
            controlAreaClone.Add(position.Twin);
        }
        //

        food += position.Food;
        production += position.Production;
        gold += position.Gold;
        
        foreach (Waypoint w in position.Neighbors)
        {
            controlArea.Add(w);
            w.CivColor = civColor;
            w.EnableWaypoint();
            //TWIN
            if (w.AsTwin || w.IsTwin)
            {
                w.Twin.CivColor = civColor;
                w.Twin.EnableWaypoint();
                controlAreaClone.Add(w.Twin);
            }
            //
            food += w.Food;
            production += w.Production;
            gold += w.Gold;
        }
        ClearFrontiers();
        ClearFrontiersClone();
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

    //

    public bool ExtendTownClone(Waypoint w)
    {
        if (!controlAreaClone.Contains(w))
        {
            controlAreaClone.Add(w);
            w.CivColor = civColor;
            w.EnableWaypoint();
            //ClearFrontiersClone(w);
            w.EnableWaypoint();
            return true;
        }

        return false;
    }

    // left , leftbot , lefttop, right , rightbot, rightop

    public void ClearFrontiersClone()
    {
        foreach (Waypoint waypoint in controlAreaClone)
        {
            if (waypoint.left)
            {
                if (controlAreaClone.Contains(waypoint.left.GetComponent<Waypoint>()))
                {
                    waypoint.spriteRenderer[0].gameObject.SetActive(false);
                }
            }
            if (waypoint.right)
            {
                if (controlAreaClone.Contains(waypoint.right.GetComponent<Waypoint>()))
                {
                    waypoint.spriteRenderer[3].gameObject.SetActive(false);
                }
            }
            if (waypoint.leftTop)
            {
                if (controlAreaClone.Contains(waypoint.leftTop.GetComponent<Waypoint>()))
                {
                    waypoint.spriteRenderer[2].gameObject.SetActive(false);
                }
            }
            if (waypoint.rightTop)
            {
                if (controlAreaClone.Contains(waypoint.rightTop.GetComponent<Waypoint>()))
                {
                    waypoint.spriteRenderer[5].gameObject.SetActive(false);
                }
            }
            if (waypoint.leftBot)
            {
                if (controlAreaClone.Contains(waypoint.leftBot.GetComponent<Waypoint>()))
                {
                    waypoint.spriteRenderer[1].gameObject.SetActive(false);
                }
            }
            if (waypoint.rightBot)
            {
                if (controlAreaClone.Contains(waypoint.rightBot.GetComponent<Waypoint>()))
                {
                    waypoint.spriteRenderer[4].gameObject.SetActive(false);
                }
            }

        }
    }

    public void ClearFrontiersClone(Waypoint waypoint)
    {
        if (waypoint.left)
        {
            if (controlAreaClone.Contains(waypoint.left.GetComponent<Waypoint>()))
            {
                waypoint.spriteRenderer[0].gameObject.SetActive(false);
            }
        }
        if (waypoint.right)
        {
            if (controlAreaClone.Contains(waypoint.right.GetComponent<Waypoint>()))
            {
                waypoint.spriteRenderer[3].gameObject.SetActive(false);
            }
        }
        if (waypoint.leftTop)
        {
            if (controlAreaClone.Contains(waypoint.leftTop.GetComponent<Waypoint>()))
            {
                waypoint.spriteRenderer[2].gameObject.SetActive(false);
            }
        }
        if (waypoint.rightTop)
        {
            if (controlAreaClone.Contains(waypoint.rightTop.GetComponent<Waypoint>()))
            {
                waypoint.spriteRenderer[5].gameObject.SetActive(false);
            }
        }
        if (waypoint.leftBot)
        {
            if (controlAreaClone.Contains(waypoint.leftBot.GetComponent<Waypoint>()))
            {
                waypoint.spriteRenderer[1].gameObject.SetActive(false);
            }
        }
        if (waypoint.rightBot)
        {
            if (controlAreaClone.Contains(waypoint.rightBot.GetComponent<Waypoint>()))
            {
                waypoint.spriteRenderer[4].gameObject.SetActive(false);
            }
        }
    }





    //

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
        ConstructionProcess();
        ThisCityAction = false;
    }



    public void ConstructionProcess()
    {
        if (construction.FoodConstruction)
        {
            construction.FoodCounter--;
            if (construction.FoodCounter == 0)
            {
                construction.buildings.Add(new Buildings(Buildings.BuildingType.Ressource, 1));
                construction.NumberOfFoodBat++;
                construction.FoodCounter = 2;
                construction.FoodConstruction = false;
            }
        }

        if (construction.ProducConstruction)
        {
            construction.ProducCounter--;
            if (construction.ProducCounter == 0)
            {
                construction.buildings.Add(new Buildings(Buildings.BuildingType.Ressource, 2));
                construction.NumberOfProducBat++;
                construction.ProducCounter = 2;
                construction.ProducConstruction = false;
            }
        }

        if (construction.GoldConstruction)
        {
            construction.GoldCounter--;
            if (construction.GoldCounter == 0)
            {
                construction.buildings.Add(new Buildings(Buildings.BuildingType.Ressource, 3));
                construction.NumberOfGoldBat++;
                construction.GoldCounter = 2;
                construction.GoldConstruction = false;
            }
        }

        if (construction.ExtensionConstruction)
        {
            construction.ExtensionCounter--;
            if (construction.ExtensionCounter == 0)
            {
                construction.buildings.Add(new Buildings(Buildings.BuildingType.Extension));
                construction.NumberOfExtension++;
                construction.ExtensionCounter = 2;
                construction.ExtensionConstruction = false;
                foreach (Waypoint w in construction.ExtensionWaypoint.Neighbors)
                {
                    controlArea.Add(w);
                    w.CivColor = civColor;
                    w.EnableWaypoint();
                    //TWIN
                    if (w.AsTwin || w.IsTwin)
                    {
                        w.Twin.CivColor = civColor;
                        w.Twin.EnableWaypoint();
                        controlAreaClone.Add(w.Twin);
                    }
                    //
                    food += w.Food;
                    production += w.Production;
                    gold += w.Gold;
                }
                ClearFrontiers();
                ClearFrontiersClone();
            }
        }
    }


}

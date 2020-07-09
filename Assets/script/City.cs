using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class City
{

    public string NameCity;
    public AudioSource clickSound;
    public AudioSource buildSound;
    public AudioSource soldierSound;

    public int population;
    public Waypoint position;
    public Color civColor;
    public List<Waypoint> controlArea= new List<Waypoint>();
    public List<Waypoint> controlAreaClone = new List<Waypoint>();

    public float food = 0;
    public float production = 0;
    public float gold = 0;

    public float StockFood=0;

    public Civilisation civ;
    public Construction construction = null;
    public List<Construction> Buildings=new List<Construction>();
    public List<Construction> Extensions=new List<Construction>();

    public float currentCost=0;
    public float FoodMultiplier = 1;
    public int CanExtend;
    
    //Construction Var
    public bool ThisCityAction; // Empeche le joueur d'effectuer plus d'une action sur la ville par tour.
       
    public City(Waypoint pos,SavedCity city)
    {
        string name = GameController.instance.Names[Random.Range(0, GameController.instance.Names.Count)];
        GameController.instance.Names.Remove(name);
        NameCity = name;
        FoodMultiplier = 1;
        CanExtend = 0;
        civColor = new Color(city.r,city.g,city.b);
        position = pos;
        position.CivColor = civColor;
        position.EnableWaypoint();
        controlArea.Add(position);
        position.UsedTile = true;
        position.Controled = true;
        //TWIN
        if (position.AsTwin || position.IsTwin)
        {
            position.Twin.CivColor = civColor;
            position.Twin.EnableWaypoint();
            controlAreaClone.Add(position.Twin);
            position.Twin.UsedTile = true;
            position.Twin.Controled = true;
        }
        //

        food = city.food;
        production += city.production;
        gold += city.gold;
        
        foreach (Waypoint w in position.Neighbors)
        {
            if(w.Controled)
                continue;
            
            controlArea.Add(w);
            w.CivColor = civColor;
            w.EnableWaypoint();
            w.Controled = true;
            //TWIN
            if (w.AsTwin || w.IsTwin)
            {
                w.Twin.CivColor = civColor;
                w.Twin.EnableWaypoint();
                w.Twin.Controled = true;
                controlAreaClone.Add(w.Twin);
            }
        }
        ClearFrontiers();
        ClearFrontiersClone();
        population = city.population;
        
    }
    
    public City(Waypoint pos,Color color)
    {

        string name = GameController.instance.Names[Random.Range(0, GameController.instance.Names.Count)];
        GameController.instance.Names.Remove(name);
        NameCity = name;
        FoodMultiplier = 1;
        civColor = color;
        CanExtend = 0;
        position = pos;
        position.CivColor = civColor;
        position.EnableWaypoint();
        controlArea.Add(position);
        position.UsedTile = true;
        position.Controled = true;
        //TWIN
        if (position.AsTwin || position.IsTwin)
        {
            position.Twin.CivColor = civColor;
            position.Twin.EnableWaypoint();
            controlAreaClone.Add(position.Twin);
            position.Twin.UsedTile = true;
            position.Twin.Controled = true;
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

    public void ShowAvailable(Construction c)
    {
        foreach (Waypoint w in controlArea)
        {
            if(!w.UsedTile && c.CheckForConditions(w))
                w.EnableHighlight();
        }
        
        foreach (Waypoint w in controlAreaClone)
        {
            if(!w.UsedTile && c.CheckForConditions(w))
                w.EnableHighlight();
        }
        
    }
    
    public void HideAvailable()
    {
        foreach (Waypoint w in controlArea)
        {
            w.DisableHighlight();
        }
        
        foreach (Waypoint w in controlAreaClone)
        {
            w.DisableHighlight();
        }
        
    }
    
    // left , leftbot , lefttop, right , rightbot, rightop
    
    public void ClearFrontiers()
    {
        foreach (Waypoint waypoint in controlArea)
        {
            if (waypoint.left)
            {
                if (controlArea.Contains(waypoint.left))
                {
                    waypoint.spriteRenderer[0].gameObject.SetActive(false);
                    if (waypoint.Twin)
                    {
                        waypoint.Twin.spriteRenderer[0].gameObject.SetActive(false);
                    }
                }
                
            }
            if (waypoint.right)
            {
                if (controlArea.Contains(waypoint.right))
                {
                    waypoint.spriteRenderer[3].gameObject.SetActive(false);
                    if (waypoint.Twin)
                    {
                        waypoint.Twin.spriteRenderer[3].gameObject.SetActive(false);
                    }
                }
            }
            if (waypoint.leftTop)
            {
                if (controlArea.Contains(waypoint.leftTop))
                {
                    waypoint.spriteRenderer[2].gameObject.SetActive(false);
                    if (waypoint.Twin)
                    {
                        waypoint.Twin.spriteRenderer[2].gameObject.SetActive(false);
                    }
                }
            }
            if (waypoint.rightTop)
            {
                if (controlArea.Contains(waypoint.rightTop))
                {
                    waypoint.spriteRenderer[5].gameObject.SetActive(false);
                    if (waypoint.Twin)
                    {
                        waypoint.Twin.spriteRenderer[5].gameObject.SetActive(false);
                    }
                }
            }
            if (waypoint.leftBot)
            {
                if (controlArea.Contains(waypoint.leftBot))
                {
                    waypoint.spriteRenderer[1].gameObject.SetActive(false);
                    if (waypoint.Twin)
                    {
                        waypoint.Twin.spriteRenderer[1].gameObject.SetActive(false);
                    }
                }
            }
            if (waypoint.rightBot)
            {
                if (controlArea.Contains(waypoint.rightBot))
                {
                    waypoint.spriteRenderer[4].gameObject.SetActive(false);
                    if (waypoint.Twin)
                    {
                        waypoint.Twin.spriteRenderer[4].gameObject.SetActive(false);
                    }
                }
            }
            
        }
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


    //

    public void EndOfTurn()
    {
        //On récupère les resources naturelles => LES VALEURS INDIQUE LA QUANTITE PAR TOUR 
        StockFood += food;
        if (StockFood > 30 * FoodMultiplier)
        {
            Debug.Log(StockFood+" for "+30*FoodMultiplier);
            population++;
            StockFood = 0;
            FoodMultiplier = FoodMultiplier * 1.5f;
            if (population % 2 == 0)
            {
                CanExtend ++;
            }
        }

        
        civ.Gold += gold + population;
        //On regarde la file de construction de la ville.
        ConstructionProcess();
        ThisCityAction = false;       
    }



    public void ConstructionProcess()
    {

        if (construction!=null)
        {
            currentCost = currentCost - production;
            if (currentCost <= 0)
            {
                //La construction est finis
                
                construction.ConstructionFinished(this);
                construction = null;
            }
        }
        
        
    }

    public void StartConstruction(Construction c)
    {

        currentCost = c.cost;
        construction = c;

    }

    public bool Contains(Construction build)
    {
        foreach (Construction b in Buildings)
        {
            if (b.index == build.index)
                return true;
        }

        return false;
    }
    
    public Construction Contains(string index)
    {
        foreach (Construction b in Buildings)
        {
            if (b.index == index)
            {
                return b;
            }                
        }

        return null;
    }
    
}

public class SavedCity
{

    public int population;
    public int X;
    public int Y;
    
    public float r;
    public float b;
    public float g;

    public float food = 0;
    public float production = 0;
    public float gold = 0;
    
    public SavedCity(City c)
    {
        food = c.food;
        production = c.production;
        gold = c.gold;

        X = c.position.X;
        Y = c.position.Y;
        population = c.population;

        r = c.civColor.r;
        b = c.civColor.b;
        g = c.civColor.g;
        
        
    }
}

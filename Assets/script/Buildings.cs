using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Construction
{

    public float cost=0;
    public string index;
    public Waypoint Position;        
    public GameObject prefab;
    public GameObject Twin;    
    public virtual void ConstructionFinished(City c){}

    
}



public class Buildings : Construction
{
    
    public BuildingType BuildType;
    
    
    public enum BuildingType
    {
        Ressource,
        Wonder,
        Extension
    }    

}

public class Grenier : Buildings
{
  
    public Grenier()
    {
        index = "Grenier";
        BuildType = BuildingType.Ressource;
        cost=50;
    }
    
    public override void ConstructionFinished(City c)
    {
        c.food += 10;
        c.Buildings.Add(new Grenier());
        c.buildSound.PlayOneShot(c.buildSound.clip);
    }
}

public class Usine : Buildings
{

    public Usine()
    {
        index = "Usine";
        BuildType = BuildingType.Ressource;
        cost=50;
    }
    
    public override void ConstructionFinished(City c)
    {
        c.production += 10;
        c.Buildings.Add(new Usine());
        c.buildSound.PlayOneShot(c.buildSound.clip);
    }
}

public class Marcher : Buildings
{

    
    public Marcher()
    {
        index = "Marcher";
        BuildType = BuildingType.Ressource;
        cost=50;
    }
    
    public override void ConstructionFinished(City c)
    {
        c.gold += 10;
        c.Buildings.Add(new Marcher());
        c.buildSound.PlayOneShot(c.buildSound.clip);
    }
}

public class Extension : Buildings
{

    public Extension()
    {
        index = "Extension";
        BuildType = BuildingType.Extension;
        cost=100;    
    }
    
    public override void ConstructionFinished(City c)
    {       
        
        GameObject ExtObj = GameObject.Instantiate(Resources.Load("Prefabs/"+index) as GameObject, Position.transform);
        ExtObj.transform.localPosition = new Vector3(0, Position.elevation, 0);
        prefab = ExtObj;        
        //TWIN
        if (Position.AsTwin || Position.IsTwin)
        {
            GameObject ExtObjC = GameObject.Instantiate(Resources.Load("Prefabs/"+index) as GameObject,Position.Twin.transform);            
            ExtObjC.transform.localPosition = new Vector3(0, Position.elevation, 0);
            Twin = ExtObjC;
        }
        
        foreach (Waypoint w in Position.Neighbors)
        {
            if(c.controlArea.Contains(w))
                continue;
            
            c.controlArea.Add(w);
            w.CivColor = c.civColor;
            w.EnableWaypoint();
            
            //TWIN
            
            if (w.AsTwin || w.IsTwin)
            {
                w.Twin.CivColor = c.civColor;
                w.Twin.EnableWaypoint();
                c.controlAreaClone.Add(w.Twin);
            }
            //
            c.food += w.Food;
            c.production += w.Production;
            c.gold += w.Gold;
        }
        c.ClearFrontiers();
        c.ClearFrontiersClone();
        c.Buildings.Add(new Extension());
        c.buildSound.PlayOneShot(c.buildSound.clip);
    }
}
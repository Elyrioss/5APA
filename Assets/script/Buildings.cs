using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Construction
{

    public float cost=0;
    public string index;
        
    public virtual void ConstructionFinished(City c){}

}



public class Buildings : Construction
{
    public string index;
    public string image;
    public BuildingType BuildType;
    public Waypoint target;    
    
    public enum BuildingType
    {
        Ressource,
        Wonder,
        Extension
    }    

}

public class Grenier : Buildings
{
    public string index = "Grenier";
    public string image = "Grenier.png";
    private BuildingType BuildType = BuildingType.Ressource;
    
    public override void ConstructionFinished(City c)
    {
        c.food += 10;
        c.Buildings.Add(new Grenier());
        c.buildSound.PlayOneShot(c.buildSound.clip);
    }
}

public class Usine : Buildings
{
    public string index = "Usine";
    public string image = "Usine.png";
    private BuildingType BuildType = BuildingType.Ressource;

    
    public override void ConstructionFinished(City c)
    {
        c.production += 10;
        c.Buildings.Add(new Usine());
        c.buildSound.PlayOneShot(c.buildSound.clip);
    }
}

public class Marcher : Buildings
{
    public string index = "Marcher";
    public string image = "Marcher.png";
    private BuildingType BuildType = BuildingType.Ressource;
    
    public override void ConstructionFinished(City c)
    {
        c.gold += 10;
        c.Buildings.Add(new Marcher());
        c.buildSound.PlayOneShot(c.buildSound.clip);
    }
}

public class Extension : Buildings
{
    public string index = "Extension";
    public string image = "Extension.png";
    private BuildingType BuildType = BuildingType.Extension;
    
    public override void ConstructionFinished(City c)
    {       
        foreach (Waypoint w in target.Neighbors)
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
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class Construction
{

    public float cost=0;
    public string index;
    public Waypoint Position;        
    public GameObject prefab;
    public GameObject Twin;
    public float Tempcost;
    public bool Redoable;
    public BuildingType BuildType;
      
    public enum BuildingType
    {
        Ressource,
        Wonder,
        Extension,
        Unit
    } 
    
    public virtual void ConstructionFinished(City c){}
  
}



public class Buildings : Construction
{
    
  // ON SAIS JAMAIS 

}

public class Grenier : Buildings
{
  
    public Grenier()
    {
        index = "Grenier";
        BuildType = BuildingType.Ressource;
        cost=50;
        Tempcost = -1;
    }
    
    public override void ConstructionFinished(City c)
    {
        c.food += 10;
        c.Buildings.Add(this);
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
        Tempcost = -1;
    }
    
    public override void ConstructionFinished(City c)
    {
        c.production += 10;
        c.Buildings.Add(this);
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
        Tempcost = -1;
    }
    
    public override void ConstructionFinished(City c)
    {
        c.gold += 10;
        c.Buildings.Add(this);
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
        Tempcost = -1;
        Redoable = true;
    }
    
    public override void ConstructionFinished(City c)
    {       
        GameObject.DestroyImmediate(prefab);
        
        Buildings b = c.Contains(index);
        c.Buildings.Remove(b);
        c.Extensions.Add(b);
        prefab = GameObject.Instantiate(Resources.Load("Prefabs/"+index) as GameObject, Position.transform);
        prefab.transform.localPosition = new Vector3(0, Position.elevation, 0);

        if (Position.LOD)
        {
            Position.LOD.SetActive(false);
        }
        //TWIN
        if (Position.AsTwin || Position.IsTwin)
        {
            GameObject.DestroyImmediate(Twin);
            Twin = GameObject.Instantiate(Resources.Load("Prefabs/"+index) as GameObject,Position.Twin.transform);            
            Twin.transform.localPosition = new Vector3(0, Position.elevation, 0);
            if (Position.Twin.LOD)
            {
                Position.Twin.LOD.SetActive(false);
            }
        }
        
        foreach (Waypoint w in Position.Neighbors)
        {
            if(w.Controled)
                continue;

            w.Controled = true;
            c.controlArea.Add(w);
            w.CivColor = c.civColor;
            w.EnableWaypoint();
            
            //TWIN
            
            if (w.AsTwin || w.IsTwin)
            {
                w.Twin.CivColor = c.civColor;
                w.Twin.EnableWaypoint();
                w.Twin.Controled = true;
                c.controlAreaClone.Add(w.Twin);
            }
            //
            c.food += w.Food;
            c.production += w.Production;
            c.gold += w.Gold;
        }
        c.ClearFrontiers();
        c.ClearFrontiersClone();
        c.buildSound.PlayOneShot(c.buildSound.clip);
    }
}
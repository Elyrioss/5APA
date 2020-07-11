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

    public virtual Construction Copy()
    {
        return null;} 
    public enum BuildingType
    {
        Ressource,
        Wonder,
        Extension,
        Unit
    } 
    
    public virtual void ConstructionFinished(City c){}

    public virtual bool CheckForConditions(Waypoint w)
    {
        return true;
    }

    public virtual void VisualSteps()
    {
    }
}



public class Buildings : Construction
{


    public void ConstructionFinishedInstantiate(City c)
    {
        GameObject.DestroyImmediate(prefab);

        Construction b = c.construction;
        c.Extensions.Add(b);
        prefab = GameObject.Instantiate(Resources.Load("Prefabs/" + index) as GameObject, Position.transform);
        prefab.transform.localPosition = new Vector3(0, Position.elevation, 0);

        if (Position.LOD)
        {
            Position.LOD.SetActive(false);
        }

        //TWIN
        if (Position.AsTwin || Position.IsTwin)
        {
            GameObject.DestroyImmediate(Twin);
            Twin = GameObject.Instantiate(Resources.Load("Prefabs/" + index) as GameObject, Position.Twin.transform);
            Twin.transform.localPosition = new Vector3(0, Position.elevation, 0);
            if (Position.Twin.LOD)
            {
                Position.Twin.LOD.SetActive(false);
            }
        }

        foreach (Waypoint w in Position.Neighbors)
        {
            if (w.Controled)
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
        VisualSteps();
    }

}

public class Grenier : Construction
{
    public override Construction Copy()
    {      
        Grenier g = new Grenier();
        g.Tempcost = Tempcost;      
        return g;
    }

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

public class Usine : Construction
{
    public override Construction Copy()
    {
        Usine g = new Usine();
        g.Tempcost = Tempcost;      
        return g;
    }

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

public class Marcher : Construction
{

    
    public override Construction Copy()
    {
        Marcher g = new Marcher();
        g.Tempcost = Tempcost;      
        return g;
    }
    
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
    
    public override Construction Copy()
    {
        Extension g = new Extension();
        g.Tempcost = Tempcost;
        g.Position = Position;
        
        return g;
    }
    public Extension()
    {
        index = "Extension";
        BuildType = BuildingType.Extension;
        cost = 100;
        Tempcost = -1;
        Redoable = true;
    }

    public override void ConstructionFinished(City c)
    {
        c.CanExtend--;
        ConstructionFinishedInstantiate(c);
    }

    public override bool CheckForConditions(Waypoint w)
    {
        if (w.HeightType == HeightType.River || w.HeightType == HeightType.DeepWater ||
            w.HeightType == HeightType.ShallowWater)
            return false;
        return true;
    }

    public override void VisualSteps()
    {
        
    }
}

public class Port : Buildings
{
    public Port()
    {
        index = "Port";
        BuildType = BuildingType.Extension;
        cost=150;    
        Tempcost = -1;
    }
    
    public override Construction Copy()
    {
        Port g = new Port();
        g.Tempcost = Tempcost;
        g.Position = Position;
        
        return g;
    }

    public override bool CheckForConditions(Waypoint w)
    {
        if (w.HeightType == HeightType.River || w.HeightType == HeightType.ShallowWater)
            return true;
        return false;
    }

    public override void ConstructionFinished(City c)
    {
        c.gold += 10;
        c.civ.BoatDiscovered = true;
        ConstructionFinishedInstantiate(c);
    }

    public override void VisualSteps()
    {
        if (Position.leftTop.UsedTile)
        {
            prefab.transform.Rotate(0,60,0);
            if(Twin)
                Twin.transform.Rotate(0,60,0);
        }          
        else if (Position.rightTop.UsedTile)
        {
            prefab.transform.Rotate(0,120,0);
            if(Twin)
                Twin.transform.Rotate(0,120,0);
        }
        if (Position.right.UsedTile)
        {
            prefab.transform.Rotate(0,180,0);
            if(Twin)
                Twin.transform.Rotate(0,180,0);

        }          
        else if (Position.rightBot.UsedTile)
        {
            prefab.transform.Rotate(0,240,0);
            if(Twin)
                Twin.transform.Rotate(0,240,0);
        }
        else if (Position.leftBot.UsedTile)
        {
            prefab.transform.Rotate(0,300,0);
            if(Twin)
                Twin.transform.Rotate(0,300,0);

        }
        GameController.instance.ChangeMat(prefab,GameController.instance.CurrentCiv.MAT);
        if (Twin)
        {
            GameController.instance.ChangeMat(Twin,GameController.instance.CurrentCiv.MAT);
        }
    }
}
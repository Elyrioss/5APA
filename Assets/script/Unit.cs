using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class Unit : Construction
{
    public bool AsPlayed;
    public int mouvementPoints;
    public int HP;
    public int MAXHP;
    public Unit Instantiation(Waypoint w)
    {
        Position = w;
        prefab = Object.Instantiate(Resources.Load("Prefabs/"+index) as GameObject,Position.transform);
        prefab.transform.localPosition = new Vector3(0, Position.elevation, 0);   
        prefab.GetComponent<ManageUnit>().Unit = this;            
        Position.Occupied = true;

        if (Position.AsTwin || Position.IsTwin)
        {
            Twin = Object.Instantiate(Resources.Load("Prefabs/"+index) as GameObject,Position.Twin.transform);        
            Twin.transform.localPosition = new Vector3(0, Position.elevation, 0);    
            Twin.GetComponent<ManageUnit>().Unit = this;
            Position.Twin.Occupied = true;
        }
        else
        {
            Twin = Object.Instantiate(Resources.Load("Prefabs/"+index) as GameObject,Position.transform);        
            Twin.transform.localPosition = new Vector3(0, Position.elevation, 0);    
            Twin.GetComponent<ManageUnit>().Unit = this;
            Twin.SetActive(false);
        }
        return this;
    }
    
    public virtual void UnitPower(){}
}

public class Warrior : Unit
{
    public Warrior()
    {
        index = "Warrior";
        cost=20;
        mouvementPoints = 8;
        AsPlayed = true;
        BuildType = BuildingType.Unit;
        HP = 20;
        MAXHP = 20;
    }

    public override void ConstructionFinished(City c)
    {
        Warrior w = new Warrior();
        w.Instantiation(c.position);
        c.civ.Units.Add(w);
        c.soldierSound.PlayOneShot(c.soldierSound.clip);
    }
    
    public override void UnitPower()
    {
        Debug.Log("SCOOPBIPOUGABOUGADOO");
    }
}

public class Archer : Unit
{
    public Archer()
    {
        index = "Archer";
        cost=15;
        mouvementPoints = 8;
        AsPlayed = true;
        BuildType = BuildingType.Unit;
        HP = 12;
        MAXHP = 12;
    }
    
    public override void ConstructionFinished(City c)
    {
        Archer w = new Archer();
        w.Instantiation(c.position);
        c.civ.Units.Add(w);
        c.soldierSound.PlayOneShot(c.soldierSound.clip);
    }
    
    public override void UnitPower()
    {
        Debug.Log("SCOOPBIPOUGABOUGADOO");
    }
}

public class Rider : Unit
{
    public Rider()
    {
        index = "Rider";
        cost=25;
        mouvementPoints = 15;
        AsPlayed = true;
        BuildType = BuildingType.Unit;
        HP = 15;
        MAXHP = 15;
    }
    
    public override void ConstructionFinished(City c)
    {
        Rider w = new Rider();
        w.Instantiation(c.position);
        c.civ.Units.Add(w);
        c.soldierSound.PlayOneShot(c.soldierSound.clip);
    }

    public override void UnitPower()
    {
        Debug.Log("SCOOPBIPOUGABOUGADOO");
    }
}

public class Colon : Unit
{
    public Colon()
    {
        index = "Colon";
        cost=100;
        mouvementPoints = 8;
        AsPlayed = true;
        BuildType = BuildingType.Unit;
        HP = 5;
        MAXHP = 5;
    }
    
    public override void ConstructionFinished(City c)
    {
        Colon w = new Colon();
        w.Instantiation(c.position);
        c.civ.Units.Add(w);
        c.soldierSound.PlayOneShot(c.soldierSound.clip);
    }
    
    public override void UnitPower()
    {
        if(Position.Controled)
            return;
        
        GameController GC = GameController.instance;
        
        GC.MapControllerScript.CreateCity(Position);
        
        GameObject.DestroyImmediate(prefab);
        GameObject.DestroyImmediate(Twin);

        GC.PlayerCiv.Units.Remove(this);

    }
}
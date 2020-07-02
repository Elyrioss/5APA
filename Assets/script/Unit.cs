using UnityEngine;

public class Unit : Construction
{
    public bool AsPlayed;
    public int mouvementPoints;
    

    public Unit Instantiation(Waypoint w)
    {
        Position = w;
        prefab = Object.Instantiate(Resources.Load("Prefabs/"+index) as GameObject,Position.transform);
        prefab.transform.localPosition = new Vector3(0, Position.elevation, 0);   
        prefab.GetComponent<ManageUnit>().Unit = this;
        if (Position.AsTwin || Position.IsTwin)
        {
            Twin = Object.Instantiate(Resources.Load("Prefabs/"+index) as GameObject,Position.Twin.transform);        
            Twin.transform.localPosition = new Vector3(0, Position.elevation, 0);    
            Twin.GetComponent<ManageUnit>().Unit = this;
        }
        return this;
    }
}

public class Warrior : Unit
{
    public Warrior()
    {
        index = "Warrior";
        cost=20;
        mouvementPoints = 3;
        AsPlayed = false;
    }

    public override void ConstructionFinished(City c)
    {
        Warrior w = new Warrior();
        w.Instantiation(c.position);
        c.Units.Add(w);
        c.soldierSound.PlayOneShot(c.soldierSound.clip);
    }
}

public class Archer : Unit
{
    public Archer()
    {
        index = "Archer";
        cost=15;
        mouvementPoints = 2;
        AsPlayed = false;
    }
    
    public override void ConstructionFinished(City c)
    {
        Archer w = new Archer();
        w.Instantiation(c.position);
        c.Units.Add(w);
        c.soldierSound.PlayOneShot(c.soldierSound.clip);
    }
}

public class Rider : Unit
{
    public Rider()
    {
        index = "Rider";
        cost=25;
        mouvementPoints = 5;
        AsPlayed = false;
    }
    
    public override void ConstructionFinished(City c)
    {
        Rider w = new Rider();
        w.Instantiation(c.position);
        c.Units.Add(w);
        c.soldierSound.PlayOneShot(c.soldierSound.clip);
    }
}
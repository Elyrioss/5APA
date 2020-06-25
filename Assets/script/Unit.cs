public class Unit : Construction
{

    public Waypoint Position;
    public UnitType Type;
    public bool AsPlayed;
    public enum UnitType
    {
        Warrior,
        Archer,
        Rider
    }
    public Unit(UnitType a,Waypoint w)
    {
        Type = a;
        Position = w;
    }

    public Unit()
    {
        Type = UnitType.Warrior;
    }
}

/*public class Melee : Unit
{
    
}

public class Range : Unit
{
    
}

public class Spear : Unit
{
    
}*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construction
{
    public float Cost;
}
public class Buildings : Construction
{
    public Waypoint Position;
    public GameObject Pref;
    

    public enum BuildingType
    {
        Ressource,
        Unit,
        Wonder
    }
}

public class Barracks : Buildings
{
    public Unit Swordman;
}

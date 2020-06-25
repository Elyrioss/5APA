using System;
using System.Collections.Generic;
using UnityEngine;

public class Construction
{
    //FoodBat
    public int NumberOfFoodBat = 0;
    public bool FoodConstruction;
    public int FoodCounter = 2;
    //ProductionBat
    public int NumberOfProducBat = 0;
    public bool ProducConstruction;
    public int ProducCounter = 2;
    //GoldBat
    public int NumberOfGoldBat = 0;
    public bool GoldConstruction;
    public int GoldCounter = 2;
    //Extension Bat
    public int NumberOfExtension = 0;
    public bool ExtensionConstruction;
    public int ExtensionCounter = 2;
    public Waypoint ExtensionWaypoint;

    //Warrior
    public int NumberOfWarrior = 0;
    public bool WarriorConstruction;
    public int WarriorCounter = 2;
    public Waypoint WarriorWaypoint;
    public GameObject WarriorUnit;

    //Archer
    public int NumberOfArcher = 0;
    public bool ArcherConstruction;
    public int ArcherCounter = 2;
    public Waypoint ArcherWaypoint;
    public GameObject ArcherUnit;
    //Rider
    public int NumberOfRider = 0;
    public bool RiderConstruction;
    public int RiderCounter = 2;
    public Waypoint RiderWaypoint;
    public GameObject RiderUnit;


    //Liste Buildings
    public List<Buildings> buildings = new List<Buildings>();

    //Liste Unités
    public List<Unit> Units = new List<Unit>();


    public Construction()
    {
        Debug.Log("File de construction");
    }

}




public class Buildings : Construction
{
    public Waypoint Position;
    public GameObject Pref;
    public BuildingType BuildType;
    public int RessourceType = 0; //0 = None,1=Food,2=Product,3=Gold

    public enum BuildingType
    {
        Ressource,
        Unit,
        Wonder,
        Extension
    }

    public Buildings(BuildingType a,int TypeOfRessource)
    {
        BuildType = a;
        RessourceType = TypeOfRessource;
    }

    public Buildings(BuildingType a)
    {
        BuildType = a;
        RessourceType = 0;
    }

    public Buildings()
    {
        BuildType = BuildingType.Ressource;
        RessourceType = 0;
    }

}

public class Barracks : Buildings
{
    public Unit Swordman;
}

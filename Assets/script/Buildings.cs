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

    //Liste Buildings
    public List<Buildings> buildings = new List<Buildings>();
    

    public Construction()
    {
        Debug.Log("File de construction");
    }


    public void ConstructionProcess()
    {
        if (FoodConstruction)
        {
            FoodCounter--;
            if (FoodCounter == 0)
            {
                buildings.Add(new Buildings(Buildings.BuildingType.Ressource, 1));
                NumberOfFoodBat++;
                FoodCounter = 2;
                FoodConstruction = false;
            }
        }

        if (ProducConstruction)
        {
            ProducCounter--;
            if (ProducCounter == 0)
            {
                buildings.Add(new Buildings(Buildings.BuildingType.Ressource, 2));
                NumberOfProducBat++;
                ProducCounter = 2;
                ProducConstruction = false;
            }
        }

        if (GoldConstruction)
        {
            GoldCounter--;
            if (GoldCounter == 0)
            {
                buildings.Add(new Buildings(Buildings.BuildingType.Ressource, 3));
                NumberOfGoldBat++;
                GoldCounter = 2;
                GoldConstruction = false;
            }
        }
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
        Wonder
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Civilisation
{
    public Color CivilisationColor;

    public Material MAT;
    
    public float Gold;

    public float Science;
    
    public List<Unit> Units = new List<Unit>();

    public List<City> Cities=new List<City>();

    public bool BoatDiscovered;

    public ScienceScreen ScienceScreen;
    
    public List<string> AuthorizedBuildings ;
    public List<string> AuthorizedUnits ;

    public int goldOre;
    public int ironOre;

    public bool FoodWin;
    public bool GoldWin;
    public bool WarWin;
    public bool ScienceWin;
    
    public int bonusDamage;
    public int bonusHp;
    public int bonusFood;
    public int bonusProd;
    public int bonusGold;
    public int bonusScience;
    public int bonusMouv;
    
    public Civilisation(Color civilisationColor, Material mat)
    {
        CivilisationColor = civilisationColor;
        MAT = mat;
        BoatDiscovered = false;
        Science = 0;
        AuthorizedBuildings = new List<string>(){"Grenier","Usine","Marcher","Laboratoire","Extension"};
        AuthorizedUnits = new List<string>(){"Archer","Warrior","Colon"};
        bonusDamage=0;
        bonusFood=0;
        bonusProd=0;
        bonusGold=0;
        bonusScience=0;
        bonusHp = 0;
        bonusMouv = 0;
        
        FoodWin=false;
        GoldWin=false;
        WarWin=false;
        ScienceWin=false;
        
        goldOre=0;
        ironOre = 0;

    }

    public City CreateCity(Waypoint position)
    {
        City city = new City(position,CivilisationColor);
        MAT.color = CivilisationColor;
        Cities.Add(city);
        city.food += bonusFood;
        city.production += bonusProd;
        city.gold += bonusGold;
        city.food += bonusFood;
        city.science += bonusScience;
        city.construction = new Construction();
        city.civ = this;
        Science += city.science;
        return city;
    }
}

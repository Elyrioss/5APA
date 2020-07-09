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

    public List<Unit> Units = new List<Unit>();

    public List<City> Cities=new List<City>();

    public Civilisation(Color civilisationColor, Material mat)
    {
        CivilisationColor = civilisationColor;
        MAT = mat;
    }

    public City CreateCity(Waypoint position)
    {
        City city = new City(position,CivilisationColor);
        MAT.color = CivilisationColor;
        Cities.Add(city);
        city.civ = this;
        return city;
    }
}

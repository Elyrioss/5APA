using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilisation
{
    public Color CivilisationColor;

    public float Gold;

    public List<Unit> Units = new List<Unit>();

    public List<City> Cities=new List<City>();

    public Civilisation(Color civilisationColor)
    {
        CivilisationColor = civilisationColor;
    }
    
    public City CreateCity(Waypoint position)
    {
        City city = new City(position,CivilisationColor);
        Cities.Add(city);
        city.civ = this;
        return city;
    }
}

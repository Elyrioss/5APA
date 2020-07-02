using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    
    public Buildings getBuildings(int index)
    {
        switch (index)
        {
            case -1:
                return new Extension();
            case 0:
                return new Grenier();
            case 1:
                return new Usine();
            case 2:
                return new Marcher();
        }
        return null;        
    }

    public Unit getUnits(int index)
    {
        switch (index)
        {
            case 0:
                return new Warrior();
            case 1:
                return new Archer();
            case 2:
                return new Rider();
        }
        return null;        
    }
    
    public int index=0;
    public Image icon;
    public TextMeshProUGUI name;
    public Construction build;
    public CityMenu Menue;
    
    public void SetupBuilding(CityMenu menu)
    {    
        build = getBuildings(index);
        icon.sprite = Resources.Load<Sprite>(build.index);
        name.text = build.index;
        Debug.Log(build.index);
        Menue = menu;
    }

    public void SetupUnit(CityMenu menu)
    {    
        build = getUnits(index);
        icon.sprite = Resources.Load<Sprite>(build.index);
        name.text = build.index;
        Debug.Log(build.index);
        Menue = menu;
    }

    
    public void Build()
    {
        City c = GameController.instance.SelectedCity;
        GameController.instance.SelectedCity.StartConstruction(build);
        Menue.CurrentConstructionTime.text = "" + Mathf.Ceil(c.construction.cost / c.production);
        Menue.CurrentConstructionImage.sprite = icon.sprite;
        Menue.CurrentConstructionText.text = build.index;
    }
    
    public void Extend()
    {
        Menue.HideBat();
        Menue.HideCity();
        GameController.instance.MapControllerScript.Extension = true;        
    }

    
}

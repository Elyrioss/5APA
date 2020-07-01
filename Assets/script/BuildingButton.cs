using System.Collections;
using System.Collections.Generic;
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


    public int index=0;
    public Image icon;
    public TextMeshProUGUI name;
    public Buildings build;
    public CityMenu Menue;
    
    public void Setup(CityMenu menu)
    {    
        build = getBuildings(index);
        icon.sprite = Resources.Load<Sprite>(build.image);
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
        City c = GameController.instance.SelectedCity;
        GameController.instance.MapControllerScript.Extension = true;
        Menue.CurrentConstructionTime.text = "" + Mathf.Ceil(c.construction.cost / c.production);
        Menue.CurrentConstructionImage.sprite = icon.sprite;
        Menue.CurrentConstructionText.text = build.index;
    }
    
}

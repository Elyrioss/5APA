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
    
    public void Setup()
    {    
        build = getBuildings(index);
        icon.sprite = Resources.Load<Sprite>(build.image);
        name.text = build.index;
    }

    public void Build()
    {
        GameController.instance.SelectedCity.StartConstruction(build);
    }
    
    public void Extend()
    {
        GameController.instance.MapControllerScript.Extension = true;
    }
    
}

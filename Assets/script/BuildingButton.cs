using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    
    public Image icon;
    public TextMeshProUGUI name;
    public TextMeshProUGUI Count;
    public Construction build;
    public CityMenu Menue;
    public Color Notfinished = new Color(85,215,15);
    
    public void UpdateNumbers(City c)
    {
        if (c.Buildings.Contains(build) && !build.Redoable)
        {
            gameObject.SetActive(false);
        }
        if (build.Tempcost > 0)
        {
            Count.color = Notfinished;
            Count.text= "" + Mathf.Ceil(build.Tempcost / c.production);
        }
        else
        {
            Count.text= "" + Mathf.Ceil(build.cost / c.production);
        }
    }

    public void SetupBuilding()
    {           
        icon.sprite = Resources.Load<Sprite>(build.index);
        name.text = build.index;
    }

    public void SetupUnit()
    {          
        icon.sprite = Resources.Load<Sprite>(build.index);
        name.text = build.index;
    }

    
    public void Build()
    {
        City c = GameController.instance.SelectedCity;
        if (c.construction != null)
        {
            c.construction.Tempcost = c.currentCost;
        }
        
        build.Tempcost = -1;
        GameController.instance.SelectedCity.StartConstruction(build);
        Menue.CurrentConstructionTime.text = "" + Mathf.Ceil(c.construction.cost / c.production);
        Menue.CurrentConstructionImage.sprite = icon.sprite;
        Menue.CurrentConstructionText.text = build.index;
    }
    
    public void Extend()
    {
        
        City c = GameController.instance.SelectedCity;
        if (c.construction != null)
        {
            c.construction.Tempcost = c.currentCost;
        }

        Menue.HideBat();
        Menue.HideCity();
        GameController.instance.MapControllerScript.Extension = true;
        GameController.instance.ExtentionTemp = build;
    }

    
}

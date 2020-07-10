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
        gameObject.SetActive(true);
        
        
        if (build.index == "Port")
        {
            Debug.Log(c.NameCity);
            Debug.Log((c.Contains(build) && !build.Redoable));
        }
        
        bool checker = false;
        foreach (Waypoint w in c.controlArea)
        {
            if (build.CheckForConditions(w))
            {
                checker = true;
            }
        }
        gameObject.SetActive(checker);
        
        if ((c.Contains(build) && !build.Redoable))
        {            
            gameObject.SetActive(false);
        }       
        else if (c.construction != null)
        {
            if (c.construction.index == build.index)
            {
                gameObject.SetActive(false);
            }
                
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
        
        if (build.index == "Extension")
        {
            if (c.CanExtend == 0)
            {
                gameObject.SetActive(false);
            }    
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
        
        Construction b = c.Contains(build.index);
        if (b != null)
        {
            b.cost = b.Tempcost;
            GameController.instance.SelectedCity.StartConstruction(b);
        }
        else
        {
            GameController.instance.SelectedCity.StartConstruction(build.Copy());
        }
       
        Menue.SetCurrentBuild("" + Mathf.Ceil(c.currentCost / c.production),Resources.Load<Sprite>(c.construction.index),c.construction.index);
        foreach (BuildingButton B in Menue.Buttons)
        {
            B.UpdateNumbers(c);
        }
    }
    
    public void Extend()
    {
        
        City c = GameController.instance.SelectedCity;
        
        Construction b = c.Contains(build.index);
        if (b != null && b.cost>0)
        {
            b.cost = b.Tempcost;
            
            GameController.instance.SelectedCity.StartConstruction(b);
            Menue.SetCurrentBuild("" + Mathf.Ceil(c.currentCost / c.production),Resources.Load<Sprite>(c.construction.index),c.construction.index);
        }
        else
        {
            c.ShowAvailable(build);
            Menue.HideBat();            
            Menue.HideCity();                                     
            GameController.instance.MapControllerScript.Extension = true;
            GameController.instance.ExtentionTemp = build.Copy();
            
        }
        if (c.construction != null)
        {
            c.construction.Tempcost = c.currentCost;            
        }

        foreach (BuildingButton B in Menue.Buttons)
        {
            B.UpdateNumbers(c);
        }
    }

    
}

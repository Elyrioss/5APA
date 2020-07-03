using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CityMenu : MonoBehaviour
{
    public Animator Anim;
    public GameObject FileUI;

    public BuildingButton pref;
    public BuildingButton Extend;
    public int Batnum;
    public RectTransform ContentBuilding;
    public int Unitnum;
    public RectTransform ContentUnit;
    
    private List<BuildingButton> Buttons = new List<BuildingButton>();
    
    
    //UI

    public TextMeshProUGUI Food;
    public TextMeshProUGUI Production;
    public TextMeshProUGUI Gold;
    public TextMeshProUGUI Population;
    public TextMeshProUGUI CityName;

    public Sprite DefaultConstruction;
    public Image CurrentConstructionImage;
    public TextMeshProUGUI CurrentConstructionText;
    public TextMeshProUGUI CurrentConstructionTime;
    
    
    public Buildings getBuildings(int index)
    {
        switch (index)
        {
            case 0:
                return new Grenier();
            case 1:
                return new Usine();
            case 2:
                return new Marcher();
            case 3:
                return new Extension();
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
    
    void Start()
    {
        
        gameObject.SetActive(false);
        
        for (int i = 0; i < Batnum; i++)
        {
            Buildings build = getBuildings(i);
            BuildingButton b;
            if (build.BuildType == Buildings.BuildingType.Ressource)
            {
                b = Instantiate(pref, ContentBuilding);
            }
            else
            {
                b = Instantiate(Extend, ContentBuilding);
            }
            b.build = build;
            b.SetupBuilding();
            b.gameObject.SetActive(true);
            Buttons.Add(b);
        }
        
        for (int i = 0; i < Unitnum; i++)
        {
            Unit build = getUnits(i);
            BuildingButton b = Instantiate(pref, ContentUnit);
            b.build = build;
            b.SetupUnit();
            b.gameObject.SetActive(true);
            Buttons.Add(b);
        }
    }


    public void ShowBat()
    {
        Anim.SetBool("ShowBat",true);
        City current = GameController.instance.SelectedCity;
        if (current == null)
            return;

        foreach (BuildingButton b in Buttons)
        {
            b.UpdateNumbers(current);
        }
        if (current.construction == null)
        {
            CurrentConstructionTime.text = "0";
            CurrentConstructionImage.sprite = DefaultConstruction;
            CurrentConstructionText.text = "None";
            return;
        }
                   
        CurrentConstructionTime.text = "" + Mathf.Ceil(current.currentCost / current.production);
        CurrentConstructionImage.sprite = Resources.Load<Sprite>(current.construction.index);
        CurrentConstructionText.text = current.construction.index;
        
    }
    
    public void HideBat()
    {
        Anim.SetBool("ShowBat",false);
    }
    
    public void ShowCity()
    {
        Anim.SetBool("ShowBat",false);
        Anim.SetBool("ShowCity",true);
    }
    
    public void HideCity()
    {
        Anim.SetBool("ShowCity",false);
        
    }
    



}

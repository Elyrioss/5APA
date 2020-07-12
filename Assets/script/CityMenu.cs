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
    
    public List<BuildingButton> Buttons = new List<BuildingButton>();
    
    
    //UI

    public TextMeshProUGUI Food;
    public TextMeshProUGUI Production;
    public TextMeshProUGUI Gold;
    public TextMeshProUGUI Science;
    public TextMeshProUGUI Population;
    public TextMeshProUGUI CityName;

    public Sprite DefaultConstruction;
    public Image CurrentConstructionImage;
    public TextMeshProUGUI CurrentConstructionText;
    public TextMeshProUGUI CurrentConstructionTime;

    public Image CivColor;
    
    public Slider PopulationSlide;
    public Slider ProductionSlide;

    public TextMeshProUGUI Construct;
    public TextMeshProUGUI NumConstruct;
    public TextMeshProUGUI NumPop;

    public TextMeshProUGUI NameUnit;
    public Image UnitCivColor;
    public Slider life;
    public TextMeshProUGUI lifeNum;

    public GameObject Unit;
    public GameObject City;
    public Construction getBuildings(int index)
    {
        switch (index)
        {
            case 0:
                return new Grenier();
            case 1:
                return new Usine();
            case 2:
                return new Laboratoire();
            case 3:
                return new Marcher();
            case 4:
                return new Extension();
            case 5:
                return new Port();
            case 6:
                return new MineFer();
            case 7:
                return new MineOr();
            case 8: 
                return new PlateformMaritime();
            case 9: 
                return new PlateformCorail();
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
            case 3:
                return new Colon();
        }
        return null;        
    }
    
    void Start()
    {
                
        for (int i = 0; i < Batnum; i++)
        {
            Construction build = getBuildings(i);
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
        if (current.construction.empty)
        {           
            SetCurrentBuild("0",DefaultConstruction,"None");
            return;
        }
        
        
        SetCurrentBuild("" + Mathf.Ceil(current.currentCost / current.production),Resources.Load<Sprite>(current.construction.index),current.construction.index);
        
    }
    
    public void HideBat()
    {
        Anim.SetBool("ShowBat",false);
    }
    
    public void ShowCity()
    {
        Anim.SetBool("ShowBat",false);
        Anim.SetBool("ShowCity",true);

        Unit.SetActive(false);
        City.SetActive(true);
        
        
        City current = GameController.instance.SelectedCity;
        if (current == null)
            return;
        
        CityName.text = current.NameCity;
        Population.text = "" + current.population;
        Food.text = "" + current.food;
        Production.text = "" + current.production;
        Gold.text = "" + current.gold;
        Science.text = "" + current.science;
        
        
        current.HideAvailable();
        GameController.instance.MapControllerScript.Extension = false;
        GameController.instance.MapControllerScript.Move = false;
        PopulationSlide.value = current.StockFood/(30 * current.FoodMultiplier);
        NumPop.text = Mathf.Ceil(((30 * current.FoodMultiplier)-current.StockFood)/current.food)+"";
        
        if (current.construction.empty)
        {           
            SetCurrentBuild("0",DefaultConstruction,"None");
        }
        else
        {
            SetCurrentBuild("" + Mathf.Ceil(current.currentCost / current.production),Resources.Load<Sprite>(current.construction.index),current.construction.index);
        }                   

    }

    public void ShowUnit(Unit unit)
    {       
        Anim.SetBool("ShowBat",false);
        Anim.SetBool("ShowCity",true);

        Unit.SetActive(true);
        City.SetActive(false);
        
        NameUnit.text = unit.index;
        life.value = (float)unit.HP / unit.MAXHP;
        lifeNum.text = (int)(((float) unit.HP / unit.MAXHP) * 100) + "%";
        UnitCivColor.color = GameController.instance.CurrentCiv.CivilisationColor; 
    }
    
    public void HideCity()
    {
        Anim.SetBool("ShowCity",false);
    }

    public void SetCurrentBuild(string time, Sprite image, string name)
    {
        CurrentConstructionTime.text = time;
        CurrentConstructionImage.sprite = image;
        CurrentConstructionText.text = name;

        if (time != "0")
        {
            ProductionSlide.maxValue = GameController.instance.SelectedCity.construction.cost;
            ProductionSlide.value = GameController.instance.SelectedCity.construction.cost-GameController.instance.SelectedCity.currentCost;
            Construct.text = name;
            NumConstruct.text = time;
        }
        else
        {
            ProductionSlide.maxValue = 1;
            ProductionSlide.value = 0;
            Construct.text = name;
            NumConstruct.text = time;
        }
        
    }

    public void UsePower()
    {
        GameController.instance.SelectedUnit.UnitPower(GameController.instance.CurrentCiv);
    }
    
    public void Sleep()
    {
        GameController.instance.SelectedUnit.AsPlayed=true;
        GameController.instance.MapControllerScript.Move=false;
    }
    
    
}

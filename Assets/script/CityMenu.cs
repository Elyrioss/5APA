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
    
    void Start()
    {
        
        gameObject.SetActive(false);
        
        Extend.SetupBuilding(this);
        for (int i = 0; i < Batnum; i++)
        {
            BuildingButton b = Instantiate(pref, ContentBuilding);
            b.index = i;
            b.SetupBuilding(this);
            b.gameObject.SetActive(true);
        }
        
        for (int i = 0; i < Unitnum; i++)
        {
            BuildingButton b = Instantiate(pref, ContentUnit);
            b.index = i;
            b.SetupUnit(this);
            b.gameObject.SetActive(true);
        }
    }


    public void ShowBat()
    {
        Anim.SetBool("ShowBat",true);
        City current = GameController.instance.SelectedCity;
        if (current == null)
            return;

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

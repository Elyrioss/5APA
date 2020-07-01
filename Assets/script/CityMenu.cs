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
    public RectTransform Content;
    
    //UI

    public TextMeshProUGUI Food;
    public TextMeshProUGUI Production;
    public TextMeshProUGUI Gold;
    public TextMeshProUGUI Population;
    public TextMeshProUGUI CityName;

    public Image CurrentConstructionImage;
    public TextMeshProUGUI CurrentConstructionText;
    public TextMeshProUGUI CurrentConstructionTime;
    
    void Start()
    {
        
        gameObject.SetActive(false);
        
        Extend.Setup(this);
        for (int i = 0; i < Batnum; i++)
        {
            BuildingButton b = Instantiate(pref, Content);
            b.index = i;
            b.Setup(this);
            b.gameObject.SetActive(true);
        }
    }


    public void ShowBat()
    {
        Anim.SetBool("ShowBat",true);
    }
    
    public void HideBat()
    {
        Anim.SetBool("ShowBat",false);
    }
    
    public void ShowCity()
    {
        Anim.SetBool("ShowCity",true);
    }
    
    public void HideCity()
    {
        Anim.SetBool("ShowCity",false);
        Anim.SetBool("ShowBat",false);
    }
    



}

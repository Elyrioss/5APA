using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CityMenu : MonoBehaviour
{
    public Animator Anim;
    public GameObject FileUI;

    public BuildingButton pref;
    public BuildingButton Extend;
    public int Batnum;
    public RectTransform Content;
    
    void Start()
    {
        
        Extend.Setup();
        for (int i = 0; i < Batnum; i++)
        {
            BuildingButton b = Instantiate(pref, Content);
            b.index = i;
            b.Setup();
            b.gameObject.SetActive(true);
        }
    }


    public void DeselectCity()
    {
        Anim.SetBool("OUT", false);
        Anim.SetBool("IN", true);
        FileUI.SetActive(false);
    }
    public void Construct()
    {
        Anim.SetBool("IN", false);
        Anim.SetBool("OUT", true);
    }

    public void CreateWarrior()
    {
        Anim.SetBool("OUT", false);
        Anim.SetBool("IN", true);
    }


}

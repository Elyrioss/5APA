using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScienceButton : MonoBehaviour
{
    
    public Science science;
    public string NameScience;
    public ScienceScreen Screen;
    public Image iconImage;
    public TextMeshProUGUI Name;
    public Image buttonColor;
    public Sprite icon;
    public List<ScienceButton> Excludes;
    public List<ScienceButton> Sons;
    
    public void Action()
    {
        Screen.currentScience = this;
        Screen.Description.text = science.Description;
        Screen.ScienceName.text = science.nameScience;
        Screen.ColorType.color = science.Type;
        Screen.Icon.sprite = icon;
        Screen.NumScience.text = Mathf.Ceil(science.cost / Screen.owner.Science)+"";

    }

}

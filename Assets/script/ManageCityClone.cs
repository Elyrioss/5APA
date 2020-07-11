using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageCityClone : MonoBehaviour
{

    public ManageCity ManageRef;

    public TextMeshPro NameCity;
    public Image Colors;
    
    public void SelectCity()
    {
        ManageRef.SelectCity();
    }

    public TextMeshProUGUI Population;
    public Slider HP;

    private void Update()
    {
        if (ManageRef)
        {
            Population.text = ManageRef.Population.text;
            HP.value = ManageRef.HP.value;
        }
    }
}

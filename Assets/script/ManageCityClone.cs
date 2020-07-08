using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManageCityClone : MonoBehaviour
{

    public ManageCity ManageRef;

    public TextMeshPro NameCity;
    public void SelectCity()
    {
        ManageRef.SelectCity();
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCityClone : MonoBehaviour
{

    public GameObject ManageRef;


    public void SelectCity()
    {
        ManageRef.GetComponent<ManageCity>().SelectCity();
    }


}

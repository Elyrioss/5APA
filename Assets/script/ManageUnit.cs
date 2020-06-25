using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageUnit : MonoBehaviour
{
    public int ThisCity; //Index of List _cities in MainMapController
    public int ThisUnit; //Index of List _cities[ThisCity].construction.Units
    public MainMapControllerScript Mapcontroller;

    public GameObject WarriorModel;
    public GameObject ArcherModel;
    public GameObject RiderModel;

    // Start is called before the first frame update
    void Start()
    {
        Mapcontroller = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMapControllerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Mapcontroller._cities[ThisCity].construction.WarriorCounter == 0)
        {
            ChoseUnitModel();
        }*/
    }


    public void ChoseUnitModel()
    {
        Debug.Log("OUIIII");
        if(Mapcontroller._cities[ThisCity].construction.Units[ThisUnit].Type == Unit.UnitType.Warrior)
        {
            WarriorModel.SetActive(true);
        }
        else if (Mapcontroller._cities[ThisCity].construction.Units[ThisUnit].Type == Unit.UnitType.Archer)
        {
            ArcherModel.SetActive(true);
        }
        else if (Mapcontroller._cities[ThisCity].construction.Units[ThisUnit].Type == Unit.UnitType.Rider)
        {
            RiderModel.SetActive(true);
        }
    }
}

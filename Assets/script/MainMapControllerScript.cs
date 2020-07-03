﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class MainMapControllerScript : MonoBehaviour
{
    public Camera _camera = null;
    Waypoint selectedWaypoint = null;
    public ManageCity cityPref;
    public ManageCityClone cityClonePref;
    public List<City> _cities = new List<City>();
    public int TmpCityIndex;
    public tileMapManager Map;
    public Transform Raystarter;

    public AudioSource clickSound;
    public AudioSource buildSound;
    public AudioSource soldierSound;
    ///TESTSUI
    private bool StartingCity;
    public CityMenu Menue;
    private Animator Anim;  
    public bool CanRaycast = true;
    public bool Extension;

    public float LeftLimit;
    public float RightLimit;
    public Camera secondCam;
    public Camera thirdCam;
    
    void Start()
    {
        _camera = GetComponent<Camera>();
        SetToLOD();
        
        RightLimit =  Map.LineOrder[Map.LineOrder.Count-1].transform.position.x;
        LeftLimit = -RightLimit;
        secondCam.transform.localPosition = new Vector3(RightLimit,secondCam.transform.localPosition.y,secondCam.transform.localPosition.z);
        thirdCam.transform.localPosition = new Vector3(LeftLimit,secondCam.transform.localPosition.y,secondCam.transform.localPosition.z);

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Raystarter.position.x < (LeftLimit/2)+40)
        {
            _camera.transform.position = secondCam.transform.position;
        }
       
        if (Raystarter.position.x > RightLimit-40)
        {
            _camera.transform.position = thirdCam.transform.position;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!CanRaycast)
            {
                return;
            }
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) // Ici on va gérer toutes les possibilités de click d'éléments
            {
                if (!hit.transform.parent.gameObject.GetComponent<Waypoint>())
                    return;
                if (!StartingCity) // Création de la ville du début ( et des autres villes après ?)
                {
                    selectedWaypoint = hit.transform.parent.gameObject.GetComponent<Waypoint>();
                    ManageCity CityObj = Instantiate(cityPref, selectedWaypoint.transform);
                    CityObj.transform.localPosition = new Vector3(0,selectedWaypoint.elevation, 0);
                    _cities.Add(new City(selectedWaypoint, Color.red));
                    
                    CityObj.ThisCity = _cities.Count - 1;
                    
                    //Add sound elements
                    
                    _cities[_cities.Count - 1].buildSound = this.buildSound;
                    _cities[_cities.Count - 1].soldierSound = this.soldierSound;
                    _cities[_cities.Count - 1].clickSound = this.clickSound;
                    //TWIN
                    if (selectedWaypoint.AsTwin || selectedWaypoint.IsTwin)
                    {
                        ManageCityClone CityObjC = Instantiate(cityClonePref, selectedWaypoint.Twin.transform);
                        CityObjC.transform.localPosition = new Vector3(0, selectedWaypoint.elevation, 0);
                        CityObjC.ManageRef = CityObj;

                    }
                    //
                    StartingCity = true;
                    Menue.ShowCity();
                }
                else if (Extension)
                {
                    if (!hit.transform.parent.gameObject.GetComponent<Waypoint>())
                        return;

                    Construction extention = GameController.instance.ExtentionTemp;
                    selectedWaypoint = hit.transform.parent.gameObject.GetComponent<Waypoint>();
                    City current = GameController.instance.SelectedCity;
                    
                    if ((current.controlArea.Contains(selectedWaypoint)||_cities[TmpCityIndex].controlAreaClone.Contains(selectedWaypoint))&& !selectedWaypoint.UsedTile)
                    {
                        Menue.ShowCity();
                        Menue.ShowBat();
                        extention.Position = selectedWaypoint;
                        current.StartConstruction(extention);
                        selectedWaypoint.UsedTile = true;
                        if (selectedWaypoint.LOD)
                        {
                            selectedWaypoint.LOD.SetActive(false);
                        }
                        if (selectedWaypoint.Twin)
                        {
                            if (selectedWaypoint.Twin.LOD)
                            {
                                selectedWaypoint.Twin.LOD.SetActive(false);
                            }
                        }
                        
                        extention.prefab = GameObject.Instantiate(Resources.Load("Prefabs/"+extention.index+"Base") as GameObject, selectedWaypoint.transform);
                        extention.prefab.transform.localPosition = new Vector3(0, selectedWaypoint.elevation, 0);
                        
                        Menue.CurrentConstructionTime.text = "" + Mathf.Ceil(current.construction.cost / current.production);
                        Menue.CurrentConstructionImage.sprite = Resources.Load<Sprite>(extention.index);
                        Menue.CurrentConstructionText.text = extention.index;
                        
                        Extension = false;
                    }
                    
                }
                
                /*
                else if (Warrior)
                {
                    selectedWaypoint = hit.transform.parent.gameObject.GetComponent<Waypoint>();
                    if (_cities[TmpCityIndex].controlArea.Contains(selectedWaypoint))
                    {
                        _cities[TmpCityIndex].construction.WarriorWaypoint = selectedWaypoint;
                        _cities[TmpCityIndex].construction.WarriorConstruction = true;
                        var Obj = Instantiate(UnitPref, selectedWaypoint.transform);
                        Obj.transform.localPosition = new Vector3(0, selectedWaypoint.elevation, 0);
                        _cities[TmpCityIndex].construction.WarriorUnit = Obj;
                        Obj.GetComponent<ManageUnit>().ThisCity = TmpCityIndex;
                        Obj.GetComponent<ManageUnit>().ThisUnit = _cities[TmpCityIndex].construction.Units.Count;
                        TmpManageCity.CityName.SetActive(true);
                        //TWIN
                        if (selectedWaypoint.AsTwin || selectedWaypoint.IsTwin)
                        {
                            var ObjC = Instantiate(UnitPref, selectedWaypoint.Twin.transform);
                            ObjC.transform.localPosition = new Vector3(0, selectedWaypoint.elevation, 0);
                        }
                        //
                        Warrior = false;
                        CanRaycast = false;
                    }
                }
                else if (Archer)
                {
                    selectedWaypoint = hit.transform.parent.gameObject.GetComponent<Waypoint>();
                    if (_cities[TmpCityIndex].controlArea.Contains(selectedWaypoint))
                    {
                        _cities[TmpCityIndex].construction.ArcherWaypoint = selectedWaypoint;
                        _cities[TmpCityIndex].construction.ArcherConstruction = true;
                        var Obj = Instantiate(UnitPref, selectedWaypoint.transform);
                        Obj.transform.localPosition = new Vector3(0, selectedWaypoint.elevation, 0);
                        _cities[TmpCityIndex].construction.ArcherUnit = Obj;
                        Obj.GetComponent<ManageUnit>().ThisCity = TmpCityIndex;
                        Obj.GetComponent<ManageUnit>().ThisUnit = _cities[TmpCityIndex].construction.Units.Count; 
                        TmpManageCity.CityName.SetActive(true);
                        //TWIN
                        if (selectedWaypoint.AsTwin || selectedWaypoint.IsTwin)
                        {
                            var ObjC = Instantiate(UnitPref, selectedWaypoint.Twin.transform);
                            ObjC.transform.localPosition = new Vector3(0, selectedWaypoint.elevation, 0);
                        }
                        //
                        Archer = false;
                        CanRaycast = false;
                    }
                }
                else if (Rider)
                {
                    selectedWaypoint = hit.transform.parent.gameObject.GetComponent<Waypoint>();
                    if (_cities[TmpCityIndex].controlArea.Contains(selectedWaypoint))
                    {
                        _cities[TmpCityIndex].construction.RiderWaypoint = selectedWaypoint;
                        _cities[TmpCityIndex].construction.RiderConstruction = true;
                        var Obj = Instantiate(UnitPref, selectedWaypoint.transform);
                        Obj.transform.localPosition = new Vector3(0, selectedWaypoint.elevation, 0);
                        _cities[TmpCityIndex].construction.RiderUnit = Obj;
                        Obj.GetComponent<ManageUnit>().ThisCity = TmpCityIndex;
                        Obj.GetComponent<ManageUnit>().ThisUnit = _cities[TmpCityIndex].construction.Units.Count;
                        TmpManageCity.CityName.SetActive(true);
                        //TWIN
                        if (selectedWaypoint.AsTwin || selectedWaypoint.IsTwin)
                        {
                            var ObjC = Instantiate(UnitPref, selectedWaypoint.Twin.transform);
                            ObjC.transform.localPosition = new Vector3(0, selectedWaypoint.elevation, 0);
                        }
                        //
                        Rider = false;
                        CanRaycast = false;
                    }
                }*/
            }
        }
    }

    public void SetToLOD()
    {
        foreach (Waypoint w in Map.LineOrder)
        {
            if (w.prop)
            {
                w.prop.SetActive(false);
                if(w.AsTwin)
                    w.Twin.prop.SetActive(false);
                
                if (w.LOD)
                {
                    w.LOD.SetActive(true);
                    if(w.AsTwin)
                        w.Twin.LOD.SetActive(true);
                }
                    
                
            }
        }
    }
    public void SetToProp()
    {
        foreach (Waypoint w in Map.LineOrder)
        {
            if (w.prop)
            {
                w.prop.SetActive(true);
                if(w.LOD)
                    w.LOD.SetActive(false);
            }
        }
    }
    
    
}

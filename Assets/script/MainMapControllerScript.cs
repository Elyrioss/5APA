using System;
using System.Collections.Generic;
using UnityEngine;

public class MainMapControllerScript : MonoBehaviour
{
    Camera _camera = null;
    Waypoint selectedWaypoint = null;
    public GameObject cityPref;
    public GameObject cityClonePref;
    public GameObject ExtensionPref;
    public GameObject UnitPref;
    public List<City> _cities = new List<City>();
    public int TmpCityIndex;
    public tileMapManager Map;
    public Transform Raystarter;
    public Transform Raystarter2;
    public Transform Raystarter3;

    private Transform CurrentRayCast;
    private Camera CurrentCamera;
    
    ///TESTSUI
    private bool StartingCity;
    public GameObject FileUI;
    private Animator Anim;  
    public bool CanRaycast = true;
    public bool Extension;
    public bool Warrior;
    public bool Archer;
    public bool Rider;
    public ManageCity TmpManageCity;

    public float LeftLimit;
    public float RightLimit;
    public Camera secondCam;
    public Camera thirdCam;
    
    void Start()
    {
        _camera = GetComponent<Camera>();
        Anim = FileUI.GetComponent<Animator>();
        SetToLOD();
        
        RightLimit =  Map.LineOrder[Map.LineOrder.Count-1].transform.position.x;
        LeftLimit = -RightLimit;
        
        Debug.Log("Left :"+LeftLimit+", Right :"+RightLimit);
        
        CurrentRayCast = Raystarter;
        CurrentCamera = _camera;
        secondCam.transform.localPosition = new Vector3(RightLimit,secondCam.transform.localPosition.y,secondCam.transform.localPosition.z);
        thirdCam.transform.localPosition = new Vector3(LeftLimit,secondCam.transform.localPosition.y,secondCam.transform.localPosition.z);

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (CurrentRayCast.position.x < (LeftLimit/2)+40)
        {
            _camera.transform.position = secondCam.transform.position;
        }
       
        if (CurrentRayCast.position.x > RightLimit-40)
        {
            _camera.transform.position = thirdCam.transform.position;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!CanRaycast)
            {
                return;
            }
            Ray ray = CurrentCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) // Ici on va gérer toutes les possibilités de click d'éléments
            {
                if (!hit.transform.parent.gameObject.GetComponent<Waypoint>())
                    return;
                if (!StartingCity) // Création de la ville du début ( et des autres villes après ?)
                {
                    selectedWaypoint = hit.transform.parent.gameObject.GetComponent<Waypoint>();
                    var CityObj = Instantiate(cityPref, selectedWaypoint.transform);
                    CityObj.transform.localPosition = new Vector3(0,selectedWaypoint.elevation, 0);
                    _cities.Add(new City(selectedWaypoint, Color.red));
                    CityObj.GetComponent<ManageCity>().ThisCity = _cities.Count - 1;
                    //TWIN
                    if (selectedWaypoint.AsTwin || selectedWaypoint.IsTwin)
                    {
                        var CityObjC = Instantiate(cityClonePref, selectedWaypoint.Twin.transform);
                        CityObjC.transform.localPosition = new Vector3(0, selectedWaypoint.elevation, 0);
                        CityObjC.GetComponent<ManageCityClone>().ManageRef = CityObj;

                    }
                    //
                    StartingCity = true;
                    CanRaycast = false;
                }
                else if (Extension)
                {
                    selectedWaypoint = hit.transform.parent.gameObject.GetComponent<Waypoint>();
                    if (_cities[TmpCityIndex].controlArea.Contains(selectedWaypoint))
                    {
                        _cities[TmpCityIndex].construction.ExtensionWaypoint = selectedWaypoint;
                        _cities[TmpCityIndex].construction.ExtensionConstruction = true;
                        var ExtObj = Instantiate(ExtensionPref, selectedWaypoint.transform);
                        ExtObj.transform.localPosition = new Vector3(0, selectedWaypoint.elevation, 0);
                        TmpManageCity.CityName.SetActive(true);
                        //TWIN
                        if (selectedWaypoint.AsTwin || selectedWaypoint.IsTwin)
                        {
                            var ExtObjC = Instantiate(ExtensionPref, selectedWaypoint.Twin.transform);
                            ExtObjC.transform.localPosition = new Vector3(0, selectedWaypoint.elevation, 0);
                        }
                        //
                        Extension = false;
                        CanRaycast = false;
                    }
                    else if (_cities[TmpCityIndex].controlAreaClone.Contains(selectedWaypoint))
                    {
                        _cities[TmpCityIndex].construction.ExtensionWaypoint = selectedWaypoint.Twin;
                        _cities[TmpCityIndex].construction.ExtensionConstruction = true;
                        var ExtObj = Instantiate(ExtensionPref, selectedWaypoint.Twin.transform);
                        ExtObj.transform.localPosition = new Vector3(0, 0, 0);
                        TmpManageCity.CityName.SetActive(true);
                        //TWIN
                        if (selectedWaypoint.Twin.AsTwin || selectedWaypoint.Twin.IsTwin)
                        {
                            var ExtObjC = Instantiate(ExtensionPref, selectedWaypoint.transform);
                        }
                        //
                        Extension = false;
                        CanRaycast = false;
                    }
                    
                }
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
                }
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

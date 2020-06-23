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
    public List<City> _cities = new List<City>();
    public int TmpCityIndex;
    public tileMapManager Map;
    public Transform Raystarter;
    
    ///TESTSUI
    private bool StartingCity;
    public GameObject FileUI;
    private Animator Anim;  
    public bool CanRaycast = true;
    public bool Extension;
    public ManageCity TmpManageCity;

    public float LeftLimit = 0;
    public float RightLimit = 0;
    
    void Start()
    {
        _camera = GetComponent<Camera>();
        Anim = FileUI.GetComponent<Animator>();
        SetToLOD();
        float Quarter = ((float)(Map.sizeChunkX * Map.chunkX + Map.sizeChunkX * (Map.chunkX/8)))/4;
        LeftLimit = Map.TwinOrder[0].transform.localPosition.x + Quarter;
        RightLimit = Map.LineOrder[Map.LineOrder.Count-1].transform.localPosition.x - Quarter;

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (transform.position.x < LeftLimit)
        {
            transform.localPosition = new Vector3(RightLimit,transform.localPosition.y,transform.localPosition.z);
            if (Physics.Raycast(Raystarter.position, Raystarter.TransformDirection(Vector3.down), out hit, 1000))
            {
                Debug.DrawRay(Raystarter.position, Raystarter.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
                if (hit.transform.parent.GetComponent<Waypoint>())
                {
                    Waypoint w = hit.transform.parent.GetComponent<Waypoint>();
                   
                }
            }
        }
        
        if (transform.position.x > RightLimit)
        {
            transform.localPosition = new Vector3(LeftLimit,transform.localPosition.y,transform.localPosition.z);
            if (Physics.Raycast(Raystarter.position, Raystarter.TransformDirection(Vector3.down), out hit, 1000))
            {
                Debug.DrawRay(Raystarter.position, Raystarter.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
                if (hit.transform.parent.GetComponent<Waypoint>())
                {
                    Waypoint w = hit.transform.parent.GetComponent<Waypoint>();
                   
                }
            }
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
                    var CityObj = Instantiate(cityPref, selectedWaypoint.transform);
                    CityObj.transform.localPosition = new Vector3(0,selectedWaypoint.elevation, 0);
                    _cities.Add(new City(selectedWaypoint, Color.red));
                    CityObj.GetComponent<ManageCity>().ThisCity = _cities.Count - 1;
                    //TWIN
                    if (selectedWaypoint.AsTwin || selectedWaypoint.IsTwin)
                    {
                        var CityObjC = Instantiate(cityClonePref, selectedWaypoint.Twin.transform);
                        CityObjC.transform.localPosition = new Vector3(0, selectedWaypoint.elevation, 0);
                        CityObjC.GetComponent<ManageCityClone>().ManageRef = this.gameObject;

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

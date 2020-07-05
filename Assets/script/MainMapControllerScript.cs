using System;
using System.Collections.Generic;
using System.Linq;
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
    public bool Move;

    private Waypoint Target;
    
    private List<Waypoint> Path = new List<Waypoint>();
    List<Waypoint> PQueue = new List<Waypoint>();
    private List<Waypoint> Visited = new List<Waypoint>();
    private int CurrentCost = 0;
    private int Cost = 15;
    private Waypoint NewPos;
    
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
                        
                        extention.Position = selectedWaypoint;
                        current.StartConstruction(extention);
                        selectedWaypoint.UsedTile = true;
                        current.Buildings.Add(extention);
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
                        
                        Menue.SetCurrentBuild("" + Mathf.Ceil(current.currentCost / current.production),Resources.Load<Sprite>(extention.index),extention.index);
                        current.HideAvailable();
                        Extension = false;
                        Menue.ShowCity();
                        Menue.ShowBat();
                    }
                    
                }
                else if (Move)
                {
                    MoveWithPath();
                }             
            }

            
        }
        if (Input.GetMouseButtonDown(1))
        {
            Menue.HideBat();            
            Menue.HideCity();
            Extension = false;
            Move = false;
        }

        if (Move)
        {
            
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) // Ici on va gérer toutes les possibilités de click d'éléments
            {
                if (!hit.transform.parent.gameObject.GetComponent<Waypoint>())
                {
                    Target = null;
                    return;
                }
                    
                selectedWaypoint = hit.transform.parent.gameObject.GetComponent<Waypoint>();
                if (selectedWaypoint == Target)
                    return;

                PQueue.Clear();
                Path.Clear();
                foreach (Waypoint w in Visited)
                {
                    w.visitedDijstra = false;
                }
                Visited.Clear();
                Target = selectedWaypoint;
                AStarCreate(GameController.instance.SelectedUnit.Position, Target);
                
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
    
    public void AStarCreate(Waypoint Start,Waypoint End)
    {
        if (Start == null || End == null)
        {
            return;
        }
        Start.MinCostToStart = 0;
        PQueue.Add(Start);
        while (PQueue.Any())
        {
            Debug.Log("IN");
            var current = PQueue.First();
            PQueue.Remove(current);
            if (current == End)
            {
                break;
            }
            foreach (Waypoint w in current.Neighbors.OrderBy(x => x.GetComponent<Waypoint>().mouvCost))
            {
                if (w.visitedDijstra)
                {
                    continue;
                }
                var nextCost = current.MinCostToStart + w.mouvCost;
                if (nextCost < w.MinCostToStart)
                {
                    w.MinCostToStart = nextCost;
                    w.NearestToStart = current;
                    if (!PQueue.Contains(w))
                    {
                        PQueue.Add(w);
                    }
                }
            }
            current.visitedDijstra = true;
            Visited.Add(current);
            PQueue = PQueue.OrderBy(x => x.MinCostToStart).ToList();
        }
        
        CreatePath(Path, End);
        Path.Reverse();
        Path.Add(End);
        //DrawPath(Path);
        //MoveWithPath(Path);      
    }
    
    private void CreatePath(List<Waypoint> TmpPath, Waypoint W)
    {
        if (W.NearestToStart == null)
        {
            return;
        }
        TmpPath.Add(W.NearestToStart);
        CreatePath(TmpPath, W.NearestToStart);
    }
    
    private void MoveWithPath()
    {
        CurrentCost = 0;
        NewPos = Path[0];
        Unit unit = GameController.instance.SelectedUnit;
        foreach(Waypoint w in Path)
        {
            CurrentCost += w.mouvCost;
            if(CurrentCost <= unit.mouvementPoints)
            {
                NewPos = w;
            }
            else
            {
                break;
            }
        }
        
        unit.Position = NewPos;
        unit.prefab.transform.position = new Vector3(NewPos.transform.position.x, NewPos.transform.position.y + NewPos.elevation, NewPos.transform.position.z);
        if (NewPos.Twin)
        {           
            unit.Twin.transform.position = new Vector3(NewPos.Twin.transform.position.x, NewPos.Twin.transform.position.y + NewPos.Twin.elevation, NewPos.Twin.transform.position.z);
            unit.Twin.SetActive(true);
        }
        else
        {
            unit.Twin.SetActive(false);   
        }
        
        //clear
        PQueue.Clear();
        Path.Clear();
        PQueue.Clear();
        Path.Clear();
        foreach (Waypoint w in Visited)
        {
            w.visitedDijstra = false;
            w.NearestToStart = null;
            w.MinCostToStart = int.MaxValue;
        }
        Visited.Clear();
        Move = false;
        unit.AsPlayed = true;
    }
    
}

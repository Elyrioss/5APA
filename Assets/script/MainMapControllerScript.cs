using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMapControllerScript : MonoBehaviour
{
    public GameController GameController;
    public Camera _camera = null;
    Waypoint selectedWaypoint = null;
    public ManageCity cityPref;
    public ManageCityClone cityClonePref;
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
    public List<Waypoint> Visited = new List<Waypoint>();
    
    public float LeftLimit;
    public float RightLimit;
    public Camera secondCam;
    public Camera thirdCam;

    public bool attackAvailable = false;

    public List<Waypoint> highlightedWayPoint = new List<Waypoint>();

    void Start()
    {
        _camera = GetComponent<Camera>();
        SetToLOD();
        GameController = global::GameController.instance;
        RightLimit =  Map.LineOrder[Map.LineOrder.Count-1].transform.position.x;
        LeftLimit = -RightLimit;
        secondCam.transform.localPosition = new Vector3(RightLimit,secondCam.transform.localPosition.y,secondCam.transform.localPosition.z);
        thirdCam.transform.localPosition = new Vector3(LeftLimit,secondCam.transform.localPosition.y,secondCam.transform.localPosition.z);
        GameController.instance.StartPos();
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

        if (Input.GetMouseButton(0))
        {
            if(IsPointerOverUI())
                return;
            
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) // Ici on va gérer toutes les possibilités de click d'éléments
            {
                if (!hit.transform.parent.gameObject.GetComponent<Waypoint>())
                    return;

                if (Extension)
                {
                    if (!hit.transform.parent.gameObject.GetComponent<Waypoint>())
                        return;

                    Construction extention = GameController.ExtentionTemp;
                    selectedWaypoint = hit.transform.parent.gameObject.GetComponent<Waypoint>();
                    City current = GameController.SelectedCity;
                    
                    if ((current.controlArea.Contains(selectedWaypoint) || current.controlAreaClone.Contains(selectedWaypoint))&& !selectedWaypoint.UsedTile && extention.CheckForConditions(selectedWaypoint))
                    {
                        
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
                        if (selectedWaypoint.Twin)
                        {
                            extention.Twin = GameObject.Instantiate(Resources.Load("Prefabs/"+extention.index+"Base") as GameObject, selectedWaypoint.Twin.transform);
                            extention.Twin.transform.localPosition = new Vector3(0, selectedWaypoint.elevation, 0);
                        }
                        
                        Menue.SetCurrentBuild("" + Mathf.Ceil(current.currentCost / current.production),Resources.Load<Sprite>(extention.index),extention.index);
                        current.HideAvailable();
                        extention.VisualSteps();                 
                        
                        Extension = false;
                        Menue.ShowCity();
                        Menue.ShowBat();
                    }
                    
                }
                if (Move)
                {
                    if (!hit.transform.parent.gameObject.GetComponent<Waypoint>())
                        return;
                    
                    selectedWaypoint = hit.transform.parent.gameObject.GetComponent<Waypoint>();
                    MoveWithPath(selectedWaypoint);  
                    Menue.HideCity();
                }

                if (attackAvailable)
                {
                    if (!hit.transform.parent.gameObject.GetComponent<Waypoint>())
                        return;

                    selectedWaypoint = hit.transform.parent.gameObject.GetComponent<Waypoint>();

                    GameController GC = GameController.instance;

                    foreach (Waypoint wp in highlightedWayPoint)
                    {
                        if (wp == selectedWaypoint)
                        {
                            Civilisation enemyCiv = GC.GetOtherCivilisation();
                            
                            foreach (Unit enemyUnit in enemyCiv.Units)
                            {
                                if(enemyUnit.Position == wp)
                                {
                                    DoAttack(GC.SelectedUnit, enemyUnit);
                                    break;
                                }
                            }
                        }

                        wp.DisableHighlight();
                    }
                }
            }

            
        }
        if (Input.GetMouseButtonDown(1))
        {
            Menue.HideBat();            
            Menue.HideCity();
            Extension = false;
            if(GameController.SelectedCity!=null)
                GameController.SelectedCity.HideAvailable();

            ClearPath();
            Move = false;
        }

        if (Move)
        {            
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {

                if (!hit.transform.parent.gameObject.GetComponent<Waypoint>())
                {
                    Target = null;
                    return;
                }                
                selectedWaypoint = hit.transform.parent.gameObject.GetComponent<Waypoint>();
                if (Target == selectedWaypoint)
                    return;

                foreach (Waypoint p in Path)
                {
                    p.DisableTrail();
                    if(p.Twin)
                        p.Twin.DisableTrail();
                }
                
                Target = selectedWaypoint;
                if (Visited.Contains(selectedWaypoint))
                {                         
                    selectedWaypoint.SetIndexTrail(1);
                    CreatePath(Path, selectedWaypoint);
                    Path.Reverse();
                    Path.Add(selectedWaypoint);
                    
                }
                if (selectedWaypoint.Twin)
                {
                    if (Visited.Contains(selectedWaypoint.Twin))
                    {
                        selectedWaypoint.SetIndexTrail(1);
                        CreatePath(Path, selectedWaypoint.Twin);
                        Path.Reverse();
                        Path.Add(selectedWaypoint);
                        selectedWaypoint.Twin.SetIndexTrail(1);
                   }
                }
                
                else
                {   
                    /*
                    Debug.Log("OUTOOOOO");
                    ClearPath();
                    AStarCreate(GameController.instance.SelectedUnit.Position,selectedWaypoint);*/
                }
            }
            
        }
        else
        {
            ClearPath();
            Visited.Clear();
        }

        if (GameController.SelectedUnit != null)
        {
            if (!GameController.SelectedUnit.AsPlayed)
            {
                Move = true;
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
    
    public void AStarCreate(Waypoint Start,float cost)
    {
        if (Start == null)
        {
            return;
        }
        
        Start.MinCostToStart = 0;
        PQueue.Add(Start);
        while (PQueue.Any())
        {
            var current = PQueue.First();
            PQueue.Remove(current);
            
            foreach (Waypoint w in current.Neighbors.OrderBy(x => x.GetComponent<Waypoint>().mouvCost))
            {
                if (w.visitedDijstra || w.HeightType == HeightType.River || w.HeightType == HeightType.DeepWater || w.HeightType == HeightType.ShallowWater || w.Occupied)
                {
                    continue;
                }
                
                var nextCost = current.MinCostToStart + w.mouvCost;
                
                if (nextCost > cost)
                {
                    continue;
                }
                
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
        ClearRange(Visited);
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
            var current = PQueue.First();
            PQueue.Remove(current);
            
            if (current == End)
            {
                break;
            }
            
            current.Neighbors.OrderBy(x => x.GetComponent<Waypoint>().mouvCost);
            foreach (Waypoint w in current.Neighbors)
            {
                if(w==null)
                    continue;
                
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
                else
                {
                    Debug.Log(current.name+" "+nextCost+" "+w.MinCostToStart);
                }
            }
            current.visitedDijstra = true;
            PQueue = PQueue.OrderBy(x => x.MinCostToStart).ToList();
        }
        
        End.SetIndexTrail(1);
        CreatePath(Path, End);
        Path.Reverse();
        Path.Add(End);     

    }
    
    private void CreatePath(List<Waypoint> TmpPath, Waypoint W)
    {
        if (W.NearestToStart == null)
        {
            Debug.Log("STAPH");
            return;
        }
        TmpPath.Add(W.NearestToStart);
        
        W.EnableTrail(W.NearestToStart);
        CreatePath(TmpPath, W.NearestToStart);
        
    }

    
    
    private void MoveWithPath(Waypoint NewPos)
    {
        if (!NewPos.Occupied)
        {
            
            if(!Visited.Contains(NewPos))
            {
                if (!Visited.Contains(NewPos.Twin))
                {
                    return;
                }
            }
            
            Unit unit = GameController.SelectedUnit;
            unit.Position.Occupied = false;
            NewPos.Occupied = true;
            unit.Position = NewPos;
            unit.prefab.transform.position = new Vector3(NewPos.transform.position.x,
                NewPos.transform.position.y + NewPos.elevation, NewPos.transform.position.z);
            if (NewPos.Twin)
            {
                unit.Twin.transform.position = new Vector3(NewPos.Twin.transform.position.x,
                    NewPos.Twin.transform.position.y + NewPos.Twin.elevation, NewPos.Twin.transform.position.z);
                unit.Twin.SetActive(true);
            }
            else
            {
                unit.Twin.SetActive(false);
            }

            
            Move = false;
            unit.AsPlayed = true;
        }
    }
    
    public void Attacking(Unit attacker)
    {
        GameController GC = GameController.instance;

        //List<Unit> enemyUnitList = new List<Unit>();

        foreach (Waypoint wp in GC.SelectedUnit.Position.Neighbors)
        {
            if (wp.Occupied)
            {
                Civilisation enemyCiv = GC.GetOtherCivilisation();
                
                foreach (Unit enemyUnit in enemyCiv.Units)
                {
                    if (enemyUnit.Position == wp)
                    {
                        //enemyUnitList.Add(enemyUnit);
                        enemyUnit.Position.EnableHighlight();
                        highlightedWayPoint.Add(enemyUnit.Position);

                        attackAvailable = true;
                        Move = false;
                    }
                }
            }
        }
    }

    public void DoAttack(Unit attackingUnit, Unit defendingUnit)
    {
        GameController GC = GameController.instance;
        Civilisation enemyCiv = GC.GetOtherCivilisation();

        if (attackingUnit.index == "Warrior" && defendingUnit.index == "Archer")
        {
            //Damage
            defendingUnit.HP -= 2 / 2;
            attackingUnit.HP -= 2 * 2;

            attackingUnit.AsPlayed = true;

            CheckHP(attackingUnit, defendingUnit);

            Debug.Log("Warrior contre Archer");
            
        }
        if (attackingUnit.index == "Warrior" && defendingUnit.index == "Warrior")
        {
            //Damage
            attackingUnit.HP -= 2;
            defendingUnit.HP -= 2;

            attackingUnit.AsPlayed = true;

            CheckHP(attackingUnit, defendingUnit);

            Debug.Log("Warrior contre Warrior");
        }
        if (attackingUnit.index == "Warrior" && defendingUnit.index == "Rider")
        {
            //Damage
            defendingUnit.HP -= 2 * 2;
            attackingUnit.HP -= 2 / 2;

            attackingUnit.AsPlayed = true;

            CheckHP(attackingUnit, defendingUnit);


            Debug.Log("Warrior contre Rider");

        }


        if (attackingUnit.index == "Archer" && defendingUnit.index == "Archer")
        {
            //Damage
            defendingUnit.HP -= 2;
            attackingUnit.HP -= 2;

            attackingUnit.AsPlayed = true;

            CheckHP(attackingUnit, defendingUnit);

            Debug.Log("Archer contre Archer");
        }
        if (attackingUnit.index == "Archer" && defendingUnit.index == "Warrior")
        {
            //Damage
            defendingUnit.HP -= 2 * 2;
            attackingUnit.HP -= 2 / 2;

            attackingUnit.AsPlayed = true;

            CheckHP(attackingUnit, defendingUnit);

            Debug.Log("Archer contre Warrior");
        }
        if (attackingUnit.index == "Archer" && defendingUnit.index == "Rider")
        {
            //Damage
            defendingUnit.HP -= 2 / 2;
            attackingUnit.HP -= 2 * 2;

            attackingUnit.AsPlayed = true;

            CheckHP(attackingUnit, defendingUnit);

            Debug.Log("Archer contre Rider");
        }

        
        if (attackingUnit.index == "Rider" && defendingUnit.index == "Archer")
        {
            //Damage
            defendingUnit.HP -= 2 * 2;
            attackingUnit.HP -= 2 / 2;

            attackingUnit.AsPlayed = true;

            CheckHP(attackingUnit, defendingUnit);

            Debug.Log("Rider contre Archer");
        }
        if (attackingUnit.index == "Rider" && defendingUnit.index == "Warrior")
        {
            //Damage
            defendingUnit.HP -= 2 / 2;
            attackingUnit.HP -= 2 * 2;

            attackingUnit.AsPlayed = true;

            CheckHP(attackingUnit, defendingUnit);

            Debug.Log("Rider contre Warrior");
        }
        if (attackingUnit.index == "Rider" && defendingUnit.index == "Rider")
        {
            //Damage
            defendingUnit.HP -= 2;
            attackingUnit.HP -= 2;

            attackingUnit.AsPlayed = true;

            CheckHP(attackingUnit, defendingUnit);

            Debug.Log("Rider contre Rider");
        }
    }
    
    public void CheckHP(Unit attackingUnit, Unit defendingUnit)
    {
        GameController GC = GameController.instance;
        Civilisation enemyCiv = GC.GetOtherCivilisation();

        if (defendingUnit.HP <= 0)
        {
            enemyCiv.Units.Remove(defendingUnit);

            if (defendingUnit.Twin != null)
            {
                DestroyImmediate(defendingUnit.Twin);
            }
            DestroyImmediate(defendingUnit.prefab);
        }

        if (attackingUnit.HP <= 0)
        {
            GC.CurrentCiv.Units.Remove(attackingUnit);

            if (attackingUnit.Twin != null)
            {
                DestroyImmediate(attackingUnit.Twin);
            }
            DestroyImmediate(attackingUnit.prefab);
        }
    }

    public void ClearRange(List<Waypoint> list)
    {
        foreach (Waypoint waypoint in list)
        {
            if (waypoint.left)
            {
                if (list.Contains(waypoint.left.GetComponent<Waypoint>()))
                {
                    waypoint.RangeRenderer[0].gameObject.SetActive(false);
                    if(waypoint.Twin)
                        waypoint.Twin.RangeRenderer[0].gameObject.SetActive(false);
                }
            }
            if (waypoint.right)
            {
                if (list.Contains(waypoint.right.GetComponent<Waypoint>()))
                {
                    waypoint.RangeRenderer[3].gameObject.SetActive(false);
                    if(waypoint.Twin)
                        waypoint.Twin.RangeRenderer[3].gameObject.SetActive(false);
                }
            }
            if (waypoint.leftTop)
            {
                if (list.Contains(waypoint.leftTop.GetComponent<Waypoint>()))
                {
                    waypoint.RangeRenderer[2].gameObject.SetActive(false);
                    if(waypoint.Twin)
                        waypoint.Twin.RangeRenderer[2].gameObject.SetActive(false);
                }
            }
            if (waypoint.rightTop)
            {
                if (list.Contains(waypoint.rightTop.GetComponent<Waypoint>()))
                {
                    waypoint.RangeRenderer[5].gameObject.SetActive(false);
                    if(waypoint.Twin)
                        waypoint.Twin.RangeRenderer[5].gameObject.SetActive(false);
                }
            }
            if (waypoint.leftBot)
            {
                if (list.Contains(waypoint.leftBot.GetComponent<Waypoint>()))
                {
                    waypoint.RangeRenderer[1].gameObject.SetActive(false);
                    if(waypoint.Twin)
                        waypoint.Twin.RangeRenderer[1].gameObject.SetActive(false);
                }
            }
            if (waypoint.rightBot)
            {
                if (list.Contains(waypoint.rightBot.GetComponent<Waypoint>()))
                {
                    waypoint.RangeRenderer[4].gameObject.SetActive(false);
                    if(waypoint.Twin)
                        waypoint.Twin.RangeRenderer[4].gameObject.SetActive(false);
                }
            }
            
           waypoint.RangeContainer.SetActive(true);
           if(waypoint.Twin)
               waypoint.Twin.RangeContainer.SetActive(true);
        }
    }

    public void ClearPath(){
        PQueue.Clear();
        Path.Clear();
        foreach (Waypoint w in Visited)
        {
            w.visitedDijstra = false;
            w.NearestToStart = null;
            w.MinCostToStart = int.MaxValue;
            w.ResetRange();
            if (w.Twin)
            {
                Waypoint W = w.Twin;
                W.visitedDijstra = false;
                W.NearestToStart = null;
                W.MinCostToStart = int.MaxValue;
                W.ResetRange();
            }
        }
        foreach (Waypoint p in Visited)
        {
            p.DisableTrail();
            if(p.Twin)
                p.Twin.DisableTrail(); 
        }
    }

    public void CreateCity(Waypoint position)
    {
        if(position.HeightType == HeightType.River || position.HeightType == HeightType.DeepWater ||position.HeightType == HeightType.ShallowWater)
            return;
        ManageCity CityObj = Instantiate(cityPref, position.transform);
        CityObj.transform.localPosition = new Vector3(0,position.elevation, 0);
        City newCity = GameController.CurrentCiv.CreateCity(position);
        CityObj.Colors.color = newCity.civColor; 
        CityObj.ThisCity = newCity;
        CityObj.NameCity.text = newCity.NameCity;
        CityObj.owner = GameController.CurrentCiv;
        //Add sound elements
                    
        newCity.buildSound = this.buildSound;
        newCity.soldierSound = this.soldierSound;
        newCity.clickSound = this.clickSound;
        //TWIN
        if (position.AsTwin || position.IsTwin)
        {
            ManageCityClone CityObjC = Instantiate(cityClonePref, position.Twin.transform);
            CityObjC.transform.localPosition = new Vector3(0, position.elevation, 0);
            CityObjC.ManageRef = CityObj;
            CityObjC.NameCity.text = newCity.NameCity;
            CityObjC.Colors.color = newCity.civColor;

        }
        //
        if (position.LOD)
        {
            position.LOD.SetActive(false);
        }
        if (position.Twin)
        {
            if (position.Twin.LOD)
            {
                position.Twin.LOD.SetActive(false);
            }
        }

        GameController.SelectedCity = newCity;

    }

    private bool IsPointerOverUI()
    {
        PointerEventData eventDataCurrent = new PointerEventData(EventSystem.current);
        eventDataCurrent.position = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrent,results);
        return results.Count > 0;
    }
    
}

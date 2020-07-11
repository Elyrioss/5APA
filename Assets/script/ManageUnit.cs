using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ManageUnit : MonoBehaviour
{

    public Unit Unit;
    
    private bool CanMove;
    private Waypoint EndPos;
    public Image Colors;
    public Civilisation Owner;
    public Slider Hp;
    public GameObject UnitModel;
    public GameObject BoatModel;
    public AudioSource Source;
    
    public void SelectUnit()
    {
        GameController GC = GameController.instance;
        if(Owner!=GC.CurrentCiv)
            return;
        
        GameController.instance.SelectedUnit = Unit;
        GC.cityMenu.ShowUnit(Unit);
        
        if (!Unit.AsPlayed)
        {
            GC.MapControllerScript.ClearPath();
            GC.MapControllerScript.Visited.Clear();           
            GC.MapControllerScript.AStarCreate(Unit.Position,Unit.mouvementPoints);          
        }
    }

    private void Update()
    {
        if (Unit != null)
        {
            Hp.value = (float)Unit.HP / Unit.MAXHP;
        }
    }

    /*
    // Update is called once per frame
    void Update()
    {
       if (!CanMove || Mapcontroller._cities[ThisCity].construction.Units[ThisUnit].AsPlayed)
        {
            return;
        }
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = CurrentCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) // Ici on va gérer toutes les possibilités de click d'éléments
            {
                if (!hit.transform.parent.gameObject.GetComponent<Waypoint>())
                    return;

                EndPos = hit.transform.parent.gameObject.GetComponent<Waypoint>();
                AStarCreate(Mapcontroller._cities[ThisCity].construction.Units[ThisUnit].Position, EndPos);
            }
        }
    }

    public void SelectUnit()
    {
        CanMove = true;
    }


    //Deplacement fonction
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
            PQueue = PQueue.OrderBy(x => x.MinCostToStart).ToList();
        }
        Debug.Log("OUUUUUUT");
        CreatePath(Path, End);
        Path.Reverse();
        Path.Add(End);       
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


    private void DrawPath(List<Waypoint> DPath)
    {
        foreach (Waypoint w in DPath)
        {
            w.CivColor = Color.red;
            w.EnableWaypoint();
        }
    }

    private void MoveWithPath(List<Waypoint> DPath)
    {
        CurrentCost = 0;
        foreach(Waypoint w in DPath)
        {
            CurrentCost += w.mouvCost;
            if(CurrentCost <= Cost)
            {
                NewPos = w;
            }
            else
            {
                break;
            }
        }
        Mapcontroller._cities[ThisCity].construction.Units[ThisUnit].Position = NewPos;
        this.gameObject.transform.position = new Vector3(NewPos.transform.position.x, NewPos.transform.position.y + NewPos.elevation, NewPos.transform.position.z);
        //clear
        PQueue.Clear();
        Path.Clear();
        CanMove = false;
        Mapcontroller._cities[ThisCity].construction.Units[ThisUnit].AsPlayed = true;
    }
*/
}

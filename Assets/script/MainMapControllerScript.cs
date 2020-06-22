using System;
using System.Collections.Generic;
using UnityEngine;

public class MainMapControllerScript : MonoBehaviour
{
    Camera _camera = null;
    Waypoint selectedWaypoint = null;
    public GameObject cityPref;
    public List<City> _cities = new List<City>();
    public tileMapManager Map;
    public Transform Raystarter;
    ///TESTSUI
    private bool StartingCity;
    public GameObject FileUI;
    private Animator Anim;  
    public bool CanRaycast = true;
    public AnimationCurve CamHeightCurve;
    private float PreviousHeight;
    
    public TileMapPos currentChunk=null;
    public List<TileMapPos> CurrentChunks = new List<TileMapPos>();
    private bool changeCull = true;
    
    void Awake()
    {
        _camera = GetComponent<Camera>();
        Anim = FileUI.GetComponent<Animator>();
        PreviousHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Raystarter.position, Raystarter.TransformDirection(Vector3.down), out hit, 1000))
        {
            Debug.DrawRay(Raystarter.position, Raystarter.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            if (hit.transform.parent.GetComponent<Waypoint>())
            {
                Waypoint w = hit.transform.parent.GetComponent<Waypoint>();
                if (w.Chunk != currentChunk)
                {
                    CurrentChunks=HeightCull(w.Chunk, (int) CamHeightCurve.Evaluate(PreviousHeight));
                    currentChunk = w.Chunk;
                    changeCull = true;
                }
            }
        }

        if ((int) CamHeightCurve.Evaluate(PreviousHeight) != (int) CamHeightCurve.Evaluate(transform.position.y))
        {
            Debug.Log("heigt");
            PreviousHeight = transform.position.y;
            CurrentChunks=HeightCull(currentChunk, (int) CamHeightCurve.Evaluate(PreviousHeight));
            changeCull = true;
        }
        
        if (changeCull)
        {                   
            foreach (TileMapPos chunk in Map.Chunks)
            {
                if (CurrentChunks.Contains(chunk))
                {
                    chunk.gameObject.SetActive(true);
                }                       
                else
                {
                    chunk.gameObject.SetActive(false);
                }
            }
            currentChunk.gameObject.SetActive(true);
            changeCull = false;
                    
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SetLeft();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetRight();
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
                    _cities.Add(new City(selectedWaypoint,Color.red));
                    var CityObj = Instantiate(cityPref, hit.transform);
                    CityObj.transform.localPosition = new Vector3(0,0, 0);
                    CityObj.GetComponent<ManageCity>().ThisCity = new City(selectedWaypoint, Color.red);
                    _cities.Add(cityPref.GetComponent<ManageCity>().ThisCity);
                    StartingCity = true;
                    CanRaycast = false;
                }
                /*else if (hit.transform.gameObject.tag == "City") // Si c'est à son tour et qu'il click sur une de ses city
                {
                    FileUI.SetActive(true);
                    //Anim.SetBool("OUT", true);
                }*/
            }
        }
    }

    public void SetRight()
    {
        for (int i = Map.Chunks.Count-2*Map.chunkY; i < Map.chunkX*Map.chunkY; i++)
        {
            Map.Chunks[i].transform.localPosition = new Vector3(0f,Map.Chunks[i].transform.localPosition.y,Map.Chunks[i].transform.localPosition.z);         
        } 
    }

    public void SetLeft()
    {  
        float val = Map.chunkX - (Map.chunkX)/4;      
        for (int j = 1; j < val; j++)
        {
            float Right = -1 * Map.Chunks[Map.Chunks.Count-Map.chunkY*j].transform.GetChild(0).localPosition.x;
            for (int i = Map.Chunks.Count-j*Map.chunkY; i < Map.Chunks.Count-Map.chunkY*(j-1); i++)
            {
                Map.Chunks[i].transform.localPosition = new Vector3(Right-(j*Map.sizeChunkX*1.732f),Map.Chunks[i].transform.localPosition.y,Map.Chunks[i].transform.localPosition.z);         
            }
        }
    }

    public List<TileMapPos> HeightCull(TileMapPos Chunk,int X)
    {
        List<TileMapPos> result = new List<TileMapPos>();
        if (X <= 1)
            return result;

        if (Chunk.Bot)
        {
            result.Add(Chunk.Bot);
        }
                    
        TileMapPos Left = Chunk, Right = Chunk;      
        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < i+(X-1); j++)
            {
                Left = Left.Left;
                result.Add(Left);
                Right = Right.Right;
                result.Add(Right);
            }

            Chunk = Chunk.Top;
            result.Add(Chunk);
            Left = Chunk;
            Right = Chunk;
        }

        return result;
    }

}

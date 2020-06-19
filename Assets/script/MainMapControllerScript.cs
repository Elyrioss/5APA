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

    
    
    void Awake()
    {
        _camera = GetComponent<Camera>();

        Anim = FileUI.GetComponent<Animator>();
        int j = 1;
        for (int i = Map.Chunks.Count-Map.chunkY; i < Map.chunkX*Map.chunkY; i++)
        {
            Map.Chunks[i].transform.localPosition = new Vector3(1,Map.Chunks[i].transform.localPosition.y,Map.Chunks[i].transform.localPosition.z);
            j=(j+1)%2;
        }
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        if (Physics.Raycast(Raystarter.position, Raystarter.TransformDirection(Vector3.down), out hit, 1000))
        {
            Debug.DrawRay(Raystarter.position, Raystarter.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            if(hit.transform.parent.GetComponent<Waypoint>())
                Debug.Log(hit.transform.parent.GetComponent<Waypoint>().name);
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
                    var CityObj = Instantiate(cityPref, new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z), Quaternion.identity);
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


}

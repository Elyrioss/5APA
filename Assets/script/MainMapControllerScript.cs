using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMapControllerScript : MonoBehaviour
{
    Camera _camera = null;
    Waypoint selectedWaypoint = null;
    public GameObject cityPref;
    public List<City> _cities = new List<City>();


    ///TESTSUI
    private bool StartingCity;
    public GameObject FileUI;
    private Animator Anim;

    public bool CanRaycast = true;

    
    
    void Awake()
    {
        _camera = GetComponent<Camera>();

        Anim = FileUI.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) // Ici on va gérer toutes les possibilités de click d'éléments
            {
                if (!StartingCity) // Création de la ville du début. 
                {
                    selectedWaypoint = hit.transform.parent.gameObject.GetComponent<Waypoint>();
                    _cities.Add(new City(selectedWaypoint,Color.yellow));
                    Instantiate(cityPref, new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z), Quaternion.identity);
                    StartingCity = true;
                    CanRaycast = false;
                }
                else if (hit.transform.gameObject.tag == "City") // Si c'est à son tour et qu'il click sur une de ses city
                {
                    Debug.Log("On doit créer l'UI de la bonne ville");
                    FileUI.SetActive(true);
                    //Anim.SetBool("OUT", true);
                }
            }
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMapControllerScript : MonoBehaviour
{
    Camera _camera = null;
    Waypoint selectedWaypoint = null;
    public GameObject cityPref;
    private List<City> _cities = new List<City>();


    ///TESTSUI
    private bool StartingCity;
    public GameObject FileUI;
    private Animator Anim;

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
            if (Physics.Raycast(ray, out hit))
            {
                if (!StartingCity)
                {
                    selectedWaypoint = hit.transform.gameObject.GetComponent<Waypoint>();
                    _cities.Add(new City(selectedWaypoint));
                    Instantiate(cityPref, new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z), Quaternion.identity);
                    StartingCity = true;
                    Debug.Log("aaaaaaaaaa");
                }
                else if (hit.transform.gameObject.tag == "City")
                {
                    Debug.Log("Create UI!!!!");
                    FileUI.SetActive(true);
                    //Anim.SetBool("OUT", true);
                }
            }
        }
    }


}

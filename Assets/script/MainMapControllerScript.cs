using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMapControllerScript : MonoBehaviour
{
    Camera _camera = null;
    Waypoint selectedWaypoint = null;
    private List<City> _cities = new List<City>();
    void Awake()
    {
        _camera = GetComponent<Camera>();
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
                selectedWaypoint = hit.transform.gameObject.GetComponent<Waypoint>();
                _cities.Add(new City(selectedWaypoint));         
                Debug.Log("aaaaaaaaaa");
            }
        }
    }


}

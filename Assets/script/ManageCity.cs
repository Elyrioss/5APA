using UnityEngine;
using UnityEngine.UI;

public class ManageCity : MonoBehaviour
{
    // Start is called before the first frame update

    public City ThisCity;

    public GameObject CityMenu;

    private GameObject Population;
    private GameObject Nourriture;
    private GameObject Production;
    private GameObject Or;
    void Start()
    {
        CityMenu = GameObject.FindGameObjectWithTag("CanevasCity");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SelectCity()
    {
        CityMenu.transform.Find("CityMenu").gameObject.SetActive(true); 
        Population = CityMenu.transform.Find("CityMenu/CityPannel/Population").gameObject;
        Population.GetComponent<Text>().text = "Population : " + ThisCity.population;
        Nourriture = CityMenu.transform.Find("CityMenu/CityPannel/Nourriture").gameObject;
        Nourriture.GetComponent<Text>().text = "Nourriture : " + ThisCity.food;
        Production = CityMenu.transform.Find("CityMenu/CityPannel/Production").gameObject;
        Production.GetComponent<Text>().text = "Production : " + ThisCity.production;
        Or = CityMenu.transform.Find("CityMenu/CityPannel/Or").gameObject;
        Or.GetComponent<Text>().text = "Or : " + ThisCity.gold;
        CityMenu.SetActive(true);
    }

}

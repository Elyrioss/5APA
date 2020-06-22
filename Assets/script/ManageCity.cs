using UnityEngine;
using UnityEngine.UI;

public class ManageCity : MonoBehaviour
{
    // Start is called before the first frame update

    //public City ThisCity;
    public int ThisCity; //Index of List _cities
    public MainMapControllerScript Mapcontroller;
    public GameController Controller;
    public GameObject CityMenu;

    private GameObject Population;
    private GameObject Nourriture;
    private GameObject Production;
    private GameObject Or;

    //UI Buildings
    private GameObject FoodBatButton;
    private GameObject FoodBatCounter;
    private GameObject FoodBatNumber;
    private GameObject ProducBatButton;
    private GameObject ProducBatCounter;
    private GameObject ProducBatNumber;
    private GameObject GoldBatButton;
    private GameObject GoldBatCounter;
    private GameObject GoldBatNumber;

    void Start()
    {
        Controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        CityMenu = GameObject.FindGameObjectWithTag("CanevasCity");
        Mapcontroller = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMapControllerScript>();
        Population = CityMenu.transform.Find("CityMenu/CityPannel/Population").gameObject;
        Nourriture = CityMenu.transform.Find("CityMenu/CityPannel/Nourriture").gameObject;
        Production = CityMenu.transform.Find("CityMenu/CityPannel/Production").gameObject;
        Or = CityMenu.transform.Find("CityMenu/CityPannel/Or").gameObject;
        FoodBatButton = CityMenu.transform.Find("CityMenu/BuildingPannel/BttnBulding/FoodBat").gameObject;
        FoodBatCounter = CityMenu.transform.Find("CityMenu/BuildingPannel/BttnBulding/FoodBat/Nbturn").gameObject;
        FoodBatNumber = CityMenu.transform.Find("CityMenu/BuildingPannel/ListOfBuilding/FoodBat/Number").gameObject;
        ProducBatButton = CityMenu.transform.Find("CityMenu/BuildingPannel/BttnBulding/ProductBat").gameObject;
        ProducBatCounter = CityMenu.transform.Find("CityMenu/BuildingPannel/BttnBulding/ProductBat/Nbturn").gameObject;
        ProducBatNumber = CityMenu.transform.Find("CityMenu/BuildingPannel/ListOfBuilding/ProductBat/Number").gameObject;
        GoldBatButton = CityMenu.transform.Find("CityMenu/BuildingPannel/BttnBulding/GoldBat").gameObject;
        GoldBatCounter = CityMenu.transform.Find("CityMenu/BuildingPannel/BttnBulding/GoldBat/Nbturn").gameObject;
        GoldBatNumber = CityMenu.transform.Find("CityMenu/BuildingPannel/ListOfBuilding/Banks/Number").gameObject;
    }


    public void SelectCity()
    {
        //Debug.Log("GOOOOOO et food : " + Mapcontroller._cities[ThisCity].food);
        Controller.SelectedCity = this.gameObject.GetComponent<ManageCity>();
        CityMenu.transform.Find("CityMenu").gameObject.SetActive(true);   
        //CityPannel
        Population.GetComponent<Text>().text = "Population : " + Mapcontroller._cities[ThisCity].population;
        Nourriture.GetComponent<Text>().text = "Nourriture : " + Mapcontroller._cities[ThisCity].food;
        Production.GetComponent<Text>().text = "Production : " + Mapcontroller._cities[ThisCity].production;
        Or.GetComponent<Text>().text = "Or : " + Mapcontroller._cities[ThisCity].gold;
        //BuildingPannel
        if (Mapcontroller._cities[ThisCity].construction.FoodConstruction)
        {
            FoodBatButton.GetComponent<Image>().color = Color.black;
        }
        else
        {
            FoodBatButton.GetComponent<Image>().color = Color.blue;
        }
        FoodBatCounter.GetComponent<Text>().text = "" + Mapcontroller._cities[ThisCity].construction.FoodCounter;
        FoodBatNumber.GetComponent<Text>().text = "" + Mapcontroller._cities[ThisCity].construction.NumberOfFoodBat;

        if (Mapcontroller._cities[ThisCity].construction.ProducConstruction)
        {
            ProducBatButton.GetComponent<Image>().color = Color.black;
        }
        else
        {
            ProducBatButton.GetComponent<Image>().color = Color.blue;
        }
        ProducBatCounter.GetComponent<Text>().text = "" + Mapcontroller._cities[ThisCity].construction.ProducCounter;
        ProducBatNumber.GetComponent<Text>().text = "" + Mapcontroller._cities[ThisCity].construction.NumberOfProducBat;


        if (Mapcontroller._cities[ThisCity].construction.GoldConstruction)
        {
            GoldBatButton.GetComponent<Image>().color = Color.black;
        }
        else
        {
            GoldBatButton.GetComponent<Image>().color = Color.blue;
        }
        GoldBatCounter.GetComponent<Text>().text = "" + Mapcontroller._cities[ThisCity].construction.GoldCounter;
        GoldBatNumber.GetComponent<Text>().text = "" + Mapcontroller._cities[ThisCity].construction.NumberOfGoldBat;

        //CityMenu.SetActive(true);
    }

    public void CreateFoodBat()
    {
        if (Mapcontroller._cities[ThisCity].ThisCityAction || Mapcontroller._cities[ThisCity].construction.FoodConstruction) // Si action deja effectué ou batiment deja en construction
        {
            return;
        }
        Mapcontroller._cities[ThisCity].construction.FoodConstruction = true;
        Mapcontroller._cities[ThisCity].ThisCityAction = true;
        FoodBatButton.GetComponent<Image>().color = Color.black;
    }

    public void CreateProducBat()
    {
        if (Mapcontroller._cities[ThisCity].ThisCityAction || Mapcontroller._cities[ThisCity].construction.ProducConstruction) // Si action deja effectué ou batiment deja en construction
        {
            return;
        }
        Mapcontroller._cities[ThisCity].construction.ProducConstruction = true;
        Mapcontroller._cities[ThisCity].ThisCityAction = true;
        ProducBatButton.GetComponent<Image>().color = Color.black;
    }

    public void CreateGoldBat()
    {
        if (Mapcontroller._cities[ThisCity].ThisCityAction || Mapcontroller._cities[ThisCity].construction.GoldConstruction) // Si action deja effectué ou batiment deja en construction
        {
            return;
        }
        Mapcontroller._cities[ThisCity].construction.GoldConstruction = true;
        Mapcontroller._cities[ThisCity].ThisCityAction = true;
        GoldBatButton.GetComponent<Image>().color = Color.black;
    }

}

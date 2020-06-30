using UnityEngine;
using UnityEngine.UI;

public class ManageCity : MonoBehaviour
{
    // Start is called before the first frame update

    public int ThisCity; //Index of List _cities in MainMapController
    public MainMapControllerScript Mapcontroller;
    public GameController Controller;
    public GameObject CityMenu;
    public GameObject CityName;

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
    private GameObject ExtBatButton;
    private GameObject ExtBatCounter;
    private GameObject ExtBatNumber;
    private GameObject WarriorButton;
    private GameObject WarriorCount;
    private GameObject WarriorNumber;
    private GameObject ArcherButton;
    private GameObject ArcherCount;
    private GameObject ArcherNumber;
    private GameObject RiderButton;
    private GameObject RiderCount;
    private GameObject RiderNumber;
/*

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
        ExtBatNumber = CityMenu.transform.Find("CityMenu/BuildingPannel/ListOfBuilding/Banks/Number").gameObject;
        ExtBatButton = CityMenu.transform.Find("CityMenu/BuildingPannel/BttnBulding/Extension").gameObject;
        ExtBatCounter = CityMenu.transform.Find("CityMenu/BuildingPannel/BttnBulding/Extension/Nbturn").gameObject;
        GoldBatNumber = CityMenu.transform.Find("CityMenu/BuildingPannel/ListOfBuilding/Extension/Number").gameObject;
        WarriorButton = CityMenu.transform.Find("CityMenu/BuildingPannel/BttnUnit/Warrior").gameObject;
        WarriorCount = CityMenu.transform.Find("CityMenu/BuildingPannel/BttnUnit/Warrior/Nbturn").gameObject;
        WarriorNumber = CityMenu.transform.Find("CityMenu/BuildingPannel/ListOfBuilding/Warrior/Number").gameObject;
        ArcherButton = CityMenu.transform.Find("CityMenu/BuildingPannel/BttnUnit/Archer").gameObject;
        ArcherCount = CityMenu.transform.Find("CityMenu/BuildingPannel/BttnUnit/Archer/Nbturn").gameObject;
        ArcherNumber = CityMenu.transform.Find("CityMenu/BuildingPannel/ListOfBuilding/Archer/Number").gameObject;
        RiderButton = CityMenu.transform.Find("CityMenu/BuildingPannel/BttnUnit/Rider").gameObject;
        RiderCount = CityMenu.transform.Find("CityMenu/BuildingPannel/BttnUnit/Rider/Nbturn").gameObject;
        RiderNumber = CityMenu.transform.Find("CityMenu/BuildingPannel/ListOfBuilding/Rider/Number").gameObject;
    }


    public void SelectCity()
    {
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


        if (Mapcontroller._cities[ThisCity].construction.ExtensionConstruction)
        {
            ExtBatButton.GetComponent<Image>().color = Color.black;
        }
        else
        {
            ExtBatButton.GetComponent<Image>().color = Color.blue;
        }
        ExtBatCounter.GetComponent<Text>().text = "" + Mapcontroller._cities[ThisCity].construction.ExtensionCounter;
        ExtBatNumber.GetComponent<Text>().text = "" + Mapcontroller._cities[ThisCity].construction.NumberOfExtension;


        if (Mapcontroller._cities[ThisCity].construction.WarriorConstruction)
        {
            WarriorButton.GetComponent<Image>().color = Color.black;
        }
        else
        {
            WarriorButton.GetComponent<Image>().color = Color.blue;
        }
        WarriorCount.GetComponent<Text>().text = "" + Mapcontroller._cities[ThisCity].construction.WarriorCounter;
        WarriorNumber.GetComponent<Text>().text = "" + Mapcontroller._cities[ThisCity].construction.NumberOfWarrior;



        //CityMenu.SetActive(true);
    }

    public bool CreateFoodBat()
    {
        if (Mapcontroller._cities[ThisCity].ThisCityAction || Mapcontroller._cities[ThisCity].construction.FoodConstruction) // Si action deja effectué ou batiment deja en construction
        {
            return false;
        }
        Mapcontroller._cities[ThisCity].construction.FoodConstruction = true;
        Mapcontroller._cities[ThisCity].ThisCityAction = true;
        FoodBatButton.GetComponent<Image>().color = Color.black;
        return true;
    }

    public bool CreateProducBat()
    {
        if (Mapcontroller._cities[ThisCity].ThisCityAction || Mapcontroller._cities[ThisCity].construction.ProducConstruction) // Si action deja effectué ou batiment deja en construction
        {
            return false;
        }
        Mapcontroller._cities[ThisCity].construction.ProducConstruction = true;
        Mapcontroller._cities[ThisCity].ThisCityAction = true;
        ProducBatButton.GetComponent<Image>().color = Color.black;
        return true;
    }

    public bool CreateGoldBat()
    {
        if (Mapcontroller._cities[ThisCity].ThisCityAction || Mapcontroller._cities[ThisCity].construction.GoldConstruction) // Si action deja effectué ou batiment deja en construction
        {
            return false;
        }
        Mapcontroller._cities[ThisCity].construction.GoldConstruction = true;
        Mapcontroller._cities[ThisCity].ThisCityAction = true;
        GoldBatButton.GetComponent<Image>().color = Color.black;
        return true;
    }

    public bool CreateExtensionBat()
    {
        if (Mapcontroller._cities[ThisCity].ThisCityAction || Mapcontroller._cities[ThisCity].construction.ExtensionConstruction) // Si action deja effectué ou batiment deja en construction
        {
            return false;
        }
        CityName.SetActive(false);
        Mapcontroller.TmpCityIndex = ThisCity;
        Mapcontroller.CanRaycast = true;
        Mapcontroller.Extension = true;
        Mapcontroller.TmpManageCity = this.gameObject.GetComponent<ManageCity>();
        return true;
    }


    public bool CreateWarrior()
    {
        if (Mapcontroller._cities[ThisCity].ThisCityAction || Mapcontroller._cities[ThisCity].construction.WarriorConstruction) // Si action deja effectué ou batiment deja en construction
        {
            return false;
        }
        CityName.SetActive(false);
        Mapcontroller.TmpCityIndex = ThisCity;
        Mapcontroller.CanRaycast = true;
        Mapcontroller.Warrior = true;
        Mapcontroller.TmpManageCity = this.gameObject.GetComponent<ManageCity>();
        return true;
    }

    public bool CreateArcher()
    {
        if (Mapcontroller._cities[ThisCity].ThisCityAction || Mapcontroller._cities[ThisCity].construction.ArcherConstruction) // Si action deja effectué ou batiment deja en construction
        {
            return false;
        }
        CityName.SetActive(false);
        Mapcontroller.TmpCityIndex = ThisCity;
        Mapcontroller.CanRaycast = true;
        Mapcontroller.Archer = true;
        Mapcontroller.TmpManageCity = this.gameObject.GetComponent<ManageCity>();
        return true;
    }

    public bool CreateRider()
    {
        if (Mapcontroller._cities[ThisCity].ThisCityAction || Mapcontroller._cities[ThisCity].construction.RiderConstruction) // Si action deja effectué ou batiment deja en construction
        {
            return false;
        }
        CityName.SetActive(false);
        Mapcontroller.TmpCityIndex = ThisCity;
        Mapcontroller.CanRaycast = true;
        Mapcontroller.Rider = true;
        Mapcontroller.TmpManageCity = this.gameObject.GetComponent<ManageCity>();
        return true;
    }


*/

}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageCity : MonoBehaviour
{
    // Start is called before the first frame update

    public int ThisCity; //Index of List _cities in MainMapController
    public MainMapControllerScript Mapcontroller;
    public GameController Controller;
    public CityMenu CityMenu;

    private TextMeshProUGUI Population;
    private TextMeshProUGUI Nourriture;
    private TextMeshProUGUI Production;
    private TextMeshProUGUI Or;


    void Start()
    {
        Controller = GameController.instance;
        CityMenu = Controller.cityMenu;
        
        Mapcontroller = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMapControllerScript>();
        Population = CityMenu.Population;
        Nourriture = CityMenu.Food;
        Production = CityMenu.Production;
        Or = CityMenu.Gold;
    }


    public void SelectCity()
    {
        Controller.SelectedCity = Mapcontroller._cities[ThisCity];
        CityMenu.ShowCity();
        CityMenu.gameObject.SetActive(true);   
        //CityPannel
        Population.text = "Population : " + Mapcontroller._cities[ThisCity].population;
        Nourriture.text = "Nourriture : " + Mapcontroller._cities[ThisCity].food;
        Production.text = "Production : " + Mapcontroller._cities[ThisCity].production;
        Or.text = "Or : " + Mapcontroller._cities[ThisCity].gold;
    }
    
    /*
    
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

using UnityEngine;
using UnityEngine.UI;


public enum TurnState { START, ENDTURN } // On verra si on gère certains evenements après que le joueur ait joué

public class GameController : MonoBehaviour
{

    public AnimationCurve foodUpgradeCurve;
    public AnimationCurve foodCostCurve;
    
    public TurnState state;
    [SerializeField] private Text TurnTxt;

    public int NumberOfTurn = 1;

    public GameObject cityMenu;


    public MainMapControllerScript MapControllerScript;
    public ManageCity SelectedCity;
    public AudioSource clickSound;
    // Start is called before the first frame update
    void Start()
    {
        state = TurnState.START;
        MapControllerScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMapControllerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }



    //UI
    public void NextTurn()
    {
        clickSound.PlayOneShot(clickSound.clip);
        state = TurnState.ENDTURN;
        cityMenu.SetActive(false);
        //Les ennemies ? La recolte ?
        foreach (City c in MapControllerScript._cities)
        {
            c.EndOfTurn();
        }
        NumberOfTurn++;
        TurnTxt.text = "Turn : " + NumberOfTurn;
        state = TurnState.START;
    }


    public void FoodBatConstruction()
    {

        SelectedCity.CreateFoodBat();
    }

    public void ProducBatConstruction()
    {
        SelectedCity.CreateProducBat();
    }

    public void GoldBatConstruction()
    {
        SelectedCity.CreateGoldBat();
    }

    public void ExtensionBatConstruction()
    {
        SelectedCity.CreateExtensionBat();
    }

    public void CreateWarrior()
    {
        SelectedCity.CreateWarrior();
    }

    public void CreateArcher()
    {
        SelectedCity.CreateArcher();
    }
    public void CreateRider()
    {
        SelectedCity.CreateRider();
    }
}

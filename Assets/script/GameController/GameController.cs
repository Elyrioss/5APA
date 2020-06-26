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
    public AudioSource buildSound;
    public AudioSource soldierSound;
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
        
        if(SelectedCity.CreateFoodBat()) buildSound.PlayOneShot(buildSound.clip);
    }

    public void ProducBatConstruction()
    {
        if (SelectedCity.CreateProducBat()) buildSound.PlayOneShot(buildSound.clip);
    }

    public void GoldBatConstruction()
    {
        if (SelectedCity.CreateGoldBat()) buildSound.PlayOneShot(buildSound.clip);
    }

    public void ExtensionBatConstruction()
    {
        if (SelectedCity.CreateExtensionBat()) buildSound.PlayOneShot(buildSound.clip);
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

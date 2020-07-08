using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum TurnState { START, ENDTURN } // On verra si on gère certains evenements après que le joueur ait joué

public class GameController : MonoBehaviour
{

    public List<string> Names = new List<string>()
    {
        "RATAGOU",
        "WASTITU",
        "ZEDIBAG",
        "QUEZACO",
        "MASCADU",
        "LABSTIT",
        "WALALOO",
        "RYUJOMA",
        "MASAKA?",
        "NANIKOR",
        "WOUPTID",
        "STALINGRAD",
        "ICICPARI",
        "KRTKIWI",
        "SHOKDART",
        "DAMEDANE"
    };
    public static GameController instance;
    public AnimationCurve foodUpgradeCurve;
    public AnimationCurve foodCostCurve;
    
    public TurnState state;
        
    [SerializeField] private TextMeshProUGUI TurnTxt;
    
    public int NumberOfTurn = 1;
    public CityMenu cityMenu;
    public MainMapControllerScript MapControllerScript;
    
    public City SelectedCity;
    public Unit SelectedUnit;
    public Construction ExtentionTemp=null;
    public AudioSource clickSound;
    
    public Civilisation PlayerCiv;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCiv = new Civilisation(Color.red);
        instance = this;
        state = TurnState.START;
        MapControllerScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMapControllerScript>();
    }


    //UI
    public void NextTurn()
    {
        clickSound.PlayOneShot(clickSound.clip);
        state = TurnState.ENDTURN;
        //Les ennemies ? La recolte ?
        foreach (City c in PlayerCiv.Cities)
        {
            c.EndOfTurn();
        }
        foreach (Unit u in PlayerCiv.Units)
        {
            u.AsPlayed = false;
        }
        NumberOfTurn++;
        TurnTxt.text = "Turn : " + NumberOfTurn;
        state = TurnState.START;
    }

}

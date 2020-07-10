using System.Collections;
using System.Collections.Generic;
using RTS_Cam;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;


public enum TurnState { START, ENDTURN } // On verra si on gère certains evenements après que le joueur ait joué

public class GameController : MonoBehaviour
{

    public List<string> Names = new List<string>()
    {
        "NYONYA",
        "SILENCEKNAVE",
        "SMITEMOMO",
        "SADAME",
        "INABA",
        "METALGEAR",
        "GAREGMACH",
        "WESHALOR",
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

    public Image TurnColor;
    
    public TurnState state;
        
    [SerializeField] private TextMeshProUGUI TurnTxt;
    
    public int NumberOfTurn = 1;
    public CityMenu cityMenu;
    public MainMapControllerScript MapControllerScript;
    public tileMapManager MapManager;    
    public City SelectedCity;
    public Unit SelectedUnit;
    public Construction ExtentionTemp=null;
    public AudioSource clickSound;
    
    public Civilisation PlayerCiv;
    public Civilisation PlayerCiv2;
    public Civilisation CurrentCiv;
    public Material Civ1;
    public Material Civ2;

    public Image Coin;
    public TextMeshProUGUI Money;

    public Image Winner;
    public GameObject WinScreen;

    private RTS_Camera Camera;
    // Start is called before the first frame update
    void Start()
    {
        PlayerCiv = new Civilisation(Color.red,Civ1);
        PlayerCiv2 = new Civilisation(Color.blue,Civ2);
        setStarts();
        
        CurrentCiv = PlayerCiv;
        instance = this;
        state = TurnState.START;
        MapControllerScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainMapControllerScript>();
        TurnColor.color = CurrentCiv.CivilisationColor;
        Camera = UnityEngine.Camera.main.GetComponent<RTS_Camera>();
        Debug.Log((PlayerCiv.Units[0].Position.transform.position.x) +" "+(MapControllerScript.RightLimit-40));
        
    }

    public void StartPos()
    {
        if (PlayerCiv.Units[0].Position.transform.position.x > MapControllerScript.RightLimit-40)
        {
            Camera.targetFollow = PlayerCiv.Units[0].Twin.transform;
        }
        else
        {
            Camera.targetFollow = PlayerCiv.Units[0].prefab.transform;
        }
        StartCoroutine(ResetCam());
    }
    public IEnumerator ResetCam(){
        yield return new WaitForSeconds(4f);
        Camera.targetFollow = null;
    }
    
    public void setStarts()
    {
        Waypoint w = null;
        do
        {
            w = MapManager.LineOrder[Random.Range(0, MapManager.LineOrder.Count)];

        } while (w == null || w.HeightType == HeightType.River || w.HeightType == HeightType.DeepWater || w.HeightType == HeightType.ShallowWater);

        Colon c = new Colon();
        c.ConstructionFinished(w,PlayerCiv);
        PlayerCiv.Units[0].AsPlayed = false;
        
        w = null;
        do
        {
            w = MapManager.LineOrder[Random.Range(0, MapManager.LineOrder.Count)];

        } while (w == null || w.HeightType == HeightType.River || w.HeightType == HeightType.DeepWater || w.HeightType == HeightType.ShallowWater);

        c = new Colon();
        
        c.ConstructionFinished(w,PlayerCiv2);
        PlayerCiv2.Units[0].AsPlayed = false;
    }

    public Civilisation GetOtherCivilisation()
    {
        if (PlayerCiv == CurrentCiv)
        {
            return PlayerCiv2;
        }
        else
        {
            return PlayerCiv;
        }
    }

    //UI
    public void NextTurn()
    {
        clickSound.PlayOneShot(clickSound.clip);
        state = TurnState.ENDTURN;
        //Les ennemies ? La recolte ?
        
        foreach (City c in CurrentCiv.Cities)
        {
            if (c.construction == null)
            {
                SelectedCity = c;
                if (c.position.transform.position.x > MapControllerScript.RightLimit-40)
                {
                    Camera.targetFollow = c.position.Twin.transform;
                }
                else
                {
                    Camera.targetFollow = c.position.transform;
                }
                StartCoroutine(ResetCam());
                cityMenu.ShowCity();
                return;
            }
        }
           
        foreach (Unit u in CurrentCiv.Units)
        {
            if (!u.AsPlayed)
            {
                SelectedUnit = u;
                if (u.prefab.transform.position.x > MapControllerScript.RightLimit-40)
                {
                    Camera.targetFollow = u.Twin.transform;
                }
                else
                {
                    Camera.targetFollow = u.prefab.transform;
                }
                StartCoroutine(ResetCam());
                cityMenu.ShowUnit(u);
                return;
            }
        }
        
        foreach (Unit u in CurrentCiv.Units)
        {
            u.AsPlayed = false;
        }
        foreach (City c in CurrentCiv.Cities)
        {
            c.EndOfTurn();
        }
        
        if (CurrentCiv.Gold > 1500)
        {
            Winner.color = CurrentCiv.CivilisationColor;
            WinScreen.SetActive(true);
            return;
        }
        NumberOfTurn++;
        TurnTxt.text = "Turn : " + NumberOfTurn;
        state = TurnState.START;
        if (CurrentCiv == PlayerCiv)
        {
            CurrentCiv = PlayerCiv2;
        }            
        else
        {
            CurrentCiv = PlayerCiv;
        }

        if (CurrentCiv.Cities.Count > 0)
        {
            if (CurrentCiv.Cities[0].position.transform.position.x > MapControllerScript.RightLimit-40)
            {
                Camera.targetFollow = CurrentCiv.Cities[0].position.Twin.transform;
            }
            else
            {
                Camera.targetFollow = CurrentCiv.Cities[0].position.transform;
            }
            SelectedCity = CurrentCiv.Cities[0];
            StartCoroutine(ResetCam());
        }
        else
        {
            if (CurrentCiv.Units[0].Position.transform.position.x > MapControllerScript.RightLimit-40)
            {
                Camera.targetFollow = CurrentCiv.Units[0].Twin.transform;
            }
            else
            {
                Camera.targetFollow = CurrentCiv.Units[0].prefab.transform;
            }
            SelectedUnit = CurrentCiv.Units[0];
            StartCoroutine(ResetCam());
        }
        
        Coin.color = CurrentCiv.CivilisationColor;
        TurnColor.color = CurrentCiv.CivilisationColor;
        Money.text =""+ CurrentCiv.Gold;
    }

}

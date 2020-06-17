using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        state = TurnState.START;
        // Initialisation de trucs ?
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }



    public void NextTurn()
    {
        state = TurnState.ENDTURN;
        //Les ennemies ? La recolte ?
        //On fait des choooooooses
        NumberOfTurn++;
        TurnTxt.text = "Turn : " + NumberOfTurn;
        state = TurnState.START;
    }
}

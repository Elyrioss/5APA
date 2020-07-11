
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using TMPro;

[RequireComponent(typeof(BoxCollider))]
public class Waypoint : MonoBehaviour
{
    public Animator _animator;
    [HideInInspector]
    public List<Waypoint> Neighbors = new List<Waypoint>(6){null,null,null,null,null,null};    
    [HideInInspector]
    public Waypoint left = null;
    [HideInInspector]
    public Waypoint right = null;
    [HideInInspector]
    public Waypoint leftBot = null;
    [HideInInspector]   
    public Waypoint rightBot = null;  
    [HideInInspector]
    public Waypoint leftTop = null;   
    [HideInInspector]
    public Waypoint rightTop = null;    
        
    public bool visitedDijstra = false;
    [HideInInspector]
    public bool visited = false;   
    
    [Header("Sprites Frontière"), Space(5)]
    public GameObject spriteContainer;
    [SerializeField]
    public SpriteRenderer[] spriteRenderer;// left , leftbot , lefttop, right , rightbot, rightop
   
    [Header("Sprites Frontière"), Space(5)]
    public GameObject RangeContainer;
    [SerializeField]
    public SpriteRenderer[] RangeRenderer;// left , leftbot , lefttop, right , rightbot, rightop
    
    [HideInInspector]
    public int X;
    [HideInInspector]
    public int Y;

    [HideInInspector]
    public bool odd;
 
    [HideInInspector]
    public TileMapPos Chunk;
   
    [HideInInspector]
    public Waypoint Twin;
    [HideInInspector]
    public bool AsTwin=false;
    [HideInInspector]
    public bool IsTwin=false;
    [HideInInspector]
    public float noiseValue;
    
    [HideInInspector]
    public float elevation=0;
    [HideInInspector]
    public BiomeType BiomeType;
    [HideInInspector]
    public HeightType HeightType;
    [HideInInspector]
    public HeatType HeatType;
    [HideInInspector]
    public MoistureType MoistureType;
    
    [HideInInspector]
    public float Food;
    [HideInInspector]
    public float Production;
    [HideInInspector]
    public float Gold;
    public float Science;
    [HideInInspector]
    public GameObject LOD;
    [HideInInspector]
    public GameObject prop;
    [HideInInspector]
    public string Prop;
    
    [HideInInspector]
    public MeshFilter TileFilter;
    [HideInInspector]
    public Material mat;
    
    [Header("Colors"), Space(5)]
    [SerializeField] public Color CivColor = Color.blue;
    [SerializeField] public Color Deactivated = Color.clear;


    public bool UsedTile=false;//For buildings
    public bool Occupied;//For Units
    public bool Controled;//For cities
    [HideInInspector]
    public int mouvCost=1;
    [HideInInspector]
    public int MinCostToStart;
    public Waypoint NearestToStart;
    [HideInInspector]
    public float HeuristicDist;
    
    public int nbTurn = -1;
    public MeshRenderer weaponMesh;

    [Header("Highlight"), Space(5)]
    public SpriteRenderer highlight;

    [Header("Trails"), Space(5)]
    public SpriteRenderer[] trailsList; // left , leftbot , lefttop, right , rightbot, rightop
    public TextMeshProUGUI numberTxt;
    public SpriteRenderer circleNumber;
    
    public void EnableWeapon()
    {
        weaponMesh.gameObject.SetActive(true);
        LOD.SetActive(false);
    }

    public void DisableWeapon()
    {
        weaponMesh.gameObject.SetActive(false);
        LOD.SetActive(true);
    }

    public void ResetNbTurnWeapon()
    {
        nbTurn = -1;
    }

    public void UpdateMatWeapon()
    {
        weaponMesh.material = GameController.instance.weaponMatList[nbTurn];
    }

    public void DisableWaypoint() // make Waypoint unreachable
    {
        foreach (SpriteRenderer sprite in spriteRenderer)
        {
            sprite.color = Deactivated;
        }
        highlight.color = Deactivated;
        _animator.SetBool("shine",false);
        DisableTrail();
    }

    public void EnableWaypoint() // make Waypoint reachable
    {
        foreach (SpriteRenderer sprite in spriteRenderer)
        {
            sprite.color = CivColor;
        }
    }

    public void DisableHighlight() //Disable highlight Waypoint
    {
        
        highlight.color = Deactivated;
        _animator.SetBool("shine",false);
    }

    public void EnableHighlight() //Enable highlight Waypoint
    {
        highlight.color = CivColor;
        _animator.SetBool("shine",true);
    }

    public void EnableTrail(Waypoint endWp)
    {
        //Determinate neighbor
        SetTrail(this,endWp);     
    }

    public void SetIndexTrail(int index)
    {
        circleNumber.color = Color.white;
        numberTxt.text = index.ToString();
    }

    public void DisableTrail()
    {
        for (int i = 0; i < trailsList.Length; i++)
        {
            trailsList[i].color = Deactivated;
        }
        circleNumber.color = Deactivated;
        numberTxt.text = "";
    }

    public void ResetRange()
    {
        RangeContainer.SetActive(false);
        
        RangeRenderer[0].gameObject.SetActive(true);
        RangeRenderer[3].gameObject.SetActive(true);
        RangeRenderer[2].gameObject.SetActive(true);
        
        RangeRenderer[5].gameObject.SetActive(true);
        RangeRenderer[1].gameObject.SetActive(true);
        RangeRenderer[4].gameObject.SetActive(true);
    }
    
    public void SetTrail(Waypoint w,Waypoint W)
    {
        List<Waypoint> list = w.Neighbors;
        if (w.left)
        {
            if (w.left == W)
            {
                w.trailsList[0].color = Color.black;
                W.trailsList[3].color = Color.black;
                if (w.Twin)
                {
                    w.Twin.trailsList[0].color = Color.black;
                    w.Twin.left.trailsList[3].color = Color.black;
                }
                return;               
            }
        }
        if (w.right)
        {
            if (w.right == W)
            {
                w.trailsList[3].color = Color.black;
                W.trailsList[0].color = Color.black;
                if (w.Twin)
                {
                    w.Twin.trailsList[3].color = Color.black;
                    w.Twin.right.trailsList[0].color = Color.black;
                }
                return;  
            }
        }
        if (w.leftTop)
        {
            if (w.leftTop==W)
            {
                w.trailsList[2].color = Color.black;
                W.trailsList[4].color = Color.black;
                if (w.Twin)
                {
                    w.Twin.trailsList[2].color = Color.black;
                    w.Twin.leftTop.trailsList[2].color = Color.black;
                }
                return;  
            }
        }
        if (w.rightTop)
        {
            if (w.rightTop==W)
            {
                w.trailsList[5].color = Color.black;
                W.trailsList[1].color = Color.black;
                if (w.Twin)
                {
                    w.Twin.trailsList[5].color = Color.black;
                    w.Twin.rightTop.trailsList[1].color = Color.black;
                }
                return;  
            }
        }
        if (w.leftBot)
        {
            if (w.leftBot==W)
            {
                w.trailsList[1].color = Color.black;
                W.trailsList[5].color = Color.black;
                
                if (w.Twin)
                {
                    w.Twin.trailsList[1].color = Color.black;
                    w.Twin.leftBot.trailsList[5].color = Color.black;
                }
                return;  
            }
        }
        if (w.rightBot)
        {
            if (w.rightBot==W)
            {
                w.trailsList[4].color = Color.black;
                W.trailsList[2].color = Color.black;
                if (w.Twin)
                {
                    w.Twin.trailsList[4].color = Color.black;
                    w.Twin.rightBot.trailsList[2].color = Color.black;
                }
            }
        }
    }
    
    
}

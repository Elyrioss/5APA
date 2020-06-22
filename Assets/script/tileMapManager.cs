using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using AccidentalNoise;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Quaternion = UnityEngine.Quaternion;
using Random = System.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class tileMapManager : MonoBehaviour
{

    
    //* DISPLAY *//

    public GameObject Prefab;
    public GameObject PrefabOcean;

    public List<Material> Materials;
    public GameObject ice;
    public GameObject border;
    public TileMapPos map;//Reference pour le contenant pour les tuiles (voir le fonctionnement des tilemap unity)
    public List<TileMapPos> Chunks = new List<TileMapPos>(); 
    public List<_3DtileType> _3DTiles;
    public Grid grid; //Contenant pour les contenant des tuile (voir le fonctionnement des tilemap unity)
    //public List<Tilemap> _maps = new List<Tilemap>();//Liste des Chunk instancié 
    public Waypoint waypointPrefab;//Prefab pour les waypoint qui stocke les donnés 
    
    
    //* TAILLE DE LA CARTE *// 
    
    public int chunkX = 3;//Nombres de chunk horizontal
    public int chunkY = 3;//Nombres de chunk vertical
    public int sizeChunkX = 10;//Taile des chunk horizontal
    public int sizeChunkY = 10;//Taile des chunk vertical
    
    //* STORAGE *//
    
    public List<Waypoint> ChunkOrder;//Ordre des tuiles par chunk du bas a gauche ou en bas a droite
    public List<Waypoint> LineOrder = new List<Waypoint>();//Ordre des tuiles sur toutes la carte du bas a gauche ou en bas a droite
    
    //* NOISE MAP *// 
    [Header("Noise generator")]
    
    public float noiseScale;//Facteur qui accentue la variation de la carte
    
    public int octaves;// Nombre de cartes à différente fréquence qui s'additionne pour rendre la carte moins lisse
    public float frequency;
    [Range(0, 1)] public float persistence;// proportion a laquel les octaves successives contribue a la carte
    
    public float lacunarity;//Similaire a la persistance, la lacunarity détermine le niveau de détail des octave successible (=> - de détail, - de variation)
    
    public Vector2 offset;//Permet de parcourir la carte (ex : si on veut décaler les valeur pour scroll sur place
    
    public int seed; //Permet de réobtenir la même noisemap en seedant le random
    public int biomeVariation = 0;//Chance de changer de biome a chaque nouvel tuile
    public int minNumberOfTile = 50;
    public AnimationCurve SmoothCurve; //Permet de rentre les variation plus douce
    
    private bool camOn = false;
    private Waypoint start;
    private bool loaded=false;
    private float[,] noise;
    public WrappingWorldGenerator Generator;
    public Tile[,] Tiles;
    public float ColdestValue, ColderValue, ColdValue;
    
    private void Start()
    {
        Generate();
        camOn = true;
        Generator.Maps((chunkX * sizeChunkX), (chunkY * sizeChunkY), Tiles, ColdestValue, ColderValue, ColdValue);
        MainMapControllerScript MMCS = Camera.main.GetComponent<MainMapControllerScript>();
        MMCS.SetLeft();
        
        TileMapPos Chunk = Chunks[0];
        List<TileMapPos> result = new List<TileMapPos>();
        result = MMCS.HeightCull(Chunk, 3);
        MMCS.currentChunk = Chunk;
        MMCS.CurrentChunks = result;

    }

    //Create the waypoints gameobjects
    public void CreateWaypoint()
    {


        seed = UnityEngine.Random.Range(0, 5000);//Seed
        UnityEngine.Random.InitState(seed);
        ChunkOrder = new List<Waypoint>();
        
        int idtile = 0; //Nom pour les tuiles 
        
        for (int n = 1; n <= chunkX; n++)
        {
            for (int N = 1; N <= chunkY; N++) // chunk en (n;N)
            {
                idtile++;
                GameObject t = Instantiate(map.gameObject, new Vector3(n, N), Quaternion.identity);
                t.GetComponent<TileMapPos>().X = n;
                t.GetComponent<TileMapPos>().Y = N;
                t.transform.SetParent(grid.transform);
                t.name = "" + n + "" + N;
                Chunks.Add(t.GetComponent<TileMapPos>());
                
                for (int i = sizeChunkX * (n - 1); i < sizeChunkX * n; i++) // Create Waypoint in (i;j)
                {
                    for (int j = sizeChunkY * (N - 1); j < sizeChunkY * N; j++)
                    {
                        Waypoint w;
                        if (j % 2 != 0) // Since columns are not aligned we have to differentiate odd columns from the rest 
                        {
                            w = Instantiate(waypointPrefab, new Vector3((i * 1.732f) + 0.866f, 0, j * 1.5f),//Création d'un waypoint qui stock les donnée des tuiles
                                Quaternion.Euler(0, 0, 0));//Create Waypoint for 
                            w.odd = true;                     
                        }
                        else
                        {
                            w = Instantiate(waypointPrefab, new Vector3(i * 1.732f, 0, j * 1.5f),
                                Quaternion.Euler(0, 0, 0));                                                                                
                            w.odd = false;                                                
                        }
                        
                        var position = w.transform.position;
                        w.X = i;
                        w.Y = j;
                        w.transform.SetParent(t.transform);
                        ChunkOrder.Add(w);
                        w.name = "" + (w.X) + (w.Y);   
                        w.i = i;
                        w.j = j;
                        w.Chunk = t.GetComponent<TileMapPos>();
                        

                    }
                }
            }
        }

        foreach (TileMapPos chunk in Chunks)
        {
            foreach (TileMapPos chunk2 in Chunks)
            {
                if (chunk != chunk2)
                {
                    if (chunk2.X == chunk.X + 1 && chunk2.Y == chunk.Y)
                    {
                        chunk.neighBours.Add(chunk2);
                        chunk.Right = chunk2;
                    }
                        

                    if (chunk2.X == chunkX && chunk.X == 1 && chunk2.Y == chunk.Y)
                    {
                        chunk.neighBours.Add(chunk2);
                        chunk.Left = chunk2;
                        
                        chunk2.neighBours.Add(chunk);
                        chunk2.Right = chunk;
                    }


                    if (chunk2.X == chunk.X - 1 && chunk2.Y == chunk.Y)
                    {
                        chunk.neighBours.Add(chunk2);
                        chunk.Left = chunk2;
                    }


                    if (chunk2.Y == chunk.Y + 1 && chunk2.X == chunk.X)
                    {
                        chunk.neighBours.Add(chunk2);
                        chunk.Top = chunk2;
                    }


                    if (chunk2.Y == chunk.Y - 1 && chunk2.X == chunk.X)
                    {
                        chunk.neighBours.Add(chunk2);
                        chunk.Bot = chunk2;
                    }
                        
                }
                
            }
        }
        
        
    }

    
    // Créer les waypoint en utilisant les Waypoint sauvegarder et une seed
    public void CreateWaypoint(List<SavedWaypoint> SW,int _seed)
    {
       
        seed = _seed;
        UnityEngine.Random.InitState(seed);
        ChunkOrder = new List<Waypoint>();
        
        foreach (SavedWaypoint sw in SW) // Comme nous avons les waypoint il suffis de les charger et stocké
        {
            Waypoint w;
            if (sw.j % 2 != 0)
            {
                w = Instantiate(waypointPrefab, new Vector3((sw.i * 1.2f)+0.6f, 0.1f, sw.j*1.045f), Quaternion.identity);
                w.odd = true;
            }
            else
            {
                w = Instantiate(waypointPrefab, new Vector3(sw.i * 1.2f, 0.1f, sw.j*1.045f), Quaternion.identity);
                w.odd = false;
            }
            w.transform.SetParent(grid.transform);
            w.HeightType = sw.HeightType;
            w.BiomeType = sw.BiomeType;
            
            w.HeatType = sw.HeatType;
            w.MoistureType = sw.MoistureType;
            
            w.elevation = sw.elevation;
            w.X = sw.X;
            w.Y = sw.Y;
            w.i = sw.i;
            w.j = sw.j;
            w.name = ""+(w.X) + (w.Y);
            ChunkOrder.Add(w);
        }

    }
    
    //Passez l'ordre des liste de chunk a la liste en ligne 
    public void Order()
    {
        List<Waypoint> temp = ChunkOrder;
        temp = temp.OrderBy(w1 => w1.X).ToList();
        LineOrder = temp;
        
        // En utilisant la liste en Ligne on ajoutes les voisins de chaque tuiles 
        
        
        for (int i = 0; i < LineOrder.Count; i++)
        {
            Waypoint w = LineOrder[i];
            if (i - 1 >= 0 && w.X == LineOrder[i - 1].X) // TUILE D'AVANT VERTICALEMENT => LeftBot pour les odd, RightBot pour les even
            {
                LineOrder[i].Neighbors.Add(LineOrder[i - 1]);
                if (w.odd)
                {
                    w.leftBot = LineOrder[i - 1];
                }
                else
                {
                    w.rightBot = LineOrder[i - 1];
                }
            }

            if (i + 1 < LineOrder.Count && w.X == LineOrder[i + 1].X)// TUILE D'APRES VERTICALEMENT => LeftTop pour les odd, RightTop pour les even
            {
                LineOrder[i].Neighbors.Add(LineOrder[i + 1]);        
                if (w.odd)
                {
                    w.leftTop = LineOrder[i + 1];
                }
                else
                {
                    w.rightTop = LineOrder[i + 1];
                }
            }

            if (w.odd && i + chunkY * sizeChunkX < LineOrder.Count) // Tuiles en diagonal pour les odd
            {
                w.Neighbors.Add(LineOrder[i + chunkY * sizeChunkX]); // RIGHT
                w.right = LineOrder[i + chunkY * sizeChunkX];
                
                LineOrder[i + chunkY * sizeChunkX].Neighbors.Add(w); // LEFT
                LineOrder[i + chunkY * sizeChunkX].left = w;
                
                if (i + (chunkY * sizeChunkX) + 1 < LineOrder.Count)
                {
                    if (LineOrder[i + (chunkY * sizeChunkX) + 1].X == LineOrder[i + chunkY * sizeChunkX].X)
                    {
                        w.Neighbors.Add(LineOrder[i + (chunkY * sizeChunkX) + 1]); //BOTLEFT
                        w.rightTop = LineOrder[i + (chunkY * sizeChunkX) + 1];
                        
                        LineOrder[i + (chunkY * sizeChunkX) + 1].Neighbors.Add(w); //TOPRIGHT
                        LineOrder[i + (chunkY * sizeChunkX) + 1].leftBot = w;

                    }

                }

                if (i + (chunkY * sizeChunkX) - 1 > 0)
                {
                    if (LineOrder[i + (chunkY * sizeChunkX) - 1].X == LineOrder[i +chunkY * sizeChunkX].X)
                    {
                        w.Neighbors.Add(LineOrder[i + (chunkY * sizeChunkX) - 1]); // TOPLEFT
                        w.rightBot = LineOrder[i + (chunkY * sizeChunkX) - 1];
                        
                        
                        LineOrder[i + (chunkY * sizeChunkX) - 1].Neighbors.Add(w); // BOTRIGHT
                        LineOrder[i + (chunkY * sizeChunkX) - 1].leftTop = w;
                        
                    }

                }

            }

            if (!w.odd && i + chunkY * sizeChunkX < LineOrder.Count) // Tuile sur les côté pour les even
            {
                w.Neighbors.Add(LineOrder[i + chunkY * sizeChunkX]); // TUILE D'APRES HORIZONTALEMENT => Right    
                w.right = LineOrder[i + chunkY * sizeChunkX];
                LineOrder[i + chunkY * sizeChunkX].Neighbors.Add(w); // TUILE D'AVANT HORIZONTALEMENT => Left
                LineOrder[i + chunkY * sizeChunkX].left = w;
            }

            if (w.X == 0)
            {
                w.Neighbors.Add(LineOrder[(chunkX * sizeChunkX * sizeChunkY * chunkY)-(sizeChunkY * chunkY)+w.Y]); // TUILE D'APRES HORIZONTALEMENT => Right    
                w.left = LineOrder[(chunkX * sizeChunkX * sizeChunkY * chunkY)-(sizeChunkY * chunkY)+w.Y];
                LineOrder[(chunkX * sizeChunkX * sizeChunkY * chunkY)-(sizeChunkY * chunkY)+w.Y].Neighbors.Add(w); // TUILE D'AVANT HORIZONTALEMENT => Left
                LineOrder[(chunkX * sizeChunkX * sizeChunkY * chunkY)-(sizeChunkY * chunkY)+w.Y].right = w;
              
            }
            

        }

        foreach (Waypoint w in LineOrder)
        {
            if (w.X == 0)
            {
                if (w.left.GetComponent<Waypoint>().rightTop)
                {
                    Waypoint W = w.left.GetComponent<Waypoint>().rightTop.GetComponent<Waypoint>();
                    w.leftTop = W;
                    W.rightBot = w;
                    w.Neighbors.Add(W);
                    W.Neighbors.Add(w);
                }
                if (w.left.GetComponent<Waypoint>().rightBot)
                {
                    Waypoint W = w.left.GetComponent<Waypoint>().rightBot.GetComponent<Waypoint>();
                    w.leftBot = W;
                    W.rightTop = w;
                    w.Neighbors.Add(W);
                    W.Neighbors.Add(w);
                }
            }
            else
            {
                break;
            }
        }
        
        Tiles = Generator.GenerateTileMap(this,(chunkX*sizeChunkX),(chunkY*sizeChunkY),lacunarity,seed);
    }
    
    //Dessiné la carte 
    public Waypoint Draw()
    {
        _3DtileType selection;
        Waypoint result = LineOrder[0];
        foreach (Waypoint w in ChunkOrder)
        {
            Material mat = Materials[0];
            GameObject currentWayPoint;
            
            if (w.HeightType == HeightType.DeepWater || w.HeightType == HeightType.ShallowWater ||w.HeightType == HeightType.River)//Si l'altitude est inférieur a 0, la case est un océan
            {
                if (w.HeightType == HeightType.River)
                {
                    float total = 0;
                    int div = 0;
                    foreach (Waypoint N in w.Neighbors)
                    {                      
                        if (N.elevation != 0f)
                        {
                            total += N.GetComponent<Waypoint>().elevation;
                            div++;
                        }
                    }

                    w.elevation = total / div;
                }
                else
                {
                    if (w.noiseValue < 0.3f)
                    {
                        mat = Materials[1];
                    }
                
                    if (w.noiseValue < 0.2f)
                    {
                        mat = Materials[2];
                    }
                }
                           
                   
                
                selection = ClimateDiagram(BiomeType.Water);  
                currentWayPoint = Instantiate(PrefabOcean,w.transform);
            }
            else
            {
                selection = ClimateDiagram(w.BiomeType);      
                mat = selection.mat;
                currentWayPoint = Instantiate(Prefab,w.transform);
            }
    
            
            currentWayPoint.transform.localPosition = new Vector3(0,w.elevation,0);
            Prop p = selection.getProp(w.elevation);

            // Donner les valeur du type de case a la case
            
            w.Food = selection.Food;
            w.Production = selection.Production;
            w.Gold = selection.Gold; 
            w.mouvCost = selection.MovCost;
            
            currentWayPoint.transform.GetChild(0).GetComponent<MeshRenderer>().material = mat;
            
            if (w.HeightType==HeightType.DeepWater || w.HeightType==HeightType.ShallowWater)
            {
                
                foreach (Waypoint neighbor in w.GetComponent<Waypoint>().Neighbors)
                {
                    if (w.HeightType!=HeightType.DeepWater && w.HeightType!=HeightType.ShallowWater)
                    {
                        if (neighbor.HeatType == HeatType.Cold || neighbor.HeatType == HeatType.Coldest || neighbor.HeatType == HeatType.Colder)
                        {
                            GameObject Ice = Instantiate(ice,w.transform);                
                            Ice.transform.localPosition = new Vector3(0,w.elevation,0);
                            break;
                        }
                    }
                }
            }
            
            if (p != null)
            {
                
                GameObject currentProp = Instantiate(p.prefab,currentWayPoint.transform);
                
                w.Food += p.foodBonus;
                w.Production += p.productionBonus;
                w.Gold += p.goldBonus;
                w.mouvCost += p.MovCost;
                currentProp.transform.localPosition = new Vector3(0,0,0);
            }
            w.spriteContainer.transform.Translate(new Vector3(0,w.elevation+0.05f,0));
            w.DisableWaypoint();
            if (w.Y == 0)
            {
                GameObject Border = Instantiate(border,w.transform);                
                Border.transform.localPosition = new Vector3(0,w.elevation,0);
                Border.transform.localEulerAngles = new Vector3(0,-120,0);
                if (w.HeightType==HeightType.DeepWater || w.HeightType==HeightType.ShallowWater)
                {
                    GameObject Ice = Instantiate(ice,w.transform);                
                    Ice.transform.localPosition = new Vector3(0,w.elevation,0);
                }
            }
            if (w.Y == chunkY*sizeChunkY - 1)
            {
                GameObject Border = Instantiate(border,w.transform);                
                Border.transform.localPosition = new Vector3(0,w.elevation,0);
                Border.transform.localEulerAngles = new Vector3(0,0,0);
                if (w.HeightType==HeightType.DeepWater || w.HeightType==HeightType.ShallowWater)
                {
                    GameObject Ice = Instantiate(ice,w.transform);                
                    Ice.transform.localPosition = new Vector3(0,w.elevation,0);
                }
            }
        }
        
        return result;
    }

    //Principal fonction, génère les type des waypoints et leur élévation
    public Waypoint Generate()
    {
        Clear();
        CreateWaypoint();
        Order();
        
        loaded = false;
                        
        foreach (Waypoint Tile in LineOrder)
        {     
            Tile.elevation = Tiles[Tile.X, Tile.Y].HeightValue*noiseScale;
            Tile.noiseValue = Tiles[Tile.X,Tile.Y].HeightValue;
            Tile.BiomeType = Tiles[Tile.X, Tile.Y].BiomeType;
            Tile.HeightType = Tiles[Tile.X, Tile.Y].HeightType;
            Tile.HeatType = Tiles[Tile.X, Tile.Y].HeatType;
            Tile.MoistureType = Tiles[Tile.X, Tile.Y].MoistureType;
            
        }
             
        Waypoint middle = Draw();

       
        
        return middle;
    }

    public void ShowDijtra()
    {
        foreach (Waypoint w in ChunkOrder)
        {
            w.gameObject.SetActive(false);
        }
        Waypoint Start = ChunkOrder[0];
        Waypoint End = LineOrder[(LineOrder.Count/2)+chunkX*sizeChunkX];        
        ChunkOrder[0].DijkstraSearch(Start,End);
        var shortestPath = new List<Waypoint>();
        shortestPath.Add(End);
        if (End.NearestToStart)
        {
            ChunkOrder[ChunkOrder.Count-1].BuildShortestPath(shortestPath, End);
            shortestPath.Reverse();
            foreach (Waypoint path in shortestPath)
            {
                path.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Waypoint Tile in LineOrder)
            {     
                Tile.gameObject.SetActive(true);
            }
        }
    }
    public _3DtileType ClimateDiagram(BiomeType type)
    {
        foreach (_3DtileType t in _3DTiles)
        {
            if (t.BiomeType == type)
            {
                return t;            
            }
        }
        return _3DTiles[10];
    }
    
    //Clean map
    public void Clear()
    {
        foreach (TileMapPos c in Chunks)
        {
            DestroyImmediate(c.gameObject);
        }
        Chunks.Clear();
        ChunkOrder.Clear();
        LineOrder.Clear();
    }

    #region Helper
    
    
    private static Random rng = new Random();
    public static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    Component CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields(); 
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }

    
    #endregion
    

}



[Serializable]
public class _3DtileType
{
    public string Name;
    public BiomeType BiomeType;
    public Material mat;
    public List<Prop> props;    
    public float Food;
    public float Production;
    public float Gold;
    public int MovCost = 1;
    
    private static Random rng = new Random();
    public static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public Prop getProp(float E)
    {
        Shuffle(props);
        foreach (Prop p in props)
        {
            float f = UnityEngine.Random.Range(0f, 1f);
            if (p.maxElevation > E && p.minElevation < E && f<p.prob)
            {                
                return p;
            }
        }

        return null;
    }
}

[Serializable]
public class Prop
{
    public string name;
    public GameObject prefab;
    public float minElevation;
    public float maxElevation;
    public float prob;
    
    public float foodBonus = 0;
    public float productionBonus = 0;
    public float goldBonus = 0;
    
    public int MovCost = 1;
} 

public class SavedWaypoint
{
    public string WPname;
    public List<String> Names;
    public float elevation=0;
    public HeightType HeightType;
    public BiomeType BiomeType;
    public HeatType HeatType;
    public MoistureType MoistureType;
    public int X;
    public int Y;
    public int i;
    public int j;
    public SavedWaypoint(Waypoint w, int _i)
    {
        WPname = w.name;
        Names=new List<string>();
        foreach (Waypoint n in w.Neighbors)
        {
            Names.Add(n.name);
        }

        BiomeType = w.BiomeType;
        HeightType = w.HeightType;
        HeatType = w.HeatType;
        MoistureType = w.MoistureType;
        elevation = w.elevation;
        X = w.X;
        Y = w.Y;
        i = w.i;
        j = w.j;  
    }
    
    
}

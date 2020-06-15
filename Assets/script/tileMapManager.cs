using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
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

    public List<Material> Materials;
    public GameObject ice;
    public Tilemap map;//Reference pour le contenant pour les tuiles (voir le fonctionnement des tilemap unity)
    public List<_3DtileType> _3DTiles;
    public List<tileType> tiles;//Liste de tout les types de tuiles possibles
    public Grid grid; //Contenant pour les contenant des tuile (voir le fonctionnement des tilemap unity)
    //public List<Tilemap> _maps = new List<Tilemap>();//Liste des Chunk instancié 
    public Waypoint waypointPrefab;//Prefab pour les waypoint qui stocke les donnés 
    public List<type> cold;//Listes des tuiles de type froide
    public List<type> hot;//Listes des tuiles de type chaude
    public List<type> temperate;//Listes des tuiles de type temperé

    private List<Region> Regions=new List<Region>();
    
    
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
    public int RegionToDisplay = 0;
    
    private void Start()
    {        
        camOn = true;
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
                        w.transform.SetParent(grid.transform);
                        ChunkOrder.Add(w);
                        w.name = "" + (w.X) + (w.Y);   
                        w.i = i;
                        w.j = j;
                        
                        
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
            w.names = sw.Names;
            w.type = sw.type;
            w.elevation = sw.elevation;
            w.X = sw.X;
            w.Y = sw.Y;
            w.i = sw.i;
            w.j = sw.j;
            w.tileid = sw.tileid;
            w.name = ""+(w.X) + (w.Y);
            ChunkOrder.Add(w);
        }

    }
    
    //Passez l'ordre des liste de chunk a la liste en ligne 
    public void Order()
    {
        List<Waypoint> temp = new List<Waypoint>();
        foreach (Waypoint w in ChunkOrder)
        {
            temp.Add(w);
        }

        int breaker = 0;
        int limit = ChunkOrder.Count;
        while (LineOrder.Count != limit)
        {
            foreach (Waypoint w in temp)
            {
                if (w.X == breaker)
                {
                    LineOrder.Add(w);
                }
            }

            foreach (Waypoint w in LineOrder)
            {
                temp.Remove(w);
            }

            breaker++;
            if (breaker > 2000)
            {
                break;
            }
        }
        
        // En utilisant la liste en Ligne on ajoutes les voisins de chaque tuiles 
        
        
        for (int i = 0; i < LineOrder.Count; i++)
        {
            Waypoint w = LineOrder[i];
            if (i - 1 >= 0 && w.X == LineOrder[i - 1].X) // TUILE D'AVANT VERTICALEMENT => LeftBot pour les odd, RightBot pour les even
            {
                LineOrder[i].Neighbors.Add(LineOrder[i - 1].gameObject);
                if (w.odd)
                {
                    w.leftBot = LineOrder[i - 1].gameObject;
                }
                else
                {
                    w.rightBot = LineOrder[i - 1].gameObject;
                }
            }

            if (i + 1 < LineOrder.Count && w.X == LineOrder[i + 1].X)// TUILE D'APRES VERTICALEMENT => LeftTop pour les odd, RightTop pour les even
            {
                LineOrder[i].Neighbors.Add(LineOrder[i + 1].gameObject);        
                if (w.odd)
                {
                    w.leftTop = LineOrder[i + 1].gameObject;
                }
                else
                {
                    w.rightTop = LineOrder[i + 1].gameObject;
                }
            }

            if (w.odd && i + chunkY * sizeChunkX < LineOrder.Count) // Tuiles en diagonal pour les odd
            {
                w.Neighbors.Add(LineOrder[i + chunkY * sizeChunkX].gameObject); // RIGHT
                w.right = LineOrder[i + chunkY * sizeChunkX].gameObject;
                
                LineOrder[i + chunkY * sizeChunkX].Neighbors.Add(w.gameObject); // LEFT
                LineOrder[i + chunkY * sizeChunkX].left = w.gameObject;
                
                if (i + (chunkY * sizeChunkX) + 1 < LineOrder.Count)
                {
                    if (LineOrder[i + (chunkY * sizeChunkX) + 1].X == LineOrder[i + chunkY * sizeChunkX].X)
                    {
                        w.Neighbors.Add(LineOrder[i + (chunkY * sizeChunkX) + 1].gameObject); //BOTLEFT
                        w.rightTop = LineOrder[i + (chunkY * sizeChunkX) + 1].gameObject;
                        
                        LineOrder[i + (chunkY * sizeChunkX) + 1].Neighbors.Add(w.gameObject); //TOPRIGHT
                        LineOrder[i + (chunkY * sizeChunkX) + 1].leftBot = w.gameObject;

                    }

                }

                if (i + (chunkY * sizeChunkX) - 1 > 0)
                {
                    if (LineOrder[i + (chunkY * sizeChunkX) - 1].X == LineOrder[i +chunkY * sizeChunkX].X)
                    {
                        w.Neighbors.Add(LineOrder[i + (chunkY * sizeChunkX) - 1].gameObject); // TOPLEFT
                        w.rightBot = LineOrder[i + (chunkY * sizeChunkX) - 1].gameObject;
                        
                        
                        LineOrder[i + (chunkY * sizeChunkX) - 1].Neighbors.Add(w.gameObject); // BOTRIGHT
                        LineOrder[i + (chunkY * sizeChunkX) - 1].leftTop = w.gameObject;
                        
                    }

                }

            }

            if (!w.odd && i + chunkY * sizeChunkX < LineOrder.Count) // Tuile sur les côté pour les even
            {
                w.Neighbors.Add(LineOrder[i + chunkY * sizeChunkX].gameObject); // TUILE D'APRES HORIZONTALEMENT => Right    
                w.right = LineOrder[i + chunkY * sizeChunkX].gameObject;
                LineOrder[i + chunkY * sizeChunkX].Neighbors.Add(w.gameObject); // TUILE D'AVANT HORIZONTALEMENT => Left
                LineOrder[i + chunkY * sizeChunkX].left = w.gameObject;
            }

        }
    }
    
    //Dessiné la carte 
    public Waypoint Draw()
    {
        List<tileType> selection;
        List<_3DtileType> selection3D;
        Waypoint result = LineOrder[0];
        foreach (Waypoint w in ChunkOrder)
        {
            Material mat = Materials[0];    
            if (w.elevation < 0)//Si l'altitude est inférieur a 0, la case est un océan
            {
                selection = ClimateDiagram(type.ocean);
                selection3D = _3DClimateDiagram(type.ocean);// prend une case de type océan
                w.type = type.ocean;
                w.elevation += 0.25f;
                           
                if (w.elevation < -0.25f)
                {
                    mat = Materials[2];
                }
                
                if (w.elevation < -0.35f)
                {
                    mat = Materials[3];
                }                
                
            }
            else
            {
                selection = ClimateDiagram(w.type);//Prend une case du type stocké dans la tuile 
                selection3D = _3DClimateDiagram(w.type);
                
                
            }

            int s;
            int s2;
            
            if (!loaded)
            {
                s = UnityEngine.Random.Range(0, selection.Count);
                s2 = UnityEngine.Random.Range(0, selection3D.Count);
                w.tileid = s;
            }
            else
            {
                s2 = w.tileid;
                s = w.tileid;
                
            }

            if (w.type != type.ocean)
            {
                mat = Materials[selection3D[s2].mat];
            }
            
            
            GameObject currentWayPoint = Instantiate(selection3D[s2].Prefab,w.transform);
            currentWayPoint.transform.localPosition = new Vector3(0,w.elevation,0);
            Prop p = selection3D[s2].getProp(w.elevation);

            // Donner les valeur du type de case a la case
            
            w.Food = selection3D[s2].Food;
            w.Production = selection3D[s2].Production;
            w.Gold = selection3D[s2].Gold;               
            currentWayPoint.transform.GetChild(0).GetComponent<MeshRenderer>().material = mat;
            
            if (w.type == type.ocean)
            {
                
                foreach (GameObject neighbor in w.GetComponent<Waypoint>().Neighbors)
                {
                    if (neighbor.GetComponent<Waypoint>().type != type.ocean)
                    {
                        Waypoint W = neighbor.GetComponent<Waypoint>();
                        if ((W.type == type.artic || W.type == type.tundra)&& W.elevation>0f)
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
                
                currentProp.transform.localPosition = new Vector3(0,0,0);
                if (p.swapMat)
                {
                    currentProp.transform.GetChild(0).GetComponent<MeshRenderer>().material = mat;
                }
            }
            w.spriteContainer.transform.Translate(new Vector3(0,w.elevation+0.05f,0));
            w.DisableWaypoint();
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
        
        
        //prep

        int changeTemp=0, changeHot=0, changeCold=0;
        
        List<Waypoint> temp = new List<Waypoint>();
        foreach (Waypoint w in ChunkOrder)
        {
            temp.Add(w);
        }
        
        int HOT = 0;
        int COLD = 0;
        int TEMP = 0;

        noise = Noise.GenerateNoiseMap(sizeChunkX * chunkX * 2, sizeChunkY * chunkY * 2, this.seed, noiseScale, octaves,
            persistence, lacunarity, offset);

        
        Queue<Waypoint> frontier = new Queue<Waypoint>();
        Waypoint start = ChunkOrder[UnityEngine.Random.Range(0, ChunkOrder.Count)];
        frontier.Enqueue(start);
        start.visited = true;
        start.type = cold[0];
        
        Region currentRegion = new Region();
        Regions.Add(currentRegion);
        
        int index=0;
        
        /*
         *Principe de l'algo :
         *
         * On utilise une Queue contenant une seul tuile prise au hasard. Ensuite on Choisis le méridien ou la tuile se trouve pour choisir le climat.
         * Une fois le climat choisis on vérifis si on change de biome, puis on lui donne un biome. Ensuite on utilise la courbe d'animation pour
         * séléctionné une hauteur adapté.
         *
         * Pour changé de tuile on prend un voisin aux hasard et l'ajoute a la queue.Si les voisins sont visité on prend une tuile aléatoire
         * parmis toutes les toiles.
         * 
        */
        while (frontier.Count > 0)
        {
            if (currentRegion.Count > 200)
            {
                currentRegion = new Region();
                Regions.Add(currentRegion);
            }
            
            type currentType;
            index++;
            Waypoint current = frontier.Dequeue();
            
            currentRegion.Add(current);            
            int c = UnityEngine.Random.Range(0, 1000);
            List<GameObject> tempNeighbors = current.Neighbors;
            Shuffle(tempNeighbors);

            if (current.Y < (sizeChunkY * chunkY) / 7)
            {
                if (c < biomeVariation && changeCold>minNumberOfTile)
                {
                    COLD = UnityEngine.Random.Range(0, cold.Count);
                    changeCold = 0;
                }

                changeCold++;
                currentType = cold[COLD];
            }
            else if (current.Y < (2 * sizeChunkY * chunkY) / 7)
            {
                if (c < biomeVariation && changeTemp>minNumberOfTile)
                {
                    TEMP = UnityEngine.Random.Range(0, temperate.Count);
                    changeTemp = 0;
                }

                changeTemp++;
                currentType = temperate[TEMP];              
            }
            else if (current.Y < (4 * sizeChunkY * chunkY) / 7)
            {
                if (c < biomeVariation && changeHot>minNumberOfTile)
                {
                    HOT = UnityEngine.Random.Range(0, hot.Count);
                    changeHot = 0;
                }

                changeHot++;
                currentType = hot[HOT];
            }
            else if (current.Y < (6 * sizeChunkY * chunkY) / 7)
            {
                if (c < biomeVariation && changeTemp>minNumberOfTile)
                {
                    TEMP = UnityEngine.Random.Range(0, temperate.Count);
                    changeTemp = 0;
                }

                changeTemp++;
                currentType = temperate[TEMP];
            }
            else
            {
                if (c < biomeVariation && changeCold>minNumberOfTile)
                {
                    COLD = UnityEngine.Random.Range(0, cold.Count);
                    changeCold = 0;
                }

                changeCold++;
                
                currentType = cold[COLD];                
            }
                 
            
            current.type = currentType;
            current.elevation = SmoothCurve.Evaluate(noise[(int)current.X, (int)current.Y]);
            current.noiseValue = noise[(int)current.X,(int) current.Y];
                  
            for (int i = 0; i < current.Neighbors.Count; i++)
            {
                GameObject Next = tempNeighbors[i];
                Waypoint next = Next.GetComponent<Waypoint>();
                if (next != null) // Si la case existe
                {
                    if (!next.visited)
                    {
                        temp.Remove(next);
                        next.visited = true;
                        frontier.Enqueue(next);
                        break;
                    }
                }                
            }
            
            
            if(index < ChunkOrder.Count && frontier.Count == 0)
            {
                Waypoint start2 = temp[UnityEngine.Random.Range(0, temp.Count)];
                if (!start2.visited)
                {
                    temp.Remove(start2);
                    start2.visited = true;
                    frontier.Enqueue(start2);
                    currentRegion = new Region();
                    Regions.Add(currentRegion);
                }
                else
                {
                    while (start2.visited)
                    {
                        start2 = temp[UnityEngine.Random.Range(0, temp.Count)];
                        if (!start2.visited)
                        {
                            temp.Remove(start2);   
                            currentRegion = new Region();
                            Regions.Add(currentRegion);
                            frontier.Enqueue(start2);
                        } 
                    }
                    start2.visited = true; 
                }
                           
            }
        }

        
        //Mélange les tuiles en copiant le type d'une tuile voisine pour rendre les limite entre les climat et biome moins homogène
        foreach (Waypoint Tile in ChunkOrder)
        {
            float change = UnityEngine.Random.Range(0, 1);
           
            if (change < 0.2)
            {
                int k = UnityEngine.Random.Range(0, Tile.Neighbors.Count);
                type T = Tile.Neighbors[k].GetComponent<Waypoint>().type;
                if ( Tile.type != type.ocean)
                {
                    int rand = UnityEngine.Random.Range(0, Tile.Neighbors.Count);
                    Tile.type = Tile.Neighbors[rand].GetComponent<Waypoint>().type; 
                    Tile.Neighbors[rand].GetComponent<Waypoint>().region.Swap(Tile);
                    
                }
                
            }
            Waypoint current = Tile.GetComponent<Waypoint>();
            if (current.region.Count < 400)
            {               
                foreach (GameObject g in current.Neighbors)
                {
                    if(current.region.Count > 400)
                        break;
                    if (g.GetComponent<Waypoint>().region != current.region)
                    {
                        current.region.Merge(g.GetComponent<Waypoint>().region);
                        Regions.Remove(g.GetComponent<Waypoint>().region);
                        Regions.TrimExcess();
                    }
                }
            }
            
            if (!Tile.visited)
            {
                Debug.Log("You missed one !");
            }
        }
     
        
        Waypoint middle = Draw();      
        return middle;
    }
    
    //Renvoie la liste des tuiles pour un type précis
    public List<tileType> ClimateDiagram(type type)
    {
        List<tileType> results = new List<tileType>();
        foreach (tileType t in tiles)
        {
            if (t.Type == type)
            {
                results.Add(t);
            }
        }

        return results;
    }
    
    public List<_3DtileType> _3DClimateDiagram(type type)
    {
        List<_3DtileType> results = new List<_3DtileType>();
        foreach (_3DtileType t in _3DTiles)
        {
            if (t.Type == type)
            {
                for (int i = 0; i < t.prob; i++)
                {
                    results.Add(t);
                }              
            }
        }

        return results;
    }
    
    //Clean map
    public void Clear()
    {
        foreach (Waypoint w in ChunkOrder)
        {
            DestroyImmediate(w.gameObject);
        }
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

    public void DisplayRegion(int i)
    {
        Debug.Log(Regions.Count);
        foreach (Waypoint w in ChunkOrder)
        {
            if (!Regions[i].Contains(w))
            {
                w.gameObject.SetActive(false);
            }
            else
            {
                w.gameObject.SetActive(true);
            }
        }
    }
    
    #endregion
    

}

public enum type
{
    ocean,
    island,
    jungle,
    mesa,
    desert,
    swamp,
    forest,
    grass,
    dirt,
    savana,
    tundra,
    artic,
    hellscape
};

[Serializable]
public class tileType
{
    public string name;
    public type Type;
    public int elevation;
    public Tile _tile;
}

[Serializable]
public class _3DtileType
{
    public string Name;
    public type Type;
    public int prob=1;
    public int mat=0;
    public GameObject Prefab;
    public List<Prop> props;
      
    public float Food;
    public float Production;
    public float Gold;
    
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
    public bool swapMat = true;

    public float foodBonus = 0;
    public float productionBonus = 0;
    public float goldBonus = 0;
} 

public class SavedWaypoint
{
    public string WPname;
    public List<String> Names;
    public float elevation=0;
    public type type;
    public int X;
    public int Y;
    public int i;
    public int j;
    public int id;
    public int tileid;
    public SavedWaypoint(Waypoint w, int _i)
    {
        WPname = w.name;
        Names=new List<string>();
        foreach (GameObject n in w.Neighbors)
        {
            Names.Add(n.name);
        }

        type = w.type;
        elevation = w.elevation;
        X = w.X;
        Y = w.Y;
        i = w.i;
        j = w.j;
        id = _i;
        tileid = w.tileid;
    }
    
    
}

public class Region
{
    private List<Waypoint> tiles;
    public int Count;
    
    public Region()
    {
        tiles = new List<Waypoint>();
        Count=0;
    }

    public bool Contains(Waypoint w)
    {
        if (tiles.Contains(w))
        {
            return true;
        }

        return false;
    }
    public void Add(Waypoint w)
    {
        if (!tiles.Contains(w))
        {
            tiles.Add(w);
            w.region = this;
            Count=tiles.Count;
        }     
    }

    public void Remove(Waypoint w)
    {
        tiles.Remove(w);
    }

    public void Swap(Waypoint w)
    {
        w.region.Remove(w);
        Add(w);
    }

    public void Merge(Region r)
    {
        foreach (Waypoint w in r.tiles)
        {
            Add(w);
        }
        
    }
    
    
}
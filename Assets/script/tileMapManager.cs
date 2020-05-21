using System;
using System.Collections;
using System.Collections.Generic;
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
    
    [Header("Map Generation")]
    
    public Tilemap map;
    public List<tileType> Tiles;
    public Grid grid;
    public List<Tilemap> maps = new List<Tilemap>();
    public int X = 3;
    public int Y = 3;
    public int chunkX = 10;
    public int chunkY = 10;
    public Waypoint WaypointPrefab;
    public List<Waypoint> ChunkOrder;
    public List<Waypoint> LineOrder = new List<Waypoint>();
    public List<Waypoint> Continent = new List<Waypoint>();
    public List<tileType.type> cold;
    public List<tileType.type> hot;
    public List<tileType.type> temperate;
    public int meridianlimit;
    
    [Header("Noise generator")]
    
    public float noiseScale;
    public int octaves;
    [Range(0, 1)] public float persistence;
    public float lacunarity;
    public Vector2 offset;
    public AnimationCurve tileHeightCurve;
    public int seed;
    public int biomeVariation = 0;
    public bool camOn = false;
    public Waypoint start;
    private bool loaded=false;
   
    
    private void Start()
    {        
        camOn = true;
    }

    //Create the waypoints gameobjects
    public void CreateWaypoint()
    {
        if (maps.Count > 0)//Clear if chunks are still in the list
        {
            Clear();
        }

        seed = UnityEngine.Random.Range(0, 5000);//Seed to replicate map
        UnityEngine.Random.InitState(seed);
        maps = new List<Tilemap>();
        ChunkOrder = new List<Waypoint>();
        
        int idtile = 0; //Naming 
        
        for (int n = 1; n <= X; n++)
        {
            for (int N = 1; N <= Y; N++) // Create chunk in (n;N)
            {
                Tilemap t = Instantiate(map, grid.transform, true); // Chunk objecy
                t.GetComponent<TileMapPos>().X = n;
                t.GetComponent<TileMapPos>().Y = N;
                maps.Add(t);
                t.name = "" + idtile;
                idtile++;
                for (int i = chunkX * (n - 1); i < chunkX * n; i++) // Create Waypoint in (i;j)
                {
                    for (int j = chunkY * (N - 1); j < chunkY * N; j++)
                    {
                        Waypoint w;
                        if (j % 2 != 0) // Since columns are not aligned we have to differentiate odd columns from the rest 
                        {
                            w = Instantiate(WaypointPrefab, new Vector3((i * 1.2f) + 0.6f, 0.1f, j * 1.045f),
                                Quaternion.identity);//Create Waypoint for 
                            w.transform.SetParent(grid.transform);
                            w.X = Mathf.RoundToInt(i * 1.2f);
                            w.Y = Mathf.RoundToInt(w.transform.position.z);
                            w.odd = true;                     
                        }
                        else
                        {
                            w = Instantiate(WaypointPrefab, new Vector3(i * 1.2f, 0.1f, j * 1.045f),
                                Quaternion.identity);                          
                            var position = w.transform.position;
                            w.X = Mathf.RoundToInt(position.x);
                            w.Y = Mathf.RoundToInt(position.z);                           
                            w.odd = false;                                                
                        }
                        
                        w.transform.SetParent(grid.transform);
                        ChunkOrder.Add(w);
                        w.name = "" + (w.X) + (w.Y);   
                        w.i = i;
                        w.j = j;
                        w.tilemap = t;
                        w.DisableWaypoint();
                    }
                }
            }
        }
    }

    
    // Creates waypoints with saved way points
    public void CreateWaypoint(List<SavedWaypoint> SW,int _seed)
    {
        if (maps.Count > 0)
        {
            Clear();
        }
       
        seed = _seed;
        UnityEngine.Random.InitState(seed);
        maps = new List<Tilemap>();
        ChunkOrder = new List<Waypoint>();
        
        int idtile = 0;
        for (int n = 1; n <= X; n++) // Setting up tilemaps
        {
            for (int N = 1; N <= Y; N++)
            {
                Tilemap t = Instantiate(map, grid.transform, true);
                t.GetComponent<TileMapPos>().X = n;
                t.GetComponent<TileMapPos>().Y = N;
                maps.Add(t);
                t.name = "" + idtile;
                idtile++;
                if (camOn)
                {
                    t.gameObject.SetActive(false);
                }
            }
        }
        
        foreach (SavedWaypoint sw in SW) // Since we have the waypoints saved we just load them and instantiate
        {
            Waypoint w;
            if (sw.j % 2 != 0)
            {
                w = Instantiate(WaypointPrefab, new Vector3((sw.i * 1.2f)+0.6f, 0.1f, sw.j*1.045f), Quaternion.identity);
                w.odd = true;
            }
            else
            {
                w = Instantiate(WaypointPrefab, new Vector3(sw.i * 1.2f, 0.1f, sw.j*1.045f), Quaternion.identity);
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
            w.tilemapname = sw.tileMap;
            w.name = ""+(w.X) + (w.Y);
            ChunkOrder.Add(w);
            foreach (Tilemap t in maps)
            {
                if (t.name == w.tilemapname)
                {
                    w.tilemap = t;
                }
            }
        }

    }
    
    //Order Waypoints in lines
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

        for (int i = 0; i < LineOrder.Count; i++)
        {
            Waypoint w = LineOrder[i];
            if (i - 1 >= 0 && w.X == LineOrder[i - 1].X)
            {
                LineOrder[i].Neighbors.Add(LineOrder[i - 1].gameObject);
            }

            if (i + 1 < LineOrder.Count && w.X == LineOrder[i + 1].X)
            {
                LineOrder[i].Neighbors.Add(LineOrder[i + 1].gameObject);
            }

            if (w.odd && i + Y * chunkX < LineOrder.Count)
            {
                w.Neighbors.Add(LineOrder[i + Y * chunkX].gameObject);
                LineOrder[i + Y * chunkX].Neighbors.Add(w.gameObject);

                if (i + (Y * chunkX) + 1 < LineOrder.Count)
                {
                    if (LineOrder[i + (Y * chunkX) + 1].X == LineOrder[i + Y * chunkX].X)
                    {
                        w.Neighbors.Add(LineOrder[i + (Y * chunkX) + 1].gameObject);
                        LineOrder[i + (Y * chunkX) + 1].Neighbors.Add(w.gameObject);
                    }

                }

                if (i + (Y * chunkX) - 1 > 0)
                {
                    if (LineOrder[i + (Y * chunkX) - 1].X == LineOrder[i +Y * chunkX].X)
                    {
                        w.Neighbors.Add(LineOrder[i + (Y * chunkX) - 1].gameObject);
                        LineOrder[i + (Y * chunkX) - 1].Neighbors.Add(w.gameObject);
                    }

                }

            }

            if (!w.odd && i + Y * chunkX < LineOrder.Count)
            {
                w.Neighbors.Add(LineOrder[i + Y * chunkX].gameObject);
                LineOrder[i + Y * chunkX].Neighbors.Add(w.gameObject);
            }

        }
    }
    
    //Draw the actual map
    public Waypoint Draw()
    {
        List<tileType> selection;
        Waypoint result = LineOrder[0];
        foreach (Waypoint w in ChunkOrder)
        {
            if (w.elevation < 0)
            {
                selection = ClimateDiagram(tileType.type.ocean);
                w.type = tileType.type.ocean;
            }
            else
            {
                selection = ClimateDiagram(w.type);
            }

            int s;
            
            if (!loaded)
            {
                s = UnityEngine.Random.Range(0, selection.Count - 1);
                w.tileid = s;
            }
            else
            {
                s = w.tileid;
            }

            w.tilemap.BoxFill(new Vector3Int(w.i, w.j, 0), selection[s]._tile, w.i, w.j, w.i, w.j);
        }
        return result;
    }
    
    //Generate the waypoints with random factors
    public Waypoint Generate()
    {
        CreateWaypoint();
        Order();
        loaded = false;
        int HOT = 0;
        int COLD = 0;
        int TEMP = 0;

        int top = 0;
        int temperate2 = 0;
        int mid = 0;
        int temperate1 = 0;
        int bot = 0;

        float[,] noise = Noise.GenerateNoiseMap(chunkX * X * 2, chunkY * Y * 2, this.seed, noiseScale, octaves,
            persistence, lacunarity, offset);
        float[,] falloffMap = FallOffGenerator.GenerateFalloffMap(chunkX * X * 2);
        List<tileType.type> currentType = cold;
        tileType.type type = cold[0];
        float currentTemp = 0;
        Queue<Waypoint> frontier = new Queue<Waypoint>();
        Waypoint start = ChunkOrder[UnityEngine.Random.Range(0, ChunkOrder.Count)];
        frontier.Enqueue(start);
        start.visited = true;
        start.type = type;
        while (frontier.Count > 0)
        {

            Waypoint current = frontier.Dequeue();
            int c = UnityEngine.Random.Range(0, 1000);
            Shuffle(current.Neighbors);
            if (current.Y < (chunkY * Y) / 7)
            {
                if (c < biomeVariation && bot > 50)
                {
                    COLD = UnityEngine.Random.Range(0, cold.Count);
                    bot = 0;
                }

                type = cold[COLD];
                bot++;
            }
            else if (current.Y < (2 * chunkY * Y) / 7)
            {
                if (c < biomeVariation && temperate1 > 50)
                {
                    TEMP = UnityEngine.Random.Range(0, temperate.Count);
                    temperate1 = 0;
                }

                type = temperate[TEMP];
                temperate1++;
            }
            else if (current.Y < (4 * chunkY * Y) / 7)
            {
                if (c < biomeVariation && mid > 50)
                {
                    HOT = UnityEngine.Random.Range(0, hot.Count);
                    mid = 0;
                }

                type = hot[HOT];
                mid++;
            }
            else if (current.Y < (6 * chunkY * Y) / 7)
            {
                if (c < biomeVariation && temperate2 > 50)
                {
                    TEMP = UnityEngine.Random.Range(0, temperate.Count);
                    temperate2 = 0;
                }

                type = temperate[TEMP];
                temperate2++;
            }
            else
            {
                if (c < biomeVariation && top > 50)
                {
                    COLD = UnityEngine.Random.Range(0, cold.Count);
                    top = 0;
                }

                type = cold[COLD];
                top++;
            }
            float change = noise[current.X, current.Y];
            if (current.Y == Mathf.RoundToInt((6 * chunkY * Y) / 7) + meridianlimit ||
                current.Y == Mathf.RoundToInt((chunkY * Y) / 7) + meridianlimit)
            {
                
                if (change > 0.6f)
                {
                    type = cold[COLD];
                }
                else
                {
                    type = temperate[TEMP];
                }
            }

            if (current.Y == Mathf.RoundToInt((4 * chunkY * Y) / 7) + meridianlimit ||
                current.Y == Mathf.RoundToInt((2 * chunkY * Y) / 7) + meridianlimit)
            {
                
                if (change > 0.6f)
                {
                    type = hot[HOT];
                }
                else
                {
                    type = temperate[TEMP];
                }
            }

            current.type = type;
            current.elevation = tileHeightCurve.Evaluate(noise[current.X, current.Y]) - 0.5f;
            foreach (GameObject Next in current.Neighbors)
            {
                Waypoint next = Next.GetComponent<Waypoint>();
                if (next != null) // Si la case existe
                {

                    if (!next.visited)
                    {
                        next.visited = true;

                        frontier.Enqueue(next);
                    }
                }
            }
        }

        Waypoint middle = Draw();      
        return middle;
    }
    
    //Select a tile in a climate
    public List<tileType> ClimateDiagram(tileType.type type)
    {
        List<tileType> results = new List<tileType>();
        foreach (tileType t in Tiles)
        {
            if (t.Type == type)
            {
                results.Add(t);
            }
        }

        return results;
    }

    //Clean map
    public void Clear()
    {
        foreach (Tilemap m in maps)
        {
            m.ClearAllTiles();
            DestroyImmediate(m.gameObject);
        }

        maps.Clear();
        foreach (Waypoint w in LineOrder)
        {
            DestroyImmediate(w.gameObject);
        }

        ChunkOrder.Clear();
        LineOrder.Clear();
        Continent.Clear();
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
    
    #endregion
    

}

[Serializable]
public class tileType
{
    public string name;

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

    public type Type;
    public int elevation;
    public Tile _tile;
}

public class SavedWaypoint
{
    public string WPname;
    public List<String> Names;
    public float elevation=0;
    public tileType.type type;
    public int X;
    public int Y;
    public int i;
    public int j;
    public int id;
    public string tileMap;
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
        tileMap = w.tilemap.name;
        tileid = w.tileid;
    }
}
using System.Collections.Generic;
using UnityEngine;
using AccidentalNoise;
 
public class Generator : MonoBehaviour {
 
    // Adjustable variables for Unity Inspector
    [SerializeField]
    int Width = 256;
    [SerializeField]
    int Height = 256;
    [SerializeField]
    int TerrainOctaves = 6;
    [SerializeField]
    double TerrainFrequency = 1.25;
 
    // Noise generator module
    ImplicitFractal HeightMap;
     
    // Height map data
    MapData HeightData;
 
    // Final Objects
    Tile[,] Tiles;
     
    // Our texture output (unity component)
    MeshRenderer HeightMapRenderer;

    
    private void UpdateNeighbors(List<Waypoint> waypoints,int width)
    {
        int i = 0;
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                Tile t = Tiles[x,y];
                              
                Waypoint W = waypoints[i];
                
                if(W.leftTop)
                    t.LeftTop = Tiles[W.leftTop.GetComponent<Waypoint>().X,W.leftTop.GetComponent<Waypoint>().Y];
                
                if(W.rightTop)
                t.RightTop = Tiles[W.rightTop.GetComponent<Waypoint>().X,W.rightTop.GetComponent<Waypoint>().Y];
                
                if(W.leftBot)
                    t.LeftBottom = Tiles[W.leftBot.GetComponent<Waypoint>().X,W.leftBot.GetComponent<Waypoint>().Y];
                
                if(W.rightBot)
                    t.RightBottom = Tiles[W.rightBot.GetComponent<Waypoint>().X,W.rightBot.GetComponent<Waypoint>().Y];
                
                if(W.left)
                    t.Left = Tiles[W.left.GetComponent<Waypoint>().X,W.left.GetComponent<Waypoint>().Y];
                
                if(W.right)
                    t.Right = Tiles[W.right.GetComponent<Waypoint>().X,W.right.GetComponent<Waypoint>().Y];

                i++;
            }
        }
    }
    public Tile[,] Generate(int width,int height,List<Waypoint> waypoints,float lacunarity)
    {
        Width = width;
        Height = height;
        // Get the mesh we are rendering our output to
        HeightMapRenderer = transform.Find("HeightTexture").GetComponent<MeshRenderer>();
 
        // Initialize the generator
        Initialize (lacunarity);
         
        // Build the height map
        GetData (HeightMap, ref HeightData);
         
        // Build our final objects based on our data
        
        LoadTiles();
        UpdateNeighbors(waypoints, width);
        // Render a texture representation of our map
        //HeightMapRenderer.materials[0].mainTexture = TextureGenerator.GetTexture (Width, Height, Tiles);
        return Tiles;
    }
 
    private void Initialize(float lacunarity)
    {
        // Initialize the HeightMap Generator
        HeightMap = new ImplicitFractal (FractalType.MULTI, 
                                       BasisType.SIMPLEX, 
                                       InterpolationType.QUINTIC, 
                                       TerrainOctaves, 
                                       TerrainFrequency, 
                                       UnityEngine.Random.Range (0, int.MaxValue));
        HeightMap.Lacunarity = lacunarity;
    }
     
    // Extract data from a noise module
    public void GetData(ImplicitModuleBase module, ref MapData mapData)
    {
        mapData = new MapData (Width, Height);
 
        // loop through each x,y point - get height value
        for (var x = 0; x < Width; x++) {
            for (var y = 0; y < Height; y++) {
 
                //Noise range
                float x1 = 0, x2 = 1;
                float y1 = 0, y2 = 1;               
                float dx = x2 - x1;
                float dy = y2 - y1;
 
                //Sample noise at smaller intervals
                float s = x / (float)Width;
                float t = y / (float)Height;
 
                // Calculate our 3D coordinates
                float nx = x1 + Mathf.Cos (s * 2 * Mathf.PI) * dx / (2 * Mathf.PI);
                float ny = x1 + Mathf.Sin (s * 2 * Mathf.PI) * dx / (2 * Mathf.PI);
                float nz = t;
 
                float heightValue = (float)HeightMap.Get (nx, ny, nz);
 
                // keep track of the max and min values found
                if (heightValue > mapData.Max)
                    mapData.Max = heightValue;
                if (heightValue < mapData.Min)
                    mapData.Min = heightValue;
 
                mapData.Data [x, y] = heightValue;
            }
        }  
    }
     
    // Build a Tile array from our data
    private void LoadTiles()
    {
        Tiles = new Tile[Width, Height];
         
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                Tile t = new Tile();
                t.X = x;
                t.Y = y;
                 
                float value = HeightData.Data[x, y];
                 
                //normalize our value between 0 and 1
                value = (value - HeightData.Min) / (HeightData.Max - HeightData.Min);
                 
                t.HeightValue = value;
 
                Tiles[x,y] = t;
            }
        }
    }
 
}
 
public static class TextureGenerator {
         
    public static Texture2D GetTexture(int width, int height, Tile[,] tiles)
    {
        var texture = new Texture2D(width, height);
        var pixels = new Color[width * height];
 
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                float value = tiles[x, y].HeightValue;
 
                //Set color range, 0 = black, 1 = white
                pixels[x + y * width] = Color.Lerp (Color.black, Color.white, value);
            }
        }
         
        texture.SetPixels(pixels);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        return texture;
    }
     
}

public class Tile
{
    public float HeightValue { get; set; }
    public int X, Y;
    
    public Tile Left;
    public Tile Right;
    public Tile LeftTop;
    public Tile RightTop;
    public Tile LeftBottom;
    public Tile RightBottom;     
    
    public Tile()
    {
    }
}

public class MapData {
 
    public float[,] Data;
    public float Min { get; set; }
    public float Max { get; set; }
 
    public MapData(int width, int height)
    {
        Data = new float[width, height];
        Min = float.MaxValue;
        Max = float.MinValue;
    }
}

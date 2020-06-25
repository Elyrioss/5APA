using System.Collections.Generic;
using UnityEngine;

public class TileMapPos : MonoBehaviour
{
    public int X;
    public int Y;

    public TileMapPos Top;
    public TileMapPos Bot;
    public TileMapPos Right;
    public TileMapPos Left;

    public int index;
    
    public List<TileMapPos> neighBours = new List<TileMapPos>();
    
}

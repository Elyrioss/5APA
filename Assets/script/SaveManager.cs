using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;

    public List<SavedWaypoint> Waypoints;
    public SavedWaypoint lastWaypoint;
    public static int seed;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        
    }

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    public void SaveGame(tileMapManager mapManager)
    {
        SaveGame saveGame = new SaveGame(mapManager);
        string dataAsJson = JsonUtility.ToJson(saveGame);
        string filePath = Path.Combine(Application.dataPath, "..", "SaveGame.json");
        File.WriteAllText(filePath, dataAsJson);
    }

    public void LoadGameForUIOnly()
    {
        LoadGame();
    }

    public void DeleteSave()
    {
        string filePath = Path.Combine(Application.dataPath, "..", "SaveGame.json");
        bool loaded = File.Exists(filePath);

        if (loaded)
        {
            File.Delete(filePath);
        }
    }

    public bool LoadGame()
    {
        string filePath = Path.Combine(Application.dataPath, "..", "SaveGame.json");
        bool loaded = File.Exists(filePath);
       
        if(loaded)
        {   
            string dataAsJson = File.ReadAllText(filePath);
            SaveGame loadedData = JsonUtility.FromJson<SaveGame>(dataAsJson);
            List<SavedWaypoint> sw = new List<SavedWaypoint>();
            foreach (string savedWaypointJson in loadedData.Waypoints)
            {
                SavedWaypoint savedCharacter = JsonUtility.FromJson<SavedWaypoint>(savedWaypointJson);
                sw.Add(savedCharacter);
            }       
            //lastWaypoint = JsonUtility.FromJson<SavedWaypoint>(loadedData.LastWaypoint);
            seed = loadedData.seed;
            Waypoints = sw;
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool StaticLoadGame()
    {
        return instance.LoadGame();
    }

    public static bool IsSaveExist()
    {
        string filePath = Path.Combine(Application.dataPath, "..", "SaveGame.json");
        return File.Exists(filePath);
    }
}

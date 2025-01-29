using System.IO;
using UnityEngine;

public class JsonLoader : MonoBehaviour
{
    public static BuildingConfig[] LoadBuildingConfig(string path)
    {
        string json = File.ReadAllText(path);
        BuildingConfig[] buildings = JsonUtility.FromJson<BuildingConfigArray>(json).buildings;
        return buildings;
    }

    [System.Serializable]
    private class BuildingConfigArray
    {
        public BuildingConfig[] buildings;
    }
}
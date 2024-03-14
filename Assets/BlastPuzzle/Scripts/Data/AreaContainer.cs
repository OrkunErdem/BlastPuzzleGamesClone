using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlastPuzzle.Scripts.Data
{
    [CreateAssetMenu(fileName = "AreaContainer", menuName = "ScriptableObjects/AreaContainer", order = 1)]
    public class AreaContainer : ScriptableObject
    {
        public List<AreaData> areaDataList = new List<AreaData>();
    }

    [System.Serializable]
    public class AreaData
    {
        public bool isAvailable;
        public int areaID;
        public string areaPoolId;
        public List<int> availableBuildings = new List<int>();
        public List<int> onBuildings = new List<int>();
        public List<BuildingData> existingBuilding = new List<BuildingData>();
    }


    [System.Serializable]
    public class AreaMetaData
    {
        public Image cityImage;
        public string cityDescription;
    }
}
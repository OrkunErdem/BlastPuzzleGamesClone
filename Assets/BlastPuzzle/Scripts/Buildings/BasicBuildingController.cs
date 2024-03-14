using BlastPuzzle.Scripts.Interfaces;
using MergeIncome.Scripts.Building;
using UnityEngine;

namespace BlastPuzzle.Scripts.Buildings
{
    public class BasicBuildingController : MonoBehaviour, IBuildingControllers
    {
        private BuildingData _buildingData;
        private BuildingGraphicsController _buildingGraphicsController;

        public void SetBuilding(BuildingData buildingData, BuildingGraphicsController buildingGraphicsController)
        {
            _buildingData = buildingData;
            _buildingGraphicsController = buildingGraphicsController;
            var x = _buildingData.buildingID;
            var sprite = Resources.Load<Sprite>("TaskGraphs/task_"+x);
            _buildingGraphicsController.SetImageSprite(sprite);
        }
    }
    
}
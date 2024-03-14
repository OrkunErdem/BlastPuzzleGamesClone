using BlastPuzzle.Scripts.Interfaces;
using UnityEngine;

namespace BlastPuzzle.Scripts.Buildings
{
    public class LockedBuildingController : MonoBehaviour, IBuildingControllers
    {
        private BuildingData _buildingData;
        private BuildingGraphicsController _buildingGraphicsController;

        public void SetBuilding(BuildingData buildingData, BuildingGraphicsController buildingGraphicsController)
        {
            _buildingData = buildingData;
            _buildingGraphicsController = buildingGraphicsController;
            var sprite = Resources.Load<Sprite>("TaskGraphs/lock");
            buildingGraphicsController.SetImageSprite(sprite);
        }
    }
}
using BlastPuzzle.Scripts.Interfaces;
using BlastPuzzle.Scripts.Managers;
using MergeIncome.Scripts.Building;
using UnityEngine;
using UnityEngine.UI;

namespace BlastPuzzle.Scripts.Buildings
{
    public class OnBuildingController : MonoBehaviour, IBuildingControllers
    {
        [SerializeField] private Button button;
        private BuildingController _buildingController;

        private void OnEnable()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnClick);
        }

        public void SetBuilding(BuildingData buildingData, BuildingGraphicsController buildingGraphicsController)
        {
            _buildingController = GetComponentInParent<BuildingController>();
            buildingGraphicsController.OpenBuildingUI(buildingData.buildingID + 1);
        }

        private void OnClick()
        {
            button.onClick.RemoveListener(OnClick);
            button.gameObject.SetActive(false);
            AreaManager.Instance.AddAvailableBuilding(_buildingController);
            _buildingController.SetBuildingState(BuildState.Unlocked);
        }
    }
}
using System.Collections.Generic;
using MergeIncome.Scripts.Building;
using UnityEngine;

namespace BlastPuzzle.Scripts.Buildings
{
    public class BuildingStateController : MonoBehaviour
    {
        private BuildingController _buildingController;
        private BasicBuildingController _basicBuildingController;
        private LockedBuildingController _lockedBuildingController;
        private OnBuildingController _onBuildingController;
        private BuildingGraphicsController _buildingGraphicsController;
        private readonly List<GameObject> _controllers=new List<GameObject>();

        private BasicBuildingController BasicBuildingController
        {
            get
            {
                if (!_basicBuildingController)

                    _basicBuildingController = GetComponentInChildren<BasicBuildingController>();
                _controllers.Add(_basicBuildingController.gameObject);
                return _basicBuildingController;
            }
        }

        private LockedBuildingController LockedBuildingController
        {
            get
            {
                if (!_lockedBuildingController)
                    _lockedBuildingController = GetComponentInChildren<LockedBuildingController>();
                _controllers.Add(_lockedBuildingController.gameObject);
                return _lockedBuildingController;
            }
        }

        private OnBuildingController OnBuildingController
        {
            get
            {
                if (!_onBuildingController)
                    _onBuildingController = GetComponentInChildren<OnBuildingController>();
                _controllers.Add(_onBuildingController.gameObject);
                return _onBuildingController;
            }
        }

        private BuildingGraphicsController BuildingGraphicsController
        {
            get
            {
                if (!_buildingGraphicsController)
                    _buildingGraphicsController = GetComponentInChildren<BuildingGraphicsController>();
                return _buildingGraphicsController;
            }
        }

        private void Awake()
        {
            _buildingController = GetComponent<BuildingController>();
        }

        private void OnEnable()
        {
            _buildingController.OnBuildStateChanged += OnBuildStateChanged;
            _buildingController.Initialize();
        }

        private void OnDisable()
        {
            _buildingController.OnBuildStateChanged -= OnBuildStateChanged;
        }

        private void OnBuildStateChanged(BuildState state)
        {
            SetState(state);
        }

        private void SetState(BuildState buildState)
        {
            switch (buildState)
            {
                case BuildState.Locked:
                    LockedBuildingController.SetBuilding(_buildingController.BuildingData, BuildingGraphicsController);
                    SetActiveGameObject(_lockedBuildingController.gameObject);
                    break;

                case BuildState.Unlocked:
                    BasicBuildingController.SetBuilding(_buildingController.BuildingData, BuildingGraphicsController);
                    SetActiveGameObject(_basicBuildingController.gameObject);
                    break;

                case BuildState.Built:
                    OnBuildingController.SetBuilding(_buildingController.BuildingData, BuildingGraphicsController);
                    SetActiveGameObject(_onBuildingController.gameObject);
                    break;

                default:
                    Debug.LogError("Unhandled build state: " + buildState);
                    break;
            }
        }

        private void SetActiveGameObject(GameObject controllerObject)
        {
            foreach (var controller in _controllers)
            {
                controller.SetActive(controller == controllerObject);
            }
        }
    }

    public enum BuildState
    {
        Locked,
        Unlocked,
        Built
    }
}
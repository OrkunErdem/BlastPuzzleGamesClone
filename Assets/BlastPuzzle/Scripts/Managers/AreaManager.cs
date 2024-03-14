using System.Collections.Generic;
using BlastPuzzle.Scripts.Buildings;
using BlastPuzzle.Scripts.Data;
using BlastPuzzle.Scripts.Models;
using MergeIncome.Scripts.Building;
using UnityEngine;

namespace BlastPuzzle.Scripts.Managers
{
    public class AreaManager : Singleton<AreaManager>
    {
        private bool _isLoaded;
        public AreaContainer areaContainer;
        private int _currentAreaId;
        private AreaData _area;
        private List<BuildingData> _allBuildings;
        private List<int> _availableBuildings;
        private List<int> _onBuildings;

        private static int _activeLevel;

        private static int ActiveLevel
        {
            get
            {
                _activeLevel = PlayerPrefs.GetInt("ActiveLevel", 0);
                return _activeLevel;
            }
            set
            {
                _activeLevel = value;
                PlayerPrefs.SetInt("ActiveLevel", value);
            }
        }

        [SerializeField] private Transform canvas;

        public int ActiveBuilding => _onBuildings.Count;

        private void Load()
        {
            _isLoaded = true;
            areaContainer = Resources.Load<AreaContainer>("AreaContainer");
            LoadArea();
        }

        private void LoadArea()
        {
            _currentAreaId = PlayerPrefs.GetInt("ActiveArea");
            _area = areaContainer.areaDataList.Find(x => x.areaID == _currentAreaId);
            _allBuildings = _area.existingBuilding;
            var availableBuildings = PlayerDataManager.Instance.GetPlayerData().AvailableBuildings;
            var onBuildings = PlayerDataManager.Instance.GetPlayerData().OnBuildings;

            if (availableBuildings.FindIndex(x => x.AreaId == _currentAreaId) == -1)
            {
                var availableBuildingData = new AvailableBuildingData
                {
                    AreaId = _currentAreaId,
                    AvailableBuildingIds = _area.availableBuildings
                };
                availableBuildings.Add(availableBuildingData);
                PlayerDataManager.Instance.SetAvailableBuildings(availableBuildings);
                Debug.Log("Available Buildings Added");
            }

            if (onBuildings.FindIndex(x => x.AreaId == _currentAreaId) == -1)
            {
                var onBuildingData = new OnBuildingsData
                {
                    AreaId = _currentAreaId,
                    OnBuildingIds = _area.onBuildings
                };
                onBuildings.Add(onBuildingData);
                PlayerDataManager.Instance.SetOnBuildings(onBuildings);
                Debug.Log("On Buildings Added");
            }

            _onBuildings = PlayerDataManager.Instance.GetPlayerData().OnBuildings
                .Find(x => x.AreaId == _currentAreaId).OnBuildingIds;
            _availableBuildings = PlayerDataManager.Instance.GetPlayerData().AvailableBuildings
                .Find(x => x.AreaId == _currentAreaId).AvailableBuildingIds;
        }

        public void GoNext()
        {
            _onBuildings ??= new List<int>();

            _onBuildings.Add(ActiveLevel);

            ActiveLevel++;
            _ = PlayerDataManager.Instance.GetPlayerData().OnBuildings;
            var onBuildingData = new OnBuildingsData
            {
                AreaId = _currentAreaId,
                OnBuildingIds = _onBuildings
            };
            var onBuildings = PlayerDataManager.Instance.GetPlayerData().OnBuildings;
            onBuildings[onBuildings.FindIndex(x => x.AreaId == _currentAreaId)] = onBuildingData;
            PlayerDataManager.Instance.SetOnBuildings(onBuildings);
        }

        public void AddAvailableBuilding(BuildingController buildingController)
        {
            _onBuildings.Remove(buildingController.buildingId);
            _availableBuildings.Add(buildingController.buildingId);
            var availableBuildingData = new AvailableBuildingData
            {
                AreaId = _currentAreaId,
                AvailableBuildingIds = _availableBuildings
            };
            var availableBuildings = PlayerDataManager.Instance.GetPlayerData().AvailableBuildings;
            availableBuildings[availableBuildings.FindIndex(x => x.AreaId == _currentAreaId)] = availableBuildingData;
            PlayerDataManager.Instance.SetAvailableBuildings(availableBuildings);
            var onBuildingData = new OnBuildingsData
            {
                AreaId = _currentAreaId,
                OnBuildingIds = _onBuildings
            };
            var onBuildings = PlayerDataManager.Instance.GetPlayerData().OnBuildings;
            onBuildings[onBuildings.FindIndex(x => x.AreaId == _currentAreaId)] = onBuildingData;
            PlayerDataManager.Instance.SetOnBuildings(onBuildings);
        }


        private void Start()
        {
            OpenArea();
        }

        private void OpenArea()
        {
            if (_isLoaded == false) Load();

            if (!_area.isAvailable) return;

            PoolingSystem.Instance.InstantiateUI(_area.areaPoolId, new Vector3(0, 0), Quaternion.identity,
                canvas);
        }


        public BuildingData GetBuilding(int buildingId)
        {
            if (_isLoaded == false)
            {
                Load();
            }

            foreach (var b in _allBuildings)
            {
                if (b.buildingID == buildingId)
                {
                    return b;
                }
            }

            return null;
        }

        public BuildState CheckStatus(int buildId)
        {
            if (_isLoaded == false) Load();

            foreach (var b in _availableBuildings)
            {
                if (buildId == b)
                {
                    return BuildState.Unlocked;
                }
            }

            foreach (var b in _onBuildings)
            {
                if (buildId == b)
                {
                    return BuildState.Built;
                }
            }


            return BuildState.Locked;
        }
    }
}
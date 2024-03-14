using System.Collections.Generic;
using BlastPuzzle.Scripts.Models;

namespace BlastPuzzle.Scripts.Managers
{
    public class PlayerDataManager : Singleton<PlayerDataManager>
    {
        private PlayerData _playerData;

        public PlayerData GetPlayerData()
        {
            if (_playerData == null)
            {
                _playerData = PlayerData.Instance;
                _playerData.Init();
            }

            return _playerData;
        }

        public void SetAvailableBuildings(List<AvailableBuildingData> availableBuildingDataList)
        {
            GetPlayerData().AvailableBuildings = availableBuildingDataList;
        }

        public void SetOnBuildings(List<OnBuildingsData> onBuildingsDataList)
        {
            GetPlayerData().OnBuildings = onBuildingsDataList;
        }

        public void SetUpgradeData(UpgradeTypes upgradeType, int level)
        {
            var tempUpgradeData = GetPlayerData().UpgradeDataList;
            if (tempUpgradeData.FindIndex(x => x.ContainsKey(upgradeType)) == -1)
            {
                tempUpgradeData.Add(new Dictionary<UpgradeTypes, int>() { { upgradeType, level } });
            }
            else
            {
                tempUpgradeData[tempUpgradeData.FindIndex(x => x.ContainsKey(upgradeType))][upgradeType] += level;
            }

            GetPlayerData().UpgradeDataList = tempUpgradeData;
        }

        public int GetUpgradeDataLevel(UpgradeTypes upgradeType, int defaultLevel = 1)
        {
            var tempUpgradeData = GetPlayerData().UpgradeDataList;
            if (tempUpgradeData.FindIndex(x => x.ContainsKey(upgradeType)) == -1)
            {
                tempUpgradeData.Add(new Dictionary<UpgradeTypes, int>() { { upgradeType, defaultLevel } });
                GetPlayerData().UpgradeDataList = tempUpgradeData;
                return defaultLevel;
            }

            return tempUpgradeData.Find(x => x.ContainsKey(upgradeType))[upgradeType];
        }
    }
}
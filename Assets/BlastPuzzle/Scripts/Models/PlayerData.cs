using System;
using System.Collections.Generic;
using Mek.Models.Stats;

namespace BlastPuzzle.Scripts.Models
{
    public sealed class PlayerData : BasePlayerData<PlayerData>, IPlayerData
    {
        private static PlayerData _instance;
        public static PlayerData Instance => _instance ??= new PlayerData();

        public override Dictionary<string, BaseStat> Prefs => new()
        {
            { PrefKeys.AvailableBuildings, new ListStat<AvailableBuildingData>(new List<AvailableBuildingData>()) },
            {
                PrefKeys.UpgradeDataList,
                new ListStat<Dictionary<UpgradeTypes, int>>(new List<Dictionary<UpgradeTypes, int>>())
            },


            {
                PrefKeys.VoiceSetting,
                new BoolStat(true)
            },
            {
                PrefKeys.MusicSetting,
                new BoolStat(true)
            },

            {
                PrefKeys.VibrationSetting,
                new BoolStat(true)
            },
            {
                PrefKeys.NotificationSetting,
                new BoolStat(true)
            },
            {
                PrefKeys.CityStates,
                new DictionaryStat<int, bool>(new Dictionary<int, bool>
                    { { 0, true }, { 1, false }, { 2, false }, { 3, false } }) //TODO
            },
            { PrefKeys.NoAdsEnabled, new BoolStat(false) },
            { PrefKeys.AlwaysRewardAdsWatchedEnabled, new BoolStat(false) }
        };

        public List<AvailableBuildingData> AvailableBuildings
        {
            get => PrefsManager.GetList(PrefKeys.AvailableBuildings, new List<AvailableBuildingData>());
            set => PrefsManager.SetList(PrefKeys.AvailableBuildings, value);
        }

        public List<OnBuildingsData> OnBuildings
        {
            get => PrefsManager.GetList(PrefKeys.OnBuildings, new List<OnBuildingsData>());
            set => PrefsManager.SetList(PrefKeys.OnBuildings, value);
        }


        public List<Dictionary<UpgradeTypes, int>> UpgradeDataList
        {
            get => PrefsManager.GetList(PrefKeys.UpgradeDataList, new List<Dictionary<UpgradeTypes, int>>());
            set => PrefsManager.SetList(PrefKeys.UpgradeDataList, value);
        }


        public bool VoiceSetting
        {
            get => PrefsManager.GetBool(PrefKeys.VoiceSetting, true);
            set => PrefsManager.SetBool(PrefKeys.VoiceSetting, value);
        }

        public bool MusicSetting
        {
            get => PrefsManager.GetBool(PrefKeys.MusicSetting, true);
            set => PrefsManager.SetBool(PrefKeys.MusicSetting, value);
        }

        public bool VibrationSetting
        {
            get => PrefsManager.GetBool(PrefKeys.VibrationSetting, true);
            set => PrefsManager.SetBool(PrefKeys.VibrationSetting, value);
        }

        public bool NotificationSetting
        {
            get => PrefsManager.GetBool(PrefKeys.NotificationSetting, true);
            set => PrefsManager.SetBool(PrefKeys.NotificationSetting, value);
        }

        public Dictionary<int, bool> CityStates
        {
            get => PrefsManager.GetDictionary(PrefKeys.CityStates, new Dictionary<int, bool>());
            set => PrefsManager.SetDictionary(PrefKeys.CityStates, value);
        }

        public bool NoAdsEnabled
        {
            get => PrefsManager.GetBool(PrefKeys.NoAdsEnabled, false);
            set => PrefsManager.SetBool(PrefKeys.NoAdsEnabled, value);
        }

        public bool AlwaysRewardAdsWatchedEnabled
        {
            get => PrefsManager.GetBool(PrefKeys.AlwaysRewardAdsWatchedEnabled, false);
            set => PrefsManager.SetBool(PrefKeys.AlwaysRewardAdsWatchedEnabled, value);
        }
    }


    public interface IPlayerData : IBasePlayerData
    {
        public List<AvailableBuildingData> AvailableBuildings { get; set; }
        public List<OnBuildingsData> OnBuildings { get; set; }
        public List<Dictionary<UpgradeTypes, int>> UpgradeDataList { get; set; }
        public bool VoiceSetting { get; set; }
        public bool MusicSetting { get; set; }
        public bool VibrationSetting { get; set; }
        public bool NotificationSetting { get; set; }
        public Dictionary<int, bool> CityStates { get; set; }
    }

    [Serializable]
    public class AvailableBuildingData
    {
        public int AreaId { get; set; }
        public List<int> AvailableBuildingIds { get; set; }
    }

    [Serializable]
    public class OnBuildingsData
    {
        public int AreaId { get; set; }
        public List<int> OnBuildingIds { get; set; }
    }
}
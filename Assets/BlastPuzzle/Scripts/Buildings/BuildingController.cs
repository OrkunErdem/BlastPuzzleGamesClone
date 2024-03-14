using System;
using BlastPuzzle.Scripts.Buildings;
using BlastPuzzle.Scripts.Managers;
using UnityEngine;

namespace MergeIncome.Scripts.Building
{
    public class BuildingController : MonoBehaviour
    {
        public int buildingId;

        public Action<BuildState> OnBuildStateChanged =
            delegate(BuildState state) { };

        private static AreaManager AreaManager => AreaManager.Instance;
        [HideInInspector] public BuildState buildState;
        public BuildingData BuildingData { get; private set; }


        public void Initialize()
        {
            BuildingData = AreaManager.GetBuilding(buildingId);
            SetBuildingState(AreaManager.CheckStatus(buildingId));
        }

        public void SetBuildingState(BuildState state)
        {
            this.buildState = state;
            OnBuildStateChanged?.Invoke(state);
        }
    }
}

[Serializable]
public class BuildingData
{
    public int buildingID;
}
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BlastPuzzle.Scripts.Managers
{
    public class GoalTracker : MonoBehaviour
    {
        private int _vaseCount;

        [SerializeField] private List<GoalEntity> goalEntities;

        private int VaseCount
        {
            get => _vaseCount;
            set
            {
                var goalEntity = goalEntities.Find(x => x.GetGoalType() == GoalType.Vase);
                _vaseCount = value;
                goalEntity.SetText(_vaseCount.ToString());
                if (value <= 0) return;
                goalEntity.OpenImage();
                goalEntity.CloseGoalImage();
            }
        }

        private int _boxCount;

        private int BoxCount
        {
            get => _boxCount;
            set
            {
                var goalEntity = goalEntities.Find(x => x.GetGoalType() == GoalType.Box);
                _boxCount = value;
                goalEntity.SetText(_boxCount.ToString());
                if (value <= 0) return;
                goalEntity.OpenImage();
                goalEntity.CloseGoalImage();
            }
        }

        private int _stoneCount;

        private int StoneCount
        {
            get => _stoneCount;
            set
            {
                var goalEntity = goalEntities.Find(x => x.GetGoalType() == GoalType.Stone);
                _stoneCount = value;
                goalEntity.SetText(_stoneCount.ToString());
                if (value <= 0) return;
                goalEntity.OpenImage();
                goalEntity.CloseGoalImage();
            }
        }

        private void OnEnable()
        {
            BoxCount = 0;
            VaseCount = 0;
            StoneCount = 0;
            EventSystem.StartListening(Events.ObstacleCreated, "GoalTracker", OnObstacleCreated);
            EventSystem.StartListening(Events.ObstacleDestroyed, "GoalTracker", OnObstacleDestroyed);
        }


        private void OnDisable()
        {
            EventSystem.StopListening(Events.ObstacleCreated, "GoalTracker");
            EventSystem.StopListening(Events.ObstacleDestroyed, "GoalTracker");
        }

        private void OnObstacleDestroyed(EventSystem.EventData obj)
        {
            switch (obj.DataArray[0])
            {
                case "VaseItem":
                    VaseCount--;
                    break;
                case "BoxItem":
                    BoxCount--;
                    break;
                case "StoneItem":
                    StoneCount--;
                    break;
            }

            CheckWin();
        }

        private void CheckWin()
        {
            if (VaseCount == 0 && BoxCount == 0 && StoneCount == 0)
            {
                EventSystem.TriggerEvent(Events.Win);
            }
            else if (VaseCount == 0)
            {
                goalEntities.Find(x => x.GetGoalType() == GoalType.Vase).OpenGoalImage();
            }
            else if (BoxCount == 0)
            {
                goalEntities.Find(x => x.GetGoalType() == GoalType.Box).OpenGoalImage();
            }
            else if (StoneCount == 0)
            {
                goalEntities.Find(x => x.GetGoalType() == GoalType.Stone).OpenGoalImage();
            }
        }

        private void OnObstacleCreated(EventSystem.EventData obj)
        {
            switch (obj.DataArray[0])
            {
                case "VaseItem":
                    VaseCount++;
                    break;
                case "BoxItem":
                    BoxCount++;
                    break;
                case "StoneItem":
                    StoneCount++;
                    break;
            }
        }
    }
}
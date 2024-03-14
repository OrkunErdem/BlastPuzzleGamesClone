using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlastPuzzle.Scripts.Data
{
    [CreateAssetMenu(fileName = "LevelContainer", menuName = "ScriptableObjects/LevelContainer", order = 4)]
    public class LevelContainer : ScriptableObject
    {
        
        [SerializeField] private List<LevelData> levels = new List<LevelData>();

        public List<LevelData> Levels => levels;

        public bool TryGetLevelData(string id, out LevelData levelData)
        {
            levelData = levels.Find(x => x.Id == id);
            return levelData; 
        }

        public void AddLevelData(LevelData levelData)
        {
            if (levels.Contains(levelData)) return;
            levels.Append(levelData);
        }
    }
}
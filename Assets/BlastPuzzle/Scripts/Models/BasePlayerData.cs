using System.Collections.Generic;

namespace BlastPuzzle.Scripts.Models
{
    public abstract class BasePlayerData<T> where T : class, IBasePlayerData, new()
    {
        public abstract Dictionary<string, BaseStat> Prefs { get; }
        public BaseStat this[string key] => PrefsManager.Prefs[key];

        public void Init()
        {
            PrefsManager.InitializeData(Prefs);
        }
    }

    public interface IBasePlayerData
    {
        Dictionary<string, BaseStat> Prefs { get; }
        void Init();
        public BaseStat this[string key] { get; }
    }
}
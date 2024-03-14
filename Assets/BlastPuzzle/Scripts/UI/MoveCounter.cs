using BlastPuzzle.Scripts.Managers.Map;
using TMPro;
using UnityEngine;

namespace BlastPuzzle.Scripts.UI
{
    public class MoveCounter : MonoBehaviour
    {
        private TMP_Text _moveCounterText;

        public TMP_Text MoveCounterText =>
            _moveCounterText ? _moveCounterText : _moveCounterText = GetComponentInChildren<TMP_Text>();

        private void OnEnable()
        {
            EventSystem.StartListening(Events.MoveCountChanged, GetInstanceID() + name, UpdateText);
        }


        private void OnDisable()
        {
            EventSystem.StopListening(Events.MoveCountChanged, GetInstanceID() + name);
        }

        private void UpdateText(EventSystem.EventData obj)
        {
            var x = obj.DataArray[0].ToString();
            MoveCounterText.text = x;
            if ((int)obj.DataArray[0]==0)
            {
                GridManager.Instance.LevelFailed();
            }
        }
    }
}
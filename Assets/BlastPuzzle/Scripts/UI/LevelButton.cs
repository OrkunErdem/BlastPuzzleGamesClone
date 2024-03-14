using BlastPuzzle.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BlastPuzzle.Scripts.UI
{
    public class LevelButton : MonoBehaviour
    {
        private Button _button;
        private Button Button => _button ? _button : _button = GetComponent<Button>();

        private TMP_Text _text;
        private TMP_Text Text => _text ? _text : _text = GetComponentInChildren<TMP_Text>();

        private void OnEnable()
        {
            Button.onClick.AddListener(OnClick);
            var x = PlayerPrefs.GetInt("ActiveLevel", 1);
            UpdateText(x == 10 ? "Finished" : "Level " + x);
        }

        private void OnDisable()
        {
            Button.onClick.RemoveListener(OnClick);
        }


        private void UpdateText(string text)
        {
            Text.text =  text;
        }


        private static void OnClick()
        {
            if ((PlayerPrefs.GetInt("ActiveLevel", 1)) == 10) return;

            if (AreaManager.Instance.ActiveBuilding == 0)
            {
                SceneManager.LoadScene("LevelScene");
            }
        }
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlastPuzzle.Scripts.Buildings
{
    public class BuildingGraphicsController : MonoBehaviour
    {
        private Image _image;
        [SerializeField] private GameObject buildingUI;

        private Image Image
        {
            get
            {
                if (!_image)
                    _image = GetComponentInChildren<Image>();
                return _image;
            }
        }


        public void SetImageSprite(Sprite sprite)
        {
            Image.enabled = true;
            buildingUI.SetActive(false);
            Image.sprite = sprite;
            Image.SetNativeSize();
        }

        public void OpenBuildingUI(int buildingLevel)
        {
            Image.enabled = false;
            buildingUI.SetActive(true);
            buildingUI.transform.localPosition = Vector3.zero;
            buildingUI.GetComponentInChildren<TMP_Text>().text = buildingLevel.ToString();
        }
    }
}
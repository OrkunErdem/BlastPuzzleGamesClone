using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalEntity : MonoBehaviour
{
    [SerializeField] private GoalType goalType;
    [SerializeField] private Image goalImage;
    [SerializeField] private TMP_Text goalText;
    private Image _image;
    private Image Image => _image ? _image : _image = GetComponent<Image>();

    public void OpenImage()
    {
        Image.enabled = true;
    }

    public void OpenGoalImage()
    {
        goalImage.enabled = true;
    }

    public void SetText(string text)
    {
        goalText.text = text;
    }

    public void CloseGoalImage()
    {
        goalImage.enabled = false;
    }

    public GoalType GetGoalType()
    {
        return goalType;
    }
}


public enum GoalType
{
    Vase,
    Box,
    Stone
}
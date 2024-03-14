using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    private Button button;
    private Button Button=>button==null?button=GetComponent<Button>():button;
    

    public TMP_Text LevelText;
    public TMP_Text PriceText;
    [SerializeField] private UpgradeTypes upgradeType;
    [SerializeField] private int amount;
    [SerializeField] private Sprite disabledSprite;
    private Sprite enableSprite;
    private bool isUpgradable = false;
    private void OnEnable()
    {
        enableSprite=Button.image.sprite;
        UpdateButtonText();
        CheckButtonStatus();
        Button.onClick.AddListener(() => DoUpgrade(upgradeType, amount));
        EventSystem.StartListening(Events.OnExchange, name+upgradeType, (a) =>
        {
            CheckButtonStatus();
        });
    }
    
    private void OnDisable()
    {
        Button.onClick.RemoveListener(() => DoUpgrade(upgradeType, amount));
        EventSystem.StopListening(Events.OnExchange, name + upgradeType);
    }
    private void DoUpgrade(UpgradeTypes _upgradeType, int _amount)
    {
        if(!isUpgradable)return;
        ReScale();
        ExchangeManager.Instance.DoExchange(ExchangeType.Coin, CalculateFee() * -1);
        UpgradeManager.Instance.DoUpgrade(_upgradeType, _amount);
        isUpgradable = false;
        CheckButtonStatus();
        UpdateButtonText();
    }
    private void UpdateButtonText()
    {
        if (UpgradeManager.Instance.GetUpgradeLevel(upgradeType) ==
            UpgradeManager.Instance.GetUpgradeTypeData(upgradeType).MaxLevel)
        {
            PriceText.text = "Max";
            LevelText.text = " Max Level";
            Button.image.sprite = disabledSprite;
        }
        else
        {
            PriceText.text = CalculateFee() + "";
            LevelText.text = "Level " + UpgradeManager.Instance.GetUpgradeLevel(upgradeType);
        }
    }

    private float CalculateFee()
    {
        return UpgradeManager.Instance.GetUpgradeFee(upgradeType);
    }
    private IEnumerator CheckButtonStatusCoroutine()
    {
        yield return new WaitForSeconds(.2f);
        CheckButtonStatus();
    }
    private void CheckButtonStatus()
    {
        if (ExchangeManager.Instance.GetExchange(ExchangeType.Coin) + CalculateFee() * -1 < 0)
        {
            ChangeButtonSprite(false);
        }else if (!UpgradeManager.Instance.CanUpdatable(upgradeType, amount))
        {
            ChangeButtonSprite(false);
        }else
        {
            ChangeButtonSprite(true);
        }

    }
    private void ChangeButtonSprite(bool _isUpgradable)
    {
        isUpgradable = _isUpgradable;
        if (_isUpgradable)
        {
            Button.image.sprite = enableSprite;
        }
        else
        {
            Button.image.sprite = disabledSprite;
        }
    }
    private void ReScale()
    {
        transform.DOScale(.9f, .1f).OnComplete(() => { transform.DOScale(1f, .1f); });
    }
}
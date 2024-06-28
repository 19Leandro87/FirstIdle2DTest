using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable] public class UpgradeLineObject 
{
    public GameObject upgradeLine;
    public Image upgradeImage;
    public Button buyButton;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI shortDescriptionText;
    public GlobalValues.UpgradeTypes Type;
}

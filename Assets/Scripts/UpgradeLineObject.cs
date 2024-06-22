using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable] public class UpgradeLineObject 
{
    [SerializeField] public Image upgradeImage;
    [SerializeField] public Button buyButton;
    [SerializeField] public TextMeshProUGUI priceText;
    [SerializeField] public TextMeshProUGUI shortDescriptionText;
}

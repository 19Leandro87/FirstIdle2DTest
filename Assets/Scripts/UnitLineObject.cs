using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable] public class UnitLineObject
{
    [SerializeField] public Image unitImage;
    [SerializeField] public Button buyButton;
    [SerializeField] public TextMeshProUGUI priceText;
    [SerializeField] public TextMeshProUGUI levelText;
}

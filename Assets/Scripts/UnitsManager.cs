using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI unit1CostText;
    private float unit1Cost;
    [SerializeField] private TextMeshProUGUI unit1CountText;
    private float unit1Count;
    [SerializeField] private Button unit1BuyButton;

    private void Awake() {
        unit1Cost = GlobalValues.UNIT1_BASE_PRICE;
        unit1CostText.text = "$ " + unit1Cost;
        unit1Count = 0;
        unit1CountText.text = "N. " + unit1Count.ToString();
    }

    private void Start() {
        unit1BuyButton.onClick.AddListener(() => { Debug.Log("UEUEUE"); });

    }


}

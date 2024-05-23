using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitsManager : MonoBehaviour
{
    public static UnitsManager Instance { get; private set; }

    private int buyMultiplier;

    //---UNIT 1---
    [SerializeField] private TextMeshProUGUI unit1CostText;
    private float unit1Cost;
    [SerializeField] private TextMeshProUGUI unit1CountText;
    private int unit1Count;
    private float unit1PollutionClean;
    [SerializeField] private Button unit1BuyButton;

    private void Awake() {
        Instance = this;
        buyMultiplier = 1;

        //---UNIT 1---
        unit1Cost = GlobalValues.UNIT1_BASE_PRICE;
        unit1CostText.text = "$ " + unit1Cost;
        unit1Count = 0;
        unit1CountText.text = "N. " + unit1Count.ToString();
        unit1PollutionClean = GlobalValues.UNIT1_BASE_POLLUTION_CLEAN;
    }

    private void Start() {
        unit1BuyButton.onClick.AddListener(() => { Debug.Log("UEUEUE"); });

    }

    private void UpdateUnitLine(int multiplier, float presentCost, float presentPollutionClean, int presentCount) {
        
    }

    public void ChangeBuyMultiplier(int multiplier) { 
        buyMultiplier = multiplier; 
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitsManager : MonoBehaviour
{
    public static UnitsManager Instance { get; private set; }

    [SerializeField] private List<UnitLineObject> unitLines;

    private int buyMultiplier;
    private List<UnitObject> units;

    private void Awake() {
        Instance = this;
        buyMultiplier = 1;
    }

    private void Start() {
        units = new List<UnitObject>();
        units.Add(GlobalValues.BASE_UNITS[0]);

        units[0].Price = 100;
        unitLines[0].priceText.text = units[0].Price.ToString();

        unitLines[0].buyButton.onClick.AddListener(() => {
            unitLines[0].priceText.text = (GlobalValues.BASE_UNITS[0].PriceFactor * units[0].Price).ToString();
        });

    }

    private void Update() {
        UnitsUnlock();

    }

    private void UpdateUnitLine(int multiplier, float presentCost, float presentPollutionClean, int presentLevel) {

    }

    public void ChangeBuyMultiplier(int multiplier) { 
        buyMultiplier = multiplier; 
    }

    public void UnitsUnlock() { 
        //depending on the pollution level, set units enabled to true gradually
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitsManager : MonoBehaviour
{
    public static UnitsManager Instance { get; private set; }

    [SerializeField] private List<GameObject> unitLines;
    [SerializeField] private List<UnitLineObject> unitDataFields;
    private List<UnitObject> units;
    private int buyMultiplier;
    private float timer, updateUnitsUnlockInterval;
    private SaveObject loadedObject;

    //COST PROGRESSION HYPOTHESIS:
    //cost_{next} = cost_{previous} + (pricefactor)^{level}
    //cost_{next} = cost_{base} + (pricefactor)^{level}
    //cost_{next} = cost_{base} * (pricefactor)^{level owned} <<<<<<<<<<<<

    private void Awake() {
        Instance = this;
        buyMultiplier = 5;
        timer = 0;
        updateUnitsUnlockInterval = 2f;
        units = new List<UnitObject>();
    }

    private void Start() {
        //create a copy of the BASE_UNITS to work with, without altering the actual BASE_UNITS
        for (int i = 0; i < GlobalValues.BASE_UNITS.Count; i++) {
            units.Add(new UnitObject());
            units[i].Enabled = GlobalValues.BASE_UNITS[i].Enabled;
            units[i].Name = GlobalValues.BASE_UNITS[i].Name;
            units[i].Level = GlobalValues.BASE_UNITS[i].Level;
            units[i].Price = GlobalValues.BASE_UNITS[i].Price;
            units[i].PriceFactor = GlobalValues.BASE_UNITS[i].PriceFactor;
            units[i].PollutionClean = GlobalValues.BASE_UNITS[i].PollutionClean;
            units[i].PollutionCleanFactor = GlobalValues.BASE_UNITS[i].PollutionCleanFactor;
            units[i].PollutionUnlockPercentage = GlobalValues.BASE_UNITS[i].PollutionUnlockPercentage;
        }

        //if there's a saved game update all the units
        if (SaveSystem.SaveGamesExist()) {
            loadedObject = JsonUtility.FromJson<SaveObject>(SaveSystem.Load());
            for (int i = 0; i < units.Count; i++) {
                unitLines[i].gameObject.SetActive(loadedObject.unitLinesEnabled[i]);
                units[i].Level = loadedObject.unitsLevel[i];
                units[i].Price = loadedObject.unitsPrice[i];
                units[i].PollutionClean = GlobalValues.BASE_UNITS[i].PollutionClean * units[i].Level * units[i].PollutionCleanFactor;
                UnitTextFieldsUpdate(i);
            }
        }

        //Add the level up function to each unit's buy button
        foreach (var unit in units) {
            int unitIndex = units.FindIndex(obj => obj.Name.Contains(unit.Name));
            unitDataFields[unitIndex].buyButton.onClick.AddListener(() => { UnitLevelUp(unitIndex); });
        }
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer > updateUnitsUnlockInterval) {
            timer = 0;
            UnitsUnlock();
        } 
    }

    private void UnitLevelUp2(int unitIndex) {
        for (int i = 0; i < buyMultiplier; i++) {
            units[unitIndex].Price = GlobalValues.BASE_UNITS[unitIndex].Price * Mathf.Pow(GlobalValues.BASE_UNITS[unitIndex].PriceFactor, units[unitIndex].Level);
            units[unitIndex].Level++;
            Debug.Log("Level: " + units[unitIndex].Level + " - Price:  " + units[unitIndex].Price + " - Base price: " + GlobalValues.BASE_UNITS[unitIndex].Price);
        }
    }

    private void UnitLevelUp(int unitIndex) {
        float referenceCost = units[unitIndex].Price;
        int referenceLevel = units[unitIndex].Level;
        float referenceCostIncremented = referenceCost;

        if (buyMultiplier > 1)
            for (int i = 0; i < buyMultiplier; i++) {
                referenceCost = GlobalValues.BASE_UNITS[unitIndex].Price * Mathf.Pow(GlobalValues.BASE_UNITS[unitIndex].PriceFactor, referenceLevel);
                referenceLevel++;
                referenceCostIncremented += referenceCost;
            }
        referenceCostIncremented = Mathf.Ceil(referenceCostIncremented);
        Debug.Log("referenceCostIncremented " + referenceCostIncremented);

        if (WorldStatsManager.Instance.GetMoney() >= referenceCostIncremented) {
            for (int i = 0; i < buyMultiplier; i++) {
                units[unitIndex].Price = GlobalValues.BASE_UNITS[unitIndex].Price * Mathf.Pow(GlobalValues.BASE_UNITS[unitIndex].PriceFactor, units[unitIndex].Level);
                units[unitIndex].Level++;
                WorldStatsManager.Instance.UpdateMoney(-(long)units[unitIndex].Price);
            }
            units[unitIndex].Price = Mathf.Ceil(units[unitIndex].Price);
            Debug.Log("price " + units[unitIndex].Price);

            WorldStatsManager.Instance.UpdateTexts(0);

            units[unitIndex].PollutionClean += units[unitIndex].PollutionCleanFactor * GlobalValues.BASE_UNITS[unitIndex].PollutionClean * buyMultiplier;

            UnitTextFieldsUpdate(unitIndex);
            WorldStatsManager.Instance.SaveStats();
        }
    }

    private void UnitTextFieldsUpdate(int unitIndex) {
        unitDataFields[unitIndex].priceText.text = Mathf.Ceil(units[unitIndex].Price).ToString();
        unitDataFields[unitIndex].levelText.text = units[unitIndex].Level.ToString();
    }

    public void SetBuyMultiplier(int multiplier) { buyMultiplier = multiplier; }

    public void UnitsUnlock() {
        float worldUpdatedPollution = WorldStatsManager.Instance.GetUpdatedPollution();
        for (int i = 0; i < GlobalValues.BASE_UNITS.Count; i++)
            if (worldUpdatedPollution < GlobalValues.BASE_POLLUTION - GlobalValues.BASE_POLLUTION * GlobalValues.BASE_UNITS[i].PollutionUnlockPercentage/100) 
                unitLines[i].SetActive(true);
    }

    public List<bool> GetEnabledUnitLines() {
        List<bool> enabled = new List<bool>();
        foreach (var unitLine in unitLines) enabled.Add(unitLine.activeSelf);
        return enabled;
    }

    public List<int> GetUnitsLevel() {
        List<int> levels = new List<int>();
        foreach (var unit in units) levels.Add(unit.Level);
        return levels;
    }

    public List<float> GetUnitsPrice() {
        List<float> prices = new List<float>();
        foreach (var unit in units) prices.Add(unit.Price);
        return prices;
    }
}

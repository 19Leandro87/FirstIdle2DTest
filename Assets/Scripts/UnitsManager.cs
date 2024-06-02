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
    //cost_{next} = cost_{base} * (pricefactor)^{level}

    private void Awake() {
        Instance = this;
        buyMultiplier = 1;
        timer = 0;
        updateUnitsUnlockInterval = 2f;
        units = new List<UnitObject>();

        //Add each basic unit to the units list, then if there's a saved game update them all
        for (int i = 0; i < GlobalValues.BASE_UNITS.Count; i++) {
            units.Add(GlobalValues.BASE_UNITS[i]);
            if (SaveSystem.SaveGamesExist()) {
                loadedObject = JsonUtility.FromJson<SaveObject>(SaveSystem.Load());
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

    private void Start() {

    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer > updateUnitsUnlockInterval) {
            timer = 0;
            UnitsUnlock();
        } 
    }

    private void UnitLevelUp(int unitIndex) {
        if (WorldStatsManager.Instance.GetMoney() >= units[unitIndex].Price) {
            long cost = (long)units[unitIndex].Price;
            Debug.Log(cost);
            WorldStatsManager.Instance.UpdateMoney(-cost);
            WorldStatsManager.Instance.UpdateWorldStats(0);
            units[unitIndex].Level += buyMultiplier;
            units[unitIndex].Price += Mathf.Ceil(Mathf.Pow(GlobalValues.BASE_UNITS[unitIndex].PriceFactor, units[unitIndex].Level));
            units[unitIndex].PollutionClean += units[unitIndex].PollutionCleanFactor * GlobalValues.BASE_UNITS[unitIndex].PollutionClean * buyMultiplier;

            UnitTextFieldsUpdate(unitIndex);
            WorldStatsManager.Instance.SaveStats();
        }
    }

    private void UnitTextFieldsUpdate(int unitIndex) {
        unitDataFields[unitIndex].priceText.text = Mathf.Ceil(units[unitIndex].Price).ToString();
        unitDataFields[unitIndex].levelText.text = units[unitIndex].Level.ToString();
    }

    public void ChangeBuyMultiplier(int multiplier) { buyMultiplier = multiplier; }

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

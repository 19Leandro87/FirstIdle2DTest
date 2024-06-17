using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class WorldStatsManager : MonoBehaviour 
{
    public static WorldStatsManager Instance {get; private set;}
    private double updatedPollution;
    private double money;
    [SerializeField] private TextMeshProUGUI pollutionText;
    [SerializeField] private TextMeshProUGUI cleanlinessText;
    [SerializeField] private TextMeshProUGUI moneyText;
    private SaveObject saveObject;
    private SaveObject loadedObject;
    private float saveInterval;
    private float timer;

    private void Awake() {
        Instance = this;
        saveObject = new SaveObject();
        timer = 0;
        saveInterval = 5f;
    }

    private void Start() {
        //if there are no save games, updated pollution = base pollution and money = 0, else updated pollution and money = last saved data
        if (!SaveSystem.SaveGamesExist()) {
            updatedPollution = GlobalValues.BASE_POLLUTION;
            money = 0;
        }
        else {
            loadedObject = JsonUtility.FromJson<SaveObject>(SaveSystem.Load());
            updatedPollution = loadedObject.updatedPollution;
            money = loadedObject.updatedMoney + GlobalValues.timeSinceLast * loadedObject.cumulativePollutionCleaning;
        }
        UpdateTexts(100 * updatedPollution / GlobalValues.BASE_POLLUTION);
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer > saveInterval) {
            timer = 0;
            SaveStats();
        }
    }

    //money increases by the same amount of which pollution decreases with a 1:1 ratio
    public void UpdateWorldStats(double pollutionChange) {
        updatedPollution -= pollutionChange;
        UpdateMoney(pollutionChange);
        saveObject.updatedPollution = updatedPollution;
        saveObject.updatedMoney = money;
        UpdateTexts(100 * updatedPollution / GlobalValues.BASE_POLLUTION);
    }

    public void UpdateTexts(double pollution) {
        pollutionText.text = "Pollution: " + pollution.ToString("F2") + "%";
        cleanlinessText.text = "The World is " + (100 - pollution).ToString("F5") + "% clean";
        moneyText.text = "$ " + GlobalValues.MoneyStringNumbersFormat(Math.Round(money, 2));
    }

    public void UpdateMoney(double moneyChange) { money += moneyChange; }

    public void SaveStats() {
        saveObject.updatedPollution = updatedPollution;
        saveObject.updatedMoney = money;
        saveObject.unitLinesEnabled = UnitsManager.Instance.GetEnabledUnitLines();
        saveObject.unitsLevel = UnitsManager.Instance.GetUnitsLevel();
        saveObject.unitsPrice = UnitsManager.Instance.GetUnitsPrice();
        saveObject.realWorldTime = WorldTimeAPI.Instance.GetRealTime();
        saveObject.cumulativePollutionCleaning = UnitsManager.Instance.GetCumulativePollutionClean();
        SaveSystem.Save(JsonUtility.ToJson(saveObject)); 
    }

    public void SetMoney(double updatedMoney) { money = updatedMoney; }

    public double GetMoney() { return money; }

    public double GetUpdatedPollution() { return updatedPollution; }

    public void SetUnitLinesEnabled(List<bool> unitLinesEnabled) { saveObject.unitLinesEnabled = unitLinesEnabled; }
}

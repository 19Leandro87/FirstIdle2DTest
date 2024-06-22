using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitsManager : MonoBehaviour 
{
    public static UnitsManager Instance { get; private set; }

    [SerializeField] private List<GameObject> unitLines;
    [SerializeField] private List<UnitLineObject> unitDataFields;
    private List<UnitObject> units;
    private int buyMultiplier = 1;
    private float timer, unitsUnlockAndCleanUpdate;
    private SaveObject loadedObject;
    private float cumulativePollutionClean = 0;

    //COST PROGRESSION HYPOTHESIS:
    //cost_{next} = cost_{base} + level * (pricefactor)^{level}
    //cost_{next} = cost_{base} * (pricefactor)^{level owned} <<<<<<<<<<<<

    private void Awake() {
        Instance = this;
        timer = 0;
        unitsUnlockAndCleanUpdate = 1f;
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
            units[i].Description = GlobalValues.BASE_UNITS[i].Description;
            UnitTextFieldsUpdate(i);
        }

        //if there's a saved game update all the units and the pollution clean
        if (SaveSystem.SaveGamesExist()) {
            loadedObject = JsonUtility.FromJson<SaveObject>(SaveSystem.Load());
            for (int i = 0; i < units.Count; i++) {
                unitLines[i].gameObject.SetActive(loadedObject.unitLinesEnabled[i]);
                units[i].Level = loadedObject.unitsLevel[i];
                units[i].Price = loadedObject.unitsPrice[i];
                UnitTextFieldsUpdate(i);
            }
            cumulativePollutionClean = loadedObject.cumulativePollutionCleaning;
        }

        foreach (var unit in units) {
            //Add the level up function to each unit's buy button
            int unitIndex = units.FindIndex(obj => obj.Name.Contains(unit.Name));
            unitDataFields[unitIndex].buyButton.onClick.AddListener(() => { UnitLevelUp(unitIndex); });

            //then add the open description when clicked/tapped function to each unit's image 
            void OpenDescription (BaseEventData eventData) { 
                DescriptionCanvas.Instance.TriggerEnabled();
                DescriptionCanvas.Instance.SetDescription(units[unitIndex].Description);
            }
            EventTrigger.Entry pointDown = new EventTrigger.Entry() { eventID = EventTriggerType.PointerDown };
            pointDown.callback.AddListener(OpenDescription);
            unitDataFields[unitIndex].unitImage.AddComponent<EventTrigger>().triggers.Add(pointDown);
        }

        //reset the buy multiplier
        SetBuyMultiplier(1);
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer > unitsUnlockAndCleanUpdate) {
            UnitsUnlock();
            UnitsPollutionCleaning();
            timer = 0;
        } 
    }

    private void UnitLevelUp(int unitIndex) {
        double referenceCost = 0;
        long referenceLevel = units[unitIndex].Level;

        for (int i = 0; i < buyMultiplier; i++) {
            referenceCost += PriceUp(unitIndex, referenceLevel);
            referenceLevel++;
        }

        //level up the unit if the available money is greater than the total level up cost 
        if (WorldStatsManager.Instance.GetMoney() >= referenceCost) {
            for (int i = 0; i < buyMultiplier; i++) {
                units[unitIndex].Price = PriceUp(unitIndex, units[unitIndex].Level);
                units[unitIndex].Level++;
                WorldStatsManager.Instance.UpdateMoney(-units[unitIndex].Price);
                cumulativePollutionClean += units[unitIndex].PollutionClean * units[unitIndex].PollutionCleanFactor;
            }

            WorldStatsManager.Instance.UpdateWorldStats(0);
            UnitTextFieldsUpdate(unitIndex);
            WorldStatsManager.Instance.SaveStats();
            SetBuyMultiplier(buyMultiplier);
        }
    }

    private void UnitTextFieldsUpdate(int unitIndex) {
        unitDataFields[unitIndex].priceText.text = GlobalValues.PriceStringNumbersFormat(PriceUp(unitIndex, units[unitIndex].Level));
        unitDataFields[unitIndex].levelText.text = units[unitIndex].Level.ToString();
    }

    private void UnitsPollutionCleaning() { WorldStatsManager.Instance.UpdateWorldStats(cumulativePollutionClean); }

    public float GetCumulativePollutionClean() {  return cumulativePollutionClean; }

    private double PriceUp(int index, long level) { return Math.Round(GlobalValues.BASE_UNITS[index].Price * Math.Pow(GlobalValues.BASE_UNITS[index].PriceFactor, level), 2); }

    public void SetBuyMultiplier(int multiplier) { 
        buyMultiplier = multiplier;
        for (int i = 0; i < units.Count; i++) {
            unitDataFields[i].buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "BUY " + buyMultiplier.ToString();
            double referenceCost = 0;
            long referenceLevel = units[i].Level;
            for (int j = 0; j < multiplier; j++) {
                referenceCost += PriceUp(i, referenceLevel);
                referenceLevel++;
            }
            unitDataFields[i].priceText.text = GlobalValues.PriceStringNumbersFormat(referenceCost);
        } 
    }

    public void UnitsUnlock() {
        double worldUpdatedPollution = WorldStatsManager.Instance.GetUpdatedPollution();
        for (int i = 0; i < GlobalValues.BASE_UNITS.Count; i++)
            if (worldUpdatedPollution < GlobalValues.BASE_POLLUTION - GlobalValues.BASE_POLLUTION * GlobalValues.BASE_UNITS[i].PollutionUnlockPercentage/100) 
                unitLines[i].SetActive(true);
    }

    public List<bool> GetEnabledUnitLines() {
        List<bool> enabled = new List<bool>();
        foreach (var unitLine in unitLines) enabled.Add(unitLine.activeSelf);
        return enabled;
    }

    public List<long> GetUnitsLevel() {
        List<long> levels = new List<long>();
        foreach (var unit in units) levels.Add(unit.Level);
        return levels;
    }

    public List<double> GetUnitsPrice() {
        List<double> prices = new List<double>();
        foreach (var unit in units) prices.Add(unit.Price);
        return prices;
    }
}

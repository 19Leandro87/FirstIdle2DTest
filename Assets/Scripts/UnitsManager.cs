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

     //COST PROGRESSION HYPOTHESIS: cost_{next} = cost_{base} * (pricefactor)^{level}
     
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
                unitLines[i].gameObject.SetActive(JsonUtility.FromJson<SaveObject>(SaveSystem.Load()).unitLinesEnabled[i]);
                units[i].Level = JsonUtility.FromJson<SaveObject>(SaveSystem.Load()).unitsLevel[i];

                if (units[i].Level > 0) {
                    units[i].Price = GlobalValues.BASE_UNITS[i].Price * Mathf.Pow(GlobalValues.BASE_UNITS[i].PriceFactor, units[i].Level);
                    units[i].PollutionClean = GlobalValues.BASE_UNITS[i].PollutionClean * units[i].Level * units[i].PollutionCleanFactor;
                }
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
        units[unitIndex].Level += buyMultiplier;
        units[unitIndex].PollutionClean += units[unitIndex].PollutionCleanFactor * GlobalValues.BASE_UNITS[unitIndex].PollutionClean * buyMultiplier;
        units[unitIndex].Price = GlobalValues.BASE_UNITS[unitIndex].Price * Mathf.Pow(GlobalValues.BASE_UNITS[unitIndex].PriceFactor, units[unitIndex].Level);

        UnitTextFieldsUpdate(unitIndex);
        WorldStatsManager.Instance.SaveStats();
    }

    private void UnitTextFieldsUpdate(int unitIndex) {
        unitDataFields[unitIndex].priceText.text = Mathf.Floor(units[unitIndex].Price).ToString();
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
}

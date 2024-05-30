using System.Collections;
using System.Collections.Generic;
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


    /*
     COST PROGRESSION HYPOTHESIS <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    cost_{next} = cost_{base} * (pricefactor)^{level}

     */

    private void Awake() {
        Instance = this;
        buyMultiplier = 1;
        units = new List<UnitObject>();

        //Add a basic unit for each existing unit line to the units list, then if there's a save game update them all
        for (int i = 0; i < unitLines.Count; i++) {
            units.Add(GlobalValues.BASE_UNITS[i]);
            if (SaveSystem.SaveGamesExist()) {
                unitLines[i].gameObject.SetActive(JsonUtility.FromJson<SaveObject>(SaveSystem.Load()).unitsEnabled[i]);
                units[i].Level = JsonUtility.FromJson<SaveObject>(SaveSystem.Load()).unitsLevel[i];

                if (units[i].Level > 0) {
                    units[i].Price = GlobalValues.BASE_UNITS[i].Price * Mathf.Pow(GlobalValues.BASE_UNITS[i].PriceFactor, units[i].Level);
                    units[i].PollutionClean = GlobalValues.BASE_UNITS[i].PollutionClean * units[i].Level * units[i].PollutionCleanFactor;
                }
                UnitTextFieldsUpdate(i);
            }
            unitDataFields[i].buyButton.onClick.AddListener(() => { UnitLevelUp(i); });
        }
    }

    private void Start() {

    }

    private void Update() {
        UnitsUnlock();
    }

    private void UnitLevelUp(int unitIndex) {
        units[unitIndex].Level += buyMultiplier;
        units[unitIndex].PollutionClean += units[unitIndex].PollutionCleanFactor * GlobalValues.BASE_UNITS[unitIndex].PollutionClean * buyMultiplier;
        units[unitIndex].Price = GlobalValues.BASE_UNITS[unitIndex].Price * Mathf.Pow(GlobalValues.BASE_UNITS[unitIndex].PriceFactor, units[unitIndex].Level);

        UnitTextFieldsUpdate(unitIndex);
    }

    private void UnitTextFieldsUpdate(int unitIndex) {
        unitDataFields[unitIndex].priceText.text = units[unitIndex].Price.ToString();
        unitDataFields[unitIndex].levelText.text = units[unitIndex].Level.ToString();
    }

    public void ChangeBuyMultiplier(int multiplier) { 
        buyMultiplier = multiplier; 
    }

    public void UnitsUnlock() { 
        //depending on the pollution level, set units enabled to true gradually
    }

}

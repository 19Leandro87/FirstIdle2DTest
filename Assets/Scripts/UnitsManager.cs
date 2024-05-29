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

                if (units[i].Level > 0)
                    units[i].Price = GlobalValues.BASE_UNITS[i].Price * Mathf.Pow(GlobalValues.BASE_UNITS[i].PriceFactor, units[i].Level);
                else
                    units[i].Price = GlobalValues.BASE_UNITS[i].Price;

                unitDataFields[i].priceText.text = units[i].Price.ToString();
                unitDataFields[i].levelText.text = units[i].Level.ToString();
            }
        }
    }

    private void Start() {


        units.Add(GlobalValues.BASE_UNITS[0]);

        units[0].Price = 100;
        //unitLines[0].priceText.text = units[0].Price.ToString();

        /*unitLines[0].buyButton.onClick.AddListener(() => {
            unitLines[0].priceText.text = (GlobalValues.BASE_UNITS[0].PriceFactor * units[0].Price).ToString();
        });*/

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

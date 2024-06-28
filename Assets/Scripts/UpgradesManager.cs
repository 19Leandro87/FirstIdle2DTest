/*
INFO
All upgrades will be disabled at first and then enabled when the conditions are met, and then disabled again after being bought, and so on.
The UNIT RELATED upgrades will enable on certain connected unit levels, the same for all units.
The POLLUTION RELATED ones will be unlocked at certain predetermined pollution levels.
The SPECIAL upgrades are to be considered mostly individually.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class UpgradesManager : MonoBehaviour
{
    public static UpgradesManager Instance { get; private set; }

    [SerializeField] private List<UpgradeLineObject> upgradeDataFields;
    private List<UpgradeObject> unitConnectedUpgrades, pollutionRelatedUpgrades, specialUpgrades;
    private readonly long[] UNIT_UPGRADES_UNLOCK_LEVEL = { 10, 25, 50, 100, 200, 250, 500, 1000, 2500, 5000, 10000 }; 
    private float timer, upgradesUnlockCheck;

    private void Awake() {  
        Instance = this;
        timer = 0;
        upgradesUnlockCheck = 3f;
        unitConnectedUpgrades = new List<UpgradeObject>();
        pollutionRelatedUpgrades = new List<UpgradeObject>();
        specialUpgrades = new List<UpgradeObject>();
    }

    private void Start() {
        //Setup all the upgrades lists and lines  
        UpgradeListsAndLinesConfig(GlobalValues.BASE_UNIT_CONN_UPGRADES, unitConnectedUpgrades);
        UpgradeListsAndLinesConfig(GlobalValues.BASE_POLLUTION_REL_UPGRADES, pollutionRelatedUpgrades);
        UpgradeListsAndLinesConfig(GlobalValues.BASE_SPECIAL_UPGRADES, specialUpgrades);
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer > upgradesUnlockCheck) {
            UpdateUpgrades();
            timer = 0;
        }
    }

    /*
        Action a = () => BuyUpgrade(1);
        TEST(allUpgrades, a);*/

    private void TEST(List<UpgradeObject> upgradeObjects, Action method) {
        foreach (var upgradeObject in upgradeObjects) {
            
        }
    }

    private void UpgradeListsAndLinesConfig(List<UpgradeObject> baseList, List<UpgradeObject> copyList) {
        for (int i = 0; i < baseList.Count; i++) {
            //create a copy of the BASE_UPGRADES (for each type) to work with, without altering the actual BASE_UPGRADES  
            copyList.Add(new UpgradeObject());
            copyList[i].Enabled = baseList[i].Enabled;
            copyList[i].Type = baseList[i].Type;
            copyList[i].Name = baseList[i].Name;
            copyList[i].ConnectedUnitIndex = baseList[i].ConnectedUnitIndex;
            copyList[i].PollutionUnlockPercentage = baseList[i].PollutionUnlockPercentage;
            copyList[i].TimesBought = baseList[i].TimesBought;
            copyList[i].Price = baseList[i].Price;
            copyList[i].ShortDescription = baseList[i].ShortDescription;
            copyList[i].FullDescription = baseList[i].FullDescription;
        }

        //Identify and set each upgrade's real upgrades list index
        List<int> upgradeLinesIndex = new List<int>();
        for (int i = 0; i < upgradeDataFields.Count; i++)
            if (upgradeDataFields[i].Type == copyList[0].Type)
                upgradeLinesIndex.Add(i);

        for (int i = 0; i < copyList.Count; i++) copyList[i].LineIndex = upgradeLinesIndex[i];

        //configure buy buttons and images tap behavior 
        foreach (UpgradeObject upgrade in copyList) {
            upgradeDataFields[upgrade.LineIndex].buyButton.onClick.AddListener(() => { BuyUpgrade(upgrade.LineIndex); });

            void OpenDescription(BaseEventData eventData) {
                DescriptionCanvas.Instance.TriggerEnabled();
                DescriptionCanvas.Instance.SetDescription(upgrade.FullDescription);
            }
            EventTrigger.Entry pointDown = new EventTrigger.Entry() { eventID = EventTriggerType.PointerDown };
            pointDown.callback.AddListener(OpenDescription);
            upgradeDataFields[upgrade.LineIndex].upgradeImage.AddComponent<EventTrigger>().triggers.Add(pointDown);
        }
    }

    private void BuyUpgrade(int upgradeIndex) { Debug.Log("funziooouna " + upgradeIndex); }

    private void UpdateUpgrades() {/*
        //unit connected upgrades unlock conditions check
        List<long> unitsLevels = UnitsManager.Instance.GetUnitsLevel();
        for (int i = 0; i < unitConnectedUpgrades.Count; i++)
            if (UNIT_UPGRADES_UNLOCK_LEVEL.Contains(unitsLevels[i])) { }


        //pollution related upgrades unlock conditions check
        double worldUpdatedPollution = WorldStatsManager.Instance.GetUpdatedPollution();
        for (int i = 0; i < pollutionRelatedUpgrades.Count; i++)
            if (worldUpdatedPollution < GlobalValues.BASE_POLLUTION - GlobalValues.BASE_POLLUTION * pollutionRelatedUpgrades[i].PollutionUnlockPercentage / 100)
                pollutionUpgradeLines[i].SetActive(true);*/
    }

    public List<bool> GetEnabledUpgradeLines() {
        List<bool> enabled = new List<bool>();
        //foreach (var upgradeLine in allUpgradeLines) enabled.Add(upgradeLine.activeSelf);
        return enabled;
    }
}

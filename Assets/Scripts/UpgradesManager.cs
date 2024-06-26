/*
INFO
All upgrades will be disabled at first and then enabled when the conditions are met, and then disabled again after being bought, and so on.
The UNIT RELATED upgrades will enable on certain connected unit levels, the same for all units.
The POLLUTION RELATED ones will be unlocked at certain predetermined pollution levels.
The SPECIAL upgrades are to be considered mostly individually.
*/

using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradesManager : MonoBehaviour
{
    public static UpgradesManager Instance { get; private set; }

    [SerializeField] private List<UpgradeLineObject> upgradeDataFields;
    private List<UpgradeObject> unitConnectedUpgrades, pollutionRelatedUpgrades, specialUpgrades;
    private readonly long[] UNIT_UPGRADES_UNLOCK_LEVEL = { 10, 25, 50, 100, 200, 250, 500, 1000, 2500, 5000, 10000 };
    private List<int> purchasableUpgradesPerUnit;
    private float timer, upgradesUnlockCheck;

    private void Awake() {  
        Instance = this;
        timer = 0;
        upgradesUnlockCheck = 3f;
        unitConnectedUpgrades = new List<UpgradeObject>();
        pollutionRelatedUpgrades = new List<UpgradeObject>();
        specialUpgrades = new List<UpgradeObject>();
        purchasableUpgradesPerUnit = new List<int>();
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
            UnlockUpdateUpgrades();
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
        //if unitConnectedUpgrades has been initialized, initialize purchasableUpgradesPerUnit
        if (copyList[0].Type == GlobalValues.UpgradeTypes.UnitConnected)
            foreach (UpgradeObject upgrade in unitConnectedUpgrades) 
                purchasableUpgradesPerUnit.Add(0);


        //Identify and set each upgrade's real upgrades list index
        List<int> upgradeLinesIndex = new List<int>();
        for (int i = 0; i < upgradeDataFields.Count; i++)
            if (upgradeDataFields[i].Type == copyList[0].Type)
                upgradeLinesIndex.Add(i);

        for (int i = 0; i < copyList.Count; i++) copyList[i].LineIndex = upgradeLinesIndex[i];

        //configure buy buttons and images tap behavior 
        foreach (UpgradeObject upgrade in copyList) {
            //the buy upgrade button located at a certain line of the scroll menu, will be linked to his upgrade in the upgrade list
            int upgradeIndex = copyList.FindIndex(obj => obj.Name.Contains(upgrade.Name));
            upgradeDataFields[upgrade.LineIndex].buyButton.onClick.AddListener(() => { BuyUpgrade(upgradeIndex, upgrade.Type); });

            void OpenDescription(BaseEventData eventData) {
                DescriptionCanvas.Instance.TriggerEnabled();
                DescriptionCanvas.Instance.SetDescription(upgrade.FullDescription);
            }
            EventTrigger.Entry pointDown = new EventTrigger.Entry() { eventID = EventTriggerType.PointerDown };
            pointDown.callback.AddListener(OpenDescription);
            upgradeDataFields[upgrade.LineIndex].upgradeImage.AddComponent<EventTrigger>().triggers.Add(pointDown);
        }
    }

    private void BuyUpgrade(int upgradeIndex, GlobalValues.UpgradeTypes upgradeType) { 
        UnlockUpdateUpgrades();
        switch (upgradeType) {
            case GlobalValues.UpgradeTypes.UnitConnected:
                Debug.Log("funziooouna " + unitConnectedUpgrades[upgradeIndex].Name);
                unitConnectedUpgrades[upgradeIndex].TimesBought++;
                UnlockUpdateUpgrades();
                if (purchasableUpgradesPerUnit[upgradeIndex] == 0) upgradeDataFields[unitConnectedUpgrades[upgradeIndex].LineIndex].upgradeLine.SetActive(false);
                Debug.Log("funziooouna " + unitConnectedUpgrades[upgradeIndex].TimesBought);
                break;

            case GlobalValues.UpgradeTypes.PollutionRelated:
                pollutionRelatedUpgrades[upgradeIndex].TimesBought++;
                upgradeDataFields[pollutionRelatedUpgrades[upgradeIndex].LineIndex].upgradeLine.SetActive(false);
                break;

            case GlobalValues.UpgradeTypes.Special:
                Debug.Log("funziooouna " + specialUpgrades[upgradeIndex].Name); 
                break;

        }
    }

    private void UnlockUpdateUpgrades() {
        //unit connected upgrades unlock conditions check
        //check which is the highest purchasable upgrade for each unit according to its level
        //and calculate which upgrades are still available to buy, considering the ones already bought 
        List<long> unitsLevels = UnitsManager.Instance.GetUnitsLevel();
        for (int i = 0; i < unitConnectedUpgrades.Count; i++) {
            int highestUpgr = 0;
            for (int j = 0; j < UNIT_UPGRADES_UNLOCK_LEVEL.Length; j++)
                if (unitsLevels[i] / UNIT_UPGRADES_UNLOCK_LEVEL[j] >= 1) 
                    highestUpgr++;

            purchasableUpgradesPerUnit[i] = highestUpgr - unitConnectedUpgrades[i].TimesBought;
            if (purchasableUpgradesPerUnit[i]>0) upgradeDataFields[unitConnectedUpgrades[i].LineIndex].upgradeLine.SetActive(true);
        }


        //pollution related upgrades unlock conditions check: if the upgrade hasn't been bought and the % of clanliness is met, activate its line
        double worldUpdatedPollution = WorldStatsManager.Instance.GetUpdatedPollution();
        for (int i = 0; i < pollutionRelatedUpgrades.Count; i++)
            if (pollutionRelatedUpgrades[i].TimesBought == 0 && worldUpdatedPollution < GlobalValues.BASE_POLLUTION - GlobalValues.BASE_POLLUTION * pollutionRelatedUpgrades[i].PollutionUnlockPercentage / 100)
                upgradeDataFields[pollutionRelatedUpgrades[i].LineIndex].upgradeLine.SetActive(true);
    }

    public List<bool> GetEnabledUpgradeLines() {
        List<bool> enabled = new List<bool>();
        //foreach (var upgradeLine in allUpgradeLines) enabled.Add(upgradeLine.activeSelf);
        return enabled;
    }
}

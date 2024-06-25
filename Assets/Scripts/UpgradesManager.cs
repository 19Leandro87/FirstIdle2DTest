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
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    public static UpgradesManager Instance { get; private set; }

    [SerializeField] private List<GameObject> upgradeLines;
    private List<GameObject> unitUpgradeLines, pollutionUpgradeLines, specialUpgradeLines;
    [SerializeField] private List<UpgradeLineObject> upgradeDataFields;
    private List<UpgradeObject> allUpgrades, unitConnectedUpgrades, pollutionRelatedUpgrades, specialUpgrades;
    private readonly long[] UNIT_UPGRADES_UNLOCK_LEVEL = { 10, 25, 50, 100, 200, 250, 500, 1000, 2500, 5000, 10000 }; 
    private float timer, upgradesUnlockCheck;


    private void Awake() {
        Instance = this;
        timer = 0;
        upgradesUnlockCheck = 3f;
        allUpgrades = new List<UpgradeObject>();
        unitConnectedUpgrades = new List<UpgradeObject>();
        pollutionRelatedUpgrades = new List<UpgradeObject>();
        specialUpgrades = new List<UpgradeObject>();
        unitUpgradeLines = new List<GameObject>();
        pollutionUpgradeLines = new List<GameObject>();
        specialUpgradeLines = new List<GameObject>();
    }

    private void Start() {
        for (int i = 0; i < GlobalValues.BASE_UPGRADES.Count; i++) {
            //create a copy of the BASE_UPGRADES to work with, without altering the actual BASE_UPGRADES
            allUpgrades.Add(new UpgradeObject());
            allUpgrades[i].Enabled = GlobalValues.BASE_UPGRADES[i].Enabled;
            allUpgrades[i].Type = GlobalValues.BASE_UPGRADES[i].Type;
            allUpgrades[i].Name = GlobalValues.BASE_UPGRADES[i].Name;
            allUpgrades[i].ConnectedUnitIndex = GlobalValues.BASE_UPGRADES[i].ConnectedUnitIndex;
            allUpgrades[i].PollutionUnlockPercentage = GlobalValues.BASE_UPGRADES[i].PollutionUnlockPercentage;
            allUpgrades[i].TimesBought = GlobalValues.BASE_UPGRADES[i].TimesBought;
            allUpgrades[i].Price = GlobalValues.BASE_UPGRADES[i].Price;
            allUpgrades[i].ShortDescription = GlobalValues.BASE_UPGRADES[i].ShortDescription;
            allUpgrades[i].FullDescription = GlobalValues.BASE_UPGRADES[i].FullDescription;

            //create a unit-related, a pollution-related and a special upgrades lists, connected to the allUpgrades list.
            switch (allUpgrades[i].Type) {
                case GlobalValues.UpgradeTypes.UnitConnected:
                    unitConnectedUpgrades.Add(allUpgrades[i]); break;
                case GlobalValues.UpgradeTypes.PollutionRelated:
                    pollutionRelatedUpgrades.Add(allUpgrades[i]); break;
                case GlobalValues.UpgradeTypes.Special:
                    specialUpgrades.Add(allUpgrades[i]); break;
            }

            //create a unit-related, a pollution-related and a special upgrades line list
            /*switch (upgradeLines[i].tag) {
                case GlobalValues.UpgradeTypes.UnitConnected.ToString():
                    unitConnectedUpgrades.Add(allUpgrades[i]); break;
                case GlobalValues.UpgradeTypes.PollutionRelated:
                    pollutionRelatedUpgrades.Add(allUpgrades[i]); break;
                case GlobalValues.UpgradeTypes.Special:
                    specialUpgrades.Add(allUpgrades[i]); break;
            }*/
            if (upgradeLines[i].tag == GlobalValues.UpgradeTypes.UnitConnected.ToString()) unitUpgradeLines.Add(upgradeLines[i]);
            if (upgradeLines[i].tag == GlobalValues.UpgradeTypes.PollutionRelated.ToString()) pollutionUpgradeLines.Add(upgradeLines[i]);
            if (upgradeLines[i].tag == GlobalValues.UpgradeTypes.Special.ToString()) specialUpgradeLines.Add(upgradeLines[i]);
        }
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer > upgradesUnlockCheck) {
            UpdateUpgrades();
            timer = 0;
        }
    }

    private void UpdateUpgrades() {
        //unit connected upgrades unlock conditions check
        List<long> unitsLevels = UnitsManager.Instance.GetUnitsLevel();
        for (int i = 0; i < unitConnectedUpgrades.Count; i++)
            if (UNIT_UPGRADES_UNLOCK_LEVEL.Contains(unitsLevels[i])) { }


        //pollution related upgrades unlock conditions check
        double worldUpdatedPollution = WorldStatsManager.Instance.GetUpdatedPollution();
        for (int i = 0; i < pollutionRelatedUpgrades.Count; i++)
            if (worldUpdatedPollution < GlobalValues.BASE_POLLUTION - GlobalValues.BASE_POLLUTION * pollutionRelatedUpgrades[i].PollutionUnlockPercentage / 100)
                pollutionUpgradeLines[i].SetActive(true);
    }

    public List<bool> GetEnabledUpgradeLines() {
        List<bool> enabled = new List<bool>();
        foreach (var upgradeLine in upgradeLines) enabled.Add(upgradeLine.activeSelf);
        return enabled;
    }
}

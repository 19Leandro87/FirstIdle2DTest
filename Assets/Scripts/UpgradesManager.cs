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
    [SerializeField] private List<UpgradeLineObject> upgradeDataFields;
    private List<UpgradeObject> allUpgrades, unitConnectedUpgrades, pollutionRelatedUpgrades, specialUpgrades;
    private readonly int[] UNIT_UPGRADES_UNLOCK_LEVEL = { 10, 25, 50, 100, 200, 250, 500, 1000, 2500, 5000, 10000 };
    private float timer, upgradesUnlockCheck;


    private void Awake() {
        Instance = this;
        timer = 0;
        upgradesUnlockCheck = 3f;
        allUpgrades = new List<UpgradeObject>();
        unitConnectedUpgrades = new List<UpgradeObject>();
        pollutionRelatedUpgrades = new List<UpgradeObject>();
        specialUpgrades = new List<UpgradeObject>();
    }

    private void Start() {
        //create a copy of the BASE_UPGRADES to work with, without altering the actual BASE_UPGRADES
        for (int i = 0; i < GlobalValues.BASE_UPGRADES.Count; i++) {
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

        //and create a unit-related, a pollution-related and a special upgrades lists, connected to the allUpgrades list.
            switch (allUpgrades[i].Type) {
                case GlobalValues.UpgradeTypes.UnitConnected:
                    unitConnectedUpgrades.Add(allUpgrades[i]); break;
                case GlobalValues.UpgradeTypes.PollutionRelated:
                    pollutionRelatedUpgrades.Add(allUpgrades[i]); break;
                case GlobalValues.UpgradeTypes.Special:
                    specialUpgrades.Add(allUpgrades[i]); break;
            }
        }
    }

    private void Update() {
        
    }

    private void UpdateUpgrades() { }

}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldStatsManager : MonoBehaviour {
    public static WorldStatsManager Instance {get; private set;}
    private float updatedPollution;
    private long money;
    [SerializeField] private TextMeshProUGUI pollutionText;
    [SerializeField] private TextMeshProUGUI cleanlinessText;
    [SerializeField] private TextMeshProUGUI moneyText;
    private SaveObject saveObject;
    private SaveObject loadedObject;
    private float saveInterval;
    private float timer;

    private void Awake() {
        Instance = this;

        SaveSystem.Init();
        saveObject = new SaveObject();

        //if there are no save games, updated pollution = base pollution and money = 0, else updated pollution and money = last saved data
        if (!SaveSystem.SaveGamesExist()) {
            updatedPollution = GlobalValues.BASE_POLLUTION;
            money = 0;
        }
        else {
            loadedObject = JsonUtility.FromJson<SaveObject>(SaveSystem.Load());
            updatedPollution = loadedObject.updatedPollution;
            money = loadedObject.updatedMoney;
        }

        timer = 0;
        saveInterval = 10f;
    }

    private void Start() {
        UpdateTexts(100 * updatedPollution / GlobalValues.BASE_POLLUTION);
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer > saveInterval) {
            timer = 0;
            saveObject.updatedPollution = updatedPollution;
            saveObject.updatedMoney = money;
            SaveStats();
        }
    }

    //money increases by the same amount of which pollution decreases with a 1:1 ratio
    public void UpdateWorldStats(float pollutionChange) {
        updatedPollution -= pollutionChange;
        UpdateMoney(Convert.ToInt64(pollutionChange));
        saveObject.updatedPollution = updatedPollution;
        saveObject.updatedMoney = money;
        UpdateTexts(100 * updatedPollution / GlobalValues.BASE_POLLUTION);
    }

    private void UpdateTexts(float pollution) {
        pollutionText.text = "Pollution: " + pollution.ToString("F2") + "%";
        cleanlinessText.text = "The World is " + (100 - pollution).ToString("F5") + "% clean";
        moneyText.text = "$ " + money.ToString();
    }

    private void UpdateMoney(long moneyChange) { money += moneyChange; }

    private void SaveStats() { SaveSystem.Save(JsonUtility.ToJson(saveObject)); }
}

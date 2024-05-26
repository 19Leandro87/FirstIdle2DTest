using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldStatsManager : MonoBehaviour {
    public static WorldStatsManager Instance {get; private set;}
    private float updatedPollution;
    [SerializeField] private TextMeshProUGUI pollutionText;
    [SerializeField] private TextMeshProUGUI cleanlinessText;
    private SaveObject saveObject;
    private float saveInterval;
    private float timer;

    private void Awake() {
        Instance = this;

        saveObject = new SaveObject();
        SaveSystem.Init();
        //if there are no save games, updated pollution = base pollution, else updated pollution = last saved pollution
        if (!SaveSystem.SaveGamesExist()) updatedPollution = GlobalValues.BASE_POLLUTION;
        else updatedPollution = JsonUtility.FromJson<SaveObject>(SaveSystem.Load()).updatedPollution;

        timer = 0;
        saveInterval = 3f;
    }

    private void Start() {
        UpdateTexts(100 * updatedPollution / GlobalValues.BASE_POLLUTION);
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer > saveInterval) {
            timer = 0;
            saveObject.updatedPollution = updatedPollution;
            SaveStats();
        }
    }

    public void UpdateWorldStats(float pollutionChange) {
        updatedPollution -= pollutionChange;
        saveObject.updatedPollution = updatedPollution;
        UpdateTexts(100 * updatedPollution / GlobalValues.BASE_POLLUTION);
    }

    private void UpdateTexts(float pollution) {
        pollutionText.text = "Pollution: " + pollution.ToString("F2") + "%";
        cleanlinessText.text = "The World is " + (100 - pollution).ToString("F5") + "% clean";
    }

    private void SaveStats() { SaveSystem.Save(JsonUtility.ToJson(saveObject)); }
}

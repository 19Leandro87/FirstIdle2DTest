using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldStatsManager : MonoBehaviour {
    public static WorldStatsManager Instance {get; private set;}

    private float initialPollution;
    private float updatedPollution = 0;
    [SerializeField] private TextMeshProUGUI pollutionText;
    [SerializeField] private TextMeshProUGUI cleanlinessText;

    //test value just for debugging purposes
    public float testValue = 0;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        //TO BE UPDATED
        initialPollution = GlobalValues.BASE_POLLUTION - updatedPollution;
        UpdateWorldStats(testValue);
    }

    public void UpdateWorldStats(float pollution) {
        testValue++;
        float newPollution = 100 * (initialPollution - pollution) / GlobalValues.BASE_POLLUTION;
        pollutionText.text = "Pollution: " + newPollution.ToString("F3") + "%";
        cleanlinessText.text = "The World is " + (100 - newPollution).ToString("F6") + "% clean";
    }

    private void SetUpdatedPollution(float newPollution) { updatedPollution = newPollution; }
}


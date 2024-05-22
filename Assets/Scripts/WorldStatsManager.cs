using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldStatsManager : MonoBehaviour {
    public static WorldStatsManager Instance {get; private set;}
    private float updatedPollution;
    [SerializeField] private TextMeshProUGUI pollutionText;
    [SerializeField] private TextMeshProUGUI cleanlinessText;

    private void Awake() {
        Instance = this;
        updatedPollution = GlobalValues.BASE_POLLUTION; //<<< gonna be = a saved value
    }

    private void Start() {
        UpdateTexts(100 * updatedPollution / GlobalValues.BASE_POLLUTION);
    }

    public void UpdateWorldStats(float pollutionChange) {
        updatedPollution -= pollutionChange;
        UpdateTexts(100 * updatedPollution / GlobalValues.BASE_POLLUTION);
    }

    private void UpdateTexts(float pollution) {
        pollutionText.text = "Pollution: " + pollution.ToString("F2") + "%";
        cleanlinessText.text = "The World is " + (100 - pollution).ToString("F5") + "% clean";
    }
}


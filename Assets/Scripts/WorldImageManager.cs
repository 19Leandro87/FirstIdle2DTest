using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldImageManager : MonoBehaviour, IPointerDownHandler
{
    public static WorldImageManager Instance { get; private set; }
    private float clickValue;

    private void Awake() {
        Instance = this;
        clickValue = 1;
    }

    public void OnPointerDown(PointerEventData eventData) {
        WorldStatsManager.Instance.UpdateWorldStats(clickValue);
    }

    private void SetClickValue(float newClickValue) { clickValue = newClickValue; }
}

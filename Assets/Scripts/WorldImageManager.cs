using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldImageManager : MonoBehaviour, IPointerDownHandler
{
    public static WorldImageManager Instance { get; private set; }
    private long clickValue;

    private void Awake() {
        Instance = this;
        clickValue = 250;
    }

    public void OnPointerDown(PointerEventData eventData) {
        WorldStatsManager.Instance.UpdateWorldStats(clickValue);
    }

    private void SetClickValue(long newClickValue) { clickValue = newClickValue; }

    private long GetClickValue() { return clickValue; }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldImageManager : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log(this.gameObject.name + " Was Clicked.");
        WorldStatsManager.Instance.UpdateWorldStats(WorldStatsManager.Instance.testValue);
    }
}

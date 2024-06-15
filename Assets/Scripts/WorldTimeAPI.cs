using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WorldTimeAPI : MonoBehaviour
{
    public static WorldTimeAPI Instance { get; private set; }

    const string API_URL = "https://worldtimeapi.org/api/ip";
    private int unix_time_now = 0;
    struct TimeData { public string unixtime; }

    IEnumerator GetRealTimeFromAPI() { 
        UnityWebRequest request = UnityWebRequest.Get(API_URL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError) 
            Debug.Log("CAN'T GET TIME FROM SERVER");
        else {
            TimeData timeData = JsonUtility.FromJson<TimeData>(request.downloadHandler.text);
            Int32.TryParse(timeData.unixtime, out unix_time_now);
        }
    }

    private void Awake() {
        if (Instance == null) { 
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else 
            Destroy(this.gameObject);
    }

    private void Start() { StartCoroutine(GetRealTimeFromAPI()); }

    public int GetRealTime() {
        StartCoroutine(GetRealTimeFromAPI());
        return unix_time_now;
    }
}

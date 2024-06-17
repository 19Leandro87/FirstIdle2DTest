using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WorldTimeAPI : MonoBehaviour
{
    public static WorldTimeAPI Instance { get; private set; }

    const string API_URL = "https://worldtimeapi.org/api/ip";
    private long unixTimeNow = 0;
    struct TimeData { public string unixtime; }

    public IEnumerator GetRealTimeFromAPI() { 
        UnityWebRequest request = UnityWebRequest.Get(API_URL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError) 
            Debug.Log("CAN'T GET TIME FROM SERVER");
        else {
            TimeData timeData = JsonUtility.FromJson<TimeData>(request.downloadHandler.text);
            Int64.TryParse(timeData.unixtime, out unixTimeNow);
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

    public long GetRealTime() {
        StartCoroutine(GetRealTimeFromAPI());
        return unixTimeNow;
    }
}

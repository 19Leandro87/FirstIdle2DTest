using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;
    private void Start() { StartCoroutine(LoadMain()); }

    IEnumerator LoadMain() {
        //Initialize World Time and Save System. If there's a saved game, calculate the elasped time since last game, if it's possible to retrieve the time from server start the game
        yield return WorldTimeAPI.Instance.GetRealTimeFromAPI();
        SaveSystem.Init();
        GlobalValues.timeOnStart = WorldTimeAPI.Instance.GetRealTime();
        if (SaveSystem.SaveGamesExist())
            GlobalValues.timeSinceLast = GlobalValues.timeOnStart - JsonUtility.FromJson<SaveObject>(SaveSystem.Load()).realWorldTime;
        if (GlobalValues.timeOnStart != 0)
            SceneManager.LoadScene("MainScene");
        else
            loadingText.text = "Connection failed.\n Check your internet connection and restart the game.";
    }
}

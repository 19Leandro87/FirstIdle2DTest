using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public static class SaveSystem {

    private static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
    private const string SAVE_EXTENSION = "txt";

    public static void Init() {
        // if Save Folder doesn't exist, create it
        if (!Directory.Exists(SAVE_FOLDER)) Directory.CreateDirectory(SAVE_FOLDER);
    }

    public static void Save(string saveString) {
        // Make sure the Save Number is unique so it doesnt overwrite a previous save file
        int saveNumber = 1;
        int numberOfSavesToKeep = 3;

        while (File.Exists(SAVE_FOLDER + "save_" + saveNumber + "." + SAVE_EXTENSION)) saveNumber++;
        // saveNumber is unique
        File.WriteAllText(SAVE_FOLDER + "save_" + saveNumber + "." + SAVE_EXTENSION, saveString);
        
        //leave only the last given number of save games
        /*if (File.Exists(SAVE_FOLDER + "save_" + (saveNumber - numberOfSavesToKeep) + "." + SAVE_EXTENSION)) {
            File.Delete(SAVE_FOLDER + "save_" + (saveNumber - numberOfSavesToKeep) + "." + SAVE_EXTENSION);
            File.Delete(SAVE_FOLDER + "save_" + (saveNumber - numberOfSavesToKeep) + "." + SAVE_EXTENSION + ".meta");
        } */

    }

    //check if there's at least one saved game
    public static bool SaveGamesExist() { return new DirectoryInfo(SAVE_FOLDER).GetFiles("*." + SAVE_EXTENSION).Length > 0; }

    public static string Load() {
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        // Get all save files
        FileInfo[] saveFiles = directoryInfo.GetFiles("*." + SAVE_EXTENSION);
        // Cycle through all save files and identify the most recent one
        FileInfo mostRecentFile = null;
        foreach (FileInfo fileInfo in saveFiles) {
            if (mostRecentFile == null) mostRecentFile = fileInfo;
            else {
                if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime) mostRecentFile = fileInfo;
            }
        }
        // If theres a save file, load it, if not return null
        if (mostRecentFile != null) return File.ReadAllText(mostRecentFile.FullName);
        else return null;
    }
}
/*
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameHandler : MonoBehaviour {

    [SerializeField] private GameObject unitGameObject;
    private IUnit unit;

    private void Awake() {
        unit = unitGameObject.GetComponent<IUnit>();
        SaveSystem.Init();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            Load();
        }
    }

    private void Save() {
        // Save
        Vector3 playerPosition = unit.GetPosition();
        int goldAmount = unit.GetGoldAmount();

        SaveObject saveObject = new SaveObject {
            goldAmount = goldAmount,
            playerPosition = playerPosition
        };
        string json = JsonUtility.ToJson(saveObject);
        SaveSystem.Save(json);

        //CMDebug.TextPopupMouse("Saved!");
    }

    private void Load() {
        // Load
        string saveString = SaveSystem.Load();
        if (saveString != null) {
            //CMDebug.TextPopupMouse("Loaded: " + saveString);

            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

            unit.SetPosition(saveObject.playerPosition);
            unit.SetGoldAmount(saveObject.goldAmount);
        }
        else {
            //CMDebug.TextPopupMouse("No save");
        }
    }


    private class SaveObject {
        public int goldAmount;
        public Vector3 playerPosition;
    }
}
*/
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System;

public static class SaveSystem {

    private static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
    private const string SAVE_EXTENSION = "txt";
    private static string SAVE_INDEX_FILE = SAVE_FOLDER + "savesIndex." + SAVE_EXTENSION;

    public static void Init() {
        // if Save Folder doesn't exist, create it
        if (!Directory.Exists(SAVE_FOLDER)) Directory.CreateDirectory(SAVE_FOLDER);
        // if saves index file doesn't exist create it and set it to 1
        if (!File.Exists(SAVE_INDEX_FILE)) File.WriteAllText(SAVE_INDEX_FILE, "1");
    }

    public static void Save(string saveString) {
        // Initialize save number, paste the actual save number converting it to int from string, create indexed save and increase index for next save
        int saveNumber = 1;
        Int32.TryParse(File.ReadAllText(SAVE_INDEX_FILE), out saveNumber);
        File.WriteAllText(SAVE_INDEX_FILE, (saveNumber + 1).ToString());
        File.WriteAllText(SAVE_FOLDER + "save_" + saveNumber + "." + SAVE_EXTENSION, saveString);

        
        // Define the number of save games to keep, then delete the n-1th + the corresponding meta
        int numberOfSavesToKeep = 3;
        if (saveNumber > numberOfSavesToKeep) {
            if (File.Exists(SAVE_FOLDER + "save_" + (saveNumber - numberOfSavesToKeep) + "." + SAVE_EXTENSION))
                File.Delete(SAVE_FOLDER + "save_" + (saveNumber - numberOfSavesToKeep) + "." + SAVE_EXTENSION);

            if (File.Exists(SAVE_FOLDER + "save_" + (saveNumber - numberOfSavesToKeep) + "." + SAVE_EXTENSION + ".meta"))
                File.Delete(SAVE_FOLDER + "save_" + (saveNumber - numberOfSavesToKeep) + "." + SAVE_EXTENSION + ".meta");
        }
    }

    //check if there's at least one saved game, considering a file is the index file
    public static bool SaveGamesExist() { return new DirectoryInfo(SAVE_FOLDER).GetFiles("*." + SAVE_EXTENSION).Length > 1; }

    public static string Load() {
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        // Get all save files
        FileInfo[] saveFiles = directoryInfo.GetFiles("save_*." + SAVE_EXTENSION);
        // Cycle through all save files and identify the most recent one
        FileInfo mostRecentFile = null;
        foreach (FileInfo fileInfo in saveFiles) {
            if (mostRecentFile == null) mostRecentFile = fileInfo;
            else {
                if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime) mostRecentFile = fileInfo;
            }
        }
        // If there's a save file, load it, if not return null
        if (mostRecentFile != null) return File.ReadAllText(mostRecentFile.FullName);
        else return null;
    }
}
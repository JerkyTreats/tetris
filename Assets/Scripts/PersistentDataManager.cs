using ProtoBuf;
using System.IO;
using System;
using UnityEngine;

public class PersistentDataManager {
    public static string SAVE_FILE_TYPE = ".dat";
    public static string SAVE_FILE_NAME = "save";
    private string saveFile;
    private string saveFileName;

    public PersistentDataManager() {
        this.saveFile = Path.Combine(Application.persistentDataPath, GetSaveFileName());
    }

    public string GetSaveFileName() {
        // 1
        int highestSaveNum = GetHighestSaveFileNum();

        // 001
        string nextSaveNum = (highestSaveNum + 1).ToString("D3");

        // save001.dat
        string saveFileName = SAVE_FILE_TYPE + nextSaveNum + SAVE_FILE_TYPE;

        return saveFileName;
    }

    private int GetHighestSaveFileNum() {
        int highestSaveNum = 0;

        string[] files = Directory.GetFiles(Application.persistentDataPath);

        foreach(string fileName in files) {
            // [save, 001.dat]
            string[] saveNameArr = fileName.Split(new string[] { SAVE_FILE_NAME }, StringSplitOptions.None);
            // [001, .dat]
            string[] saveFileNumArr = saveNameArr[1].Split(new string[] { SAVE_FILE_TYPE }, StringSplitOptions.None);
            int saveFileNum = Int32.Parse(saveFileNumArr[1]); // to int

            if (saveFileNum > highestSaveNum )
                highestSaveNum = saveFileNum;
        }

        return highestSaveNum;
    }

    private void OnApplicationQuit() {
        // ...
    }



    private void LoadGame() {

    }



    private void SaveGame() {

    }
}

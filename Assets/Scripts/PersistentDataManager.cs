using ProtoBuf;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PersistentDataManager {
    public static string SAVE_FILE_TYPE = ".dat";
    public static string SAVE_FILE_NAME = "save";
    private string saveFile;
    private string saveFileName;

    private string saveDir = Application.persistentDataPath;

    public PersistentDataManager() {
        this.saveFile = Path.Combine(saveDir, GetNewSaveFileName());
    }

    public string GetNewSaveFileName() {
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

            // 1
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



    private void SaveBoard(Board board) {
        string fileName = GetNewSaveFileName();

        using var file = File.OpenWrite(fileName);

        var data = new BoardData
        {
            spawnPosition = board.spawnPosition,
            boardSize = board.boardSize,
            tiles = board.placedBlocks
        };

        Serializer.Serialize(file, data);
    }
}

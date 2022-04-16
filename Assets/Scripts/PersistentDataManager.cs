using ProtoBuf;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;

// SUPA WIP 
public class PersistentDataManager {
    public static string saveFileType = ".dat";
    public static string SAVE_FILE_NAME = "save";
    private string _saveFile;
    private string _saveFileName;

    private string _saveDir = Application.persistentDataPath;

    public PersistentDataManager() {
        _saveFile = Path.Combine(_saveDir, GetNewSaveFileName());
    }

    public string GetNewSaveFileName() {
        // 1
        var highestSaveNum = GetHighestSaveFileNum(); 

        // 001
        var nextSaveNum = (highestSaveNum + 1).ToString("D3");

        // save001.dat
        var saveFileName = saveFileType + nextSaveNum + saveFileType;

        return saveFileName;
    }

    private int GetHighestSaveFileNum() {
        var highestSaveNum = 0;

        var files = Directory.GetFiles(Application.persistentDataPath);

        foreach(var fileName in files) {
            // [save, 001.dat]
            var saveNameArr = fileName.Split(new string[] { SAVE_FILE_NAME }, StringSplitOptions.None);
            // [001, .dat]
            var saveFileNumArr = saveNameArr[1].Split(new string[] { saveFileType }, StringSplitOptions.None);

            // 1
            var saveFileNum = Int32.Parse(saveFileNumArr[1]); // to int

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



    // private void SaveBoard(Board board) {
    //     var fileName = GetNewSaveFileName();
    //
    //     using var file = File.OpenWrite(fileName);
    //
    //     var data = new BoardData
    //     {
    //         spawnPosition = board.spawnPosition,
    //         boardSize = board.boardSize,
    //         // tiles = board.placedBlocks
    //     };
    //
    //     Serializer.Serialize(file, data);
    // }
}

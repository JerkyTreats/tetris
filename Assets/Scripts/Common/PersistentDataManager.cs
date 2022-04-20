using System;
using System.IO;
using Board.Persistence;
using ProtoBuf;
using UnityEngine;

// SUPA WIP 
namespace Common
{
    public class PersistentDataManager {
        private static readonly string SaveFileType;
        private static readonly string SaveFileName;
        private readonly string _saveFile;
        private string _saveFileName;

        private readonly string _saveDir = Application.persistentDataPath;

        public PersistentDataManager()
        {
            _saveFile = Path.Combine(_saveDir, GetNewSaveFileName());
        }

        static PersistentDataManager()
        {
            SaveFileType = ".dat";
            SaveFileName = "save";
        }

        private static string GetNewSaveFileName() {
            // 1
            var highestSaveNum = GetHighestSaveFileNum(); 

            // 001
            var nextSaveNum = (highestSaveNum + 1).ToString("D3");

            // save001.dat
            var saveFileName = SaveFileName + nextSaveNum + SaveFileType;

            return saveFileName;
        }

        private static int GetHighestSaveFileNum() {
            var highestSaveNum = 0;

            var files = Directory.GetFiles(Application.persistentDataPath);

            foreach(var filePath in files)
            {

                var fileName = Path.GetFileName(filePath);
                // [save, 001.dat]
                var saveNameArr = fileName.Split(new string[] { SaveFileName }, StringSplitOptions.None);
                // [001, .dat]
                var saveFileNumArr = saveNameArr[1].Split(new string[] { SaveFileType }, StringSplitOptions.None);

                // 1
                var saveFileNum = Int32.Parse(saveFileNumArr[0]); // to int

                if (saveFileNum > highestSaveNum )
                    highestSaveNum = saveFileNum;
            }

            return highestSaveNum;
        }
        
        public void SaveBoard(BoardData boardData)
        {
            using var file = File.OpenWrite(_saveFile);
            Serializer.Serialize(file, boardData);
            file.Close();
        }
    }
}

using System;
using System.IO;
using System.IO.Compression;
using ProtoBuf;
using UnityEngine;

namespace Persistence
{
    /// <summary>
    /// Provides CRUD operations from the local file system
    /// </summary>
    public class LocalFileDataContext {
        private static readonly string SaveFileType;
        private static readonly string SaveFileName;
        private readonly string _saveFile;
        private string _saveFileName;

        private readonly string _saveDir = Application.persistentDataPath;

        /// <summary>
        /// Create a new LocalFileDataContext 
        /// </summary>
        public LocalFileDataContext()
        {
            _saveFile = Path.Combine(_saveDir, GetNewSaveFileName());
        }

        static LocalFileDataContext()
        {
            SaveFileType = ".dat";
            SaveFileName = "save";
        }

        /// <summary>
        /// Serialize a generic object to file on disk
        /// </summary>
        /// <param name="toSave"></param>
        /// <typeparam name="T"></typeparam>
        public void Save<T>(T toSave)
        {
            using var file = File.OpenWrite(_saveFile);
            using var gzip = new GZipStream(file, CompressionMode.Compress);

            Serializer.SerializeWithLengthPrefix(gzip, toSave, PrefixStyle.Fixed32BigEndian);
        }

        /// <summary>
        /// Deserialize a generic object and return
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Load<T>()
        {
            var latestFile = GetHighestSaveFileNum(); 
            var fileName = $"{SaveFileName}{latestFile:D3}{SaveFileType}";
            var filePath = Path.Combine(_saveDir, fileName);
            
            using var file = File.OpenRead(filePath);
            using var gzip = new GZipStream(file, CompressionMode.Decompress);

            var deserializedObject = Serializer.DeserializeWithLengthPrefix<T>(gzip, PrefixStyle.Fixed32BigEndian);
            return deserializedObject;
        }
        
        // Generate a new file name in format {NAME}(001 + 1){FILETYPE}
        private static string GetNewSaveFileName() {
            // 1
            var highestSaveNum = GetHighestSaveFileNum(); 

            // 001
            var nextSaveNum = (highestSaveNum + 1).ToString("D3");

            // save001.dat
            var saveFileName = SaveFileName + nextSaveNum + SaveFileType;

            return saveFileName;
        }

        // Find the highest int number given filename format {NAME}001{FILETYPE}
        private static int GetHighestSaveFileNum() {
            var files = Directory.GetFiles(Application.persistentDataPath);

            if (files.Length == 0) return 0;
            
            var highestSaveNum = 0;
            
            foreach(var filePath in files)
            {
                var fileName = Path.GetFileName(filePath);
                if (!fileName.Contains(SaveFileName)) continue;
                
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
    }
}

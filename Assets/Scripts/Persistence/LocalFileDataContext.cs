using System;
using System.Collections.Generic;
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
        public const string SaveFileType = ".dat";
        public const string SaveFileName = "save";
        private readonly string _saveFile;
        private string _saveFileName;

        public readonly string SaveDir = Application.persistentDataPath;

        /// <summary>
        /// Create a new LocalFileDataContext 
        /// </summary>
        public LocalFileDataContext()
        {
            _saveFile = Path.Combine(SaveDir, GetNewSaveFileName());
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
        public T Load<T>(string fileName)
        {
            var path = Path.Combine(SaveDir, fileName);

            return DeserializedObject<T>(path);
        }

        private static T DeserializedObject<T>(string filePath)
        {
            using var file = File.OpenRead(filePath);
            using var gzip = new GZipStream(file, CompressionMode.Decompress);

            var deserializedObject = Serializer.DeserializeWithLengthPrefix<T>(gzip, PrefixStyle.Fixed32BigEndian);
            return deserializedObject;
        }

        // Generate a new file name in format {NAME}(001 + 1){FILETYPE}
        private string GetNewSaveFileName() {
            // 1
            var highestSaveNum = GetHighestSaveFileNum(); 

            // 001
            var nextSaveNum = (highestSaveNum + 1).ToString("D3");

            // save001.dat
            var saveFileName = SaveFileName + nextSaveNum + SaveFileType;

            return saveFileName;
        }

        // Find the highest int number given filename format {NAME}001{FILETYPE}
        private int GetHighestSaveFileNum() {
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

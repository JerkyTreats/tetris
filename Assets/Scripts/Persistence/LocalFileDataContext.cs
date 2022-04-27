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

        public readonly string saveDir = Application.persistentDataPath;

        /// <summary>
        /// Serialize and save generic object to unity default persistent data path
        /// </summary>
        /// <param name="toSave">Object to save</param>
        /// <param name="fileName">string name of file</param>
        /// <typeparam name="T">Protobuf serializable typeof object</typeparam>
        public void Save<T>(T toSave, string fileName)
        {
            var path = Path.Combine(saveDir, fileName);
            using var file = File.OpenWrite(path);
            using var gzip = new GZipStream(file, CompressionMode.Compress);

            Serializer.SerializeWithLengthPrefix(gzip, toSave, PrefixStyle.Fixed32BigEndian);
        }

        /// <summary>
        /// Deserialize and return a generic object
        /// </summary>
        /// <typeparam name="T">Protobuf serializable typeof object</typeparam>
        /// <returns>Deserialized object</returns>
        public T Load<T>(string fileName)
        {
            var path = Path.Combine(saveDir, fileName);

            return DeserializeObject<T>(path);
        }

        private static T DeserializeObject<T>(string filePath)
        {
            using var file = File.OpenRead(filePath);
            using var gzip = new GZipStream(file, CompressionMode.Decompress);

            var deserializedObject = Serializer.DeserializeWithLengthPrefix<T>(gzip, PrefixStyle.Fixed32BigEndian);
            return deserializedObject;
        }
    }
}

using Board.Persistence;
using ProtoBuf.Meta;
using UnityEngine;

namespace Initialization
{
    [CreateAssetMenu(fileName = "ProtobufInit", menuName = "ScriptableObjects/ProtobufInit", order = 0)]
    public class ProtobufInitialization : ScriptableObject
    {
        /// <summary>
        /// Intended to run once, and only once, when the game is first started. 
        /// From https://blog.oliverbooth.dev/2021/04/27/writing-a-portable-save-load-system/
        /// Protobuf doesn't understand UnityEngine.Vector3Int (or any Unity type).
        /// Therefore we've created a surrogate type. Here we tell protobuf to use the surrogate in place of the Unity type
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        private static void ProtoBufSerializationSetup() {
            var model = RuntimeTypeModel.Default;
            model.Add<Vector3IntSurrogate>();
            model.Add<Vector3Int>(false).SetSurrogate(typeof(Vector3IntSurrogate));
            
            model.Add<Vector2IntSurrogate>();
            model.Add<Vector2Int>(false).SetSurrogate(typeof(Vector2IntSurrogate));
        }
    }
}
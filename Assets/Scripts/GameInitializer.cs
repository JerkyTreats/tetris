using UnityEngine;
using ProtoBuf.Meta;

/// <summary>
/// Intended to run once, and only once, when the game is first started. Really really Jesus only once.
/// </summary>
public class GameInitializer : MonoBehaviour
{
    void Awake () {
        ProtoBufSerializationSetup();
    }

    /// <summary>
    /// From https://blog.oliverbooth.dev/2021/04/27/writing-a-portable-save-load-system/
    /// <para>
    /// Protobuf doesn't understand UnityEngine.Vector3Int (or any Unity type).
    /// Therefore we've created a surrogate type. Here we tell protobuf to use the surrogate in place of the Unity type
    /// </summary>
    void ProtoBufSerializationSetup() {
        var model = RuntimeTypeModel.Default;
        model.Add<Vector3IntSurrogate>();
        model.Add<Vector3>(false).SetSurrogate(typeof(Vector3IntSurrogate));
    }
}

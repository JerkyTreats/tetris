using UnityEngine;
using UnityEngine.Tilemaps;
using ProtoBuf.Meta;

/// <summary>
/// Holds Game and Initialization data for the game.
/// Tiles/Data objects are populated in editor, passed through to game objects.
/// </summary>
public class GameData : MonoBehaviour
{
    public Tile ghostTile;
    public Tile gridTile;
    public TetrominoData[] tetrominos;

    public BorderPieceData[] borderPieces;

    void Awake () {
        ProtoBufSerializationSetup();
    }

    /// <summary>
    /// Intended to run once, and only once, when the game is first started.
    /// From https://blog.oliverbooth.dev/2021/04/27/writing-a-portable-save-load-system/
    /// <para>
    /// Protobuf doesn't understand UnityEngine.Vector3Int (or any Unity type).
    /// Therefore we've created a surrogate type. Here we tell protobuf to use the surrogate in place of the Unity type
    /// </summary>
    private void ProtoBufSerializationSetup() {
        var model = RuntimeTypeModel.Default;
        model.Add<Vector3IntSurrogate>();
        model.Add<Vector3>(false).SetSurrogate(typeof(Vector3IntSurrogate));
    }

}

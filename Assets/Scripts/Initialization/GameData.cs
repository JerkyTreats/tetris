using System;
using Board;
using Board.Persistence;
using ProtoBuf.Meta;
using Tetris;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Initialization
{
    /// <summary>
    /// Holds Game and Initialization data for the game.
    /// Tiles/Data objects are populated in editor, passed through to game objects.
    /// If I knew how to define an asset in code, a lot of this goes away
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameData", order = 1)]
    public class GameData : ScriptableObject
    {
        public Tile ghostTile;
        public Tile gridTile;
        public TetrominoData[] tetrominos;
        public BorderPieceData[] borderPieces;
        public BlockData[] blocks;

        void Awake () {
            ProtoBufSerializationSetup();
        }

        /// <summary>
        /// Intended to run once, and only once, when the game is first started. 
        /// From https://blog.oliverbooth.dev/2021/04/27/writing-a-portable-save-load-system/
        /// Protobuf doesn't understand UnityEngine.Vector3Int (or any Unity type).
        /// Therefore we've created a surrogate type. Here we tell protobuf to use the surrogate in place of the Unity type
        /// </summary>
        private static void ProtoBufSerializationSetup() {
            var model = RuntimeTypeModel.Default;
            model.Add<Vector3IntSurrogate>();
            model.Add<Vector3Int>(false).SetSurrogate(typeof(Vector3IntSurrogate));
            
            model.Add<Vector2IntSurrogate>();
            model.Add<Vector2Int>(false).SetSurrogate(typeof(Vector2IntSurrogate));
        }
        
        // This is a gross hack that I hate 
        public Block GetBlockFromTile(Tile tile)
        {
            foreach (var blockData in blocks)
            {
                if (tile == blockData.tile)
                    return blockData.block;
            }

            throw new ApplicationException($"Could not find block enum for item { tile.ToString() }" );
        }

        public TileBase GetTileFromBlock(Block block)
        {
            foreach (var blockData in blocks)
            {
                if (block == blockData.block)
                    return blockData.tile;
            }

            throw new ApplicationException($"Could not find block enum for item { block.ToString() }" );
        }
    }
}

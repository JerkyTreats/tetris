using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Board.Persistence
{
    [ProtoContract]
    public struct BoardData {

        [ProtoMember(2)] public Vector3Int cameraPosition;
        [ProtoMember(3)] public Vector3Int boardPosition;
        [ProtoMember(4)] public Vector2Int boardSize;
        [ProtoMember(5)] public int sortOrder;
        [ProtoMember(6)] public List<BoardTileData> tiles;
        [ProtoMember(7)] public string userSavedBoardName;

        // TODO BoardData : Objects data scope too wide
        // userSavedBoardName should likely be contained in a BoardDataSaveData object
        // But I'm also cramming userSavedBoardName in here for now because it's my prototype and this is the corner I'm cutting today
        
        /// <summary>
        /// BoardData Constructor
        /// </summary>
        /// <param name="cameraPosition">BoardCamera Position</param>
        /// <param name="boardPosition">Board Position in World Space</param>
        /// <param name="boardSize">Width/Length of the Board</param>
        /// <param name="sortOrder">Unity 2D sort order</param>
        /// <param name="tiles">Tiles placed on the Board</param>
        /// <param name="userSavedBoardName">Name provided by user for a saved board</param>
        public BoardData(Vector3Int cameraPosition, Vector3Int boardPosition, Vector2Int boardSize, int sortOrder = 0, List<BoardTileData> tiles = null, string userSavedBoardName = null)
        {
            this.cameraPosition  = cameraPosition;
            this.boardPosition = boardPosition;
            this.boardSize = boardSize;
            this.sortOrder = sortOrder;

            this.tiles = tiles ?? new List<BoardTileData>();
            this.userSavedBoardName = userSavedBoardName ?? "";
        }
    }

    [ProtoContract]
    public struct BoardTileData {
        [ProtoMember(1)]
        public Block block;
        [ProtoMember(2)]
        public Vector3Int position;
    }

    [ProtoContract]
    public enum Block {
        [ProtoMember(0)]
        Blue,
        [ProtoMember(1)]
        Cyan,
        [ProtoMember(2)]
        Ghost,
        [ProtoMember(3)]
        Green,
        [ProtoMember(4)]
        Orange,
        [ProtoMember(5)]
        Purple,
        [ProtoMember(6)]
        Red,
        [ProtoMember(7)]
        Yellow,
    }

    [ProtoContract]
    public struct Vector3IntSurrogate
    {
        [ProtoMember(1)]
        public int X { get; set; }

        [ProtoMember(2)]
        public int Y { get; set; }

        [ProtoMember(3)]
        public int Z { get; set; }

        public static implicit operator Vector3Int(Vector3IntSurrogate vector) =>
            new Vector3Int(vector.X, vector.Y, vector.Z);

        public static implicit operator Vector3IntSurrogate(Vector3Int vector) =>
            new Vector3IntSurrogate { X = vector.x, Y = vector.y, Z = vector.z };
    }
    
    [ProtoContract]
    public struct Vector2IntSurrogate
    {
        [ProtoMember(1)]
        public int X { get; set; }

        [ProtoMember(2)]
        public int Y { get; set; }


        public static implicit operator Vector2Int(Vector2IntSurrogate vector) =>
            new Vector2Int(vector.X, vector.Y);

        public static implicit operator Vector2IntSurrogate(Vector2Int vector) =>
            new Vector2IntSurrogate { X = vector.x, Y = vector.y };
    }
}
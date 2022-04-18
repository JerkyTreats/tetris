using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Board
{
    [ProtoContract]
    public struct BoardData {

        [ProtoMember(1)]
        public Vector3Int spawnPosition;
        [ProtoMember(2)]
        public Vector3Int cameraPosition;
        [ProtoMember(3)]
        public Vector3Int boardPosition;
        [ProtoMember(4)]
        public Vector2Int boardSize;
        [ProtoMember(5)]
        public int sortOrder;
        [ProtoMember(6)]
        public List<BoardTileData> tiles;
    }

    [ProtoContract]
    public struct BoardTileData {
        public Block block;
        public Vector3Int position;
    }

    [ProtoContract]
    public enum Block {
        Blue,
        Cyan,
        Ghost,
        Green,
        Orange,
        Purple,
        Red,
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
}
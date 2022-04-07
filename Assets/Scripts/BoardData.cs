using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public struct BoardData {
    public Vector3Int spawnPosition;
    public Vector2Int boardSize;
    public List<Tile> tiles;
}

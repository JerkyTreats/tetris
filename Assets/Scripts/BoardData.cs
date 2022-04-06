using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[System.Serializable]
public struct BoardData {
    public TetrominoData[] tetrominos;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize;
    public List<PlacedPiece> placedPieces;
}

[System.Serializable]
public struct PlacedPiece {
    public TetrominoData tetromino;
    public Vector3Int position;
    // public BoardData boardData;
}

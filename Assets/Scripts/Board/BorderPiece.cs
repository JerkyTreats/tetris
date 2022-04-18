using UnityEngine;
using UnityEngine.Tilemaps;

namespace Board
{
    public enum BorderPieceType {
        Top,
        Right,
        Left,
        Bottom,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
    }

    [System.Serializable]
    public struct BorderPieceData {
        public BorderPieceType borderPiece;
        public Tile tile;
    }
}
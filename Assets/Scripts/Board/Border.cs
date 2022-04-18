using Common;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Board
{
    public class Border : MonoBehaviour
    {
        public Board board;
        public BorderPieceData[] borderPieces;
        public Vector3 thisPosition;
        private Tilemap Tilemap { get; set; }

        /// <summary>
        /// Create the Border GameObject and initialize
        /// </summary>
        public static Border CreateBoardBorder(Board board)
        {
            var borderObject = TileGameObjectFactory.CreateNewTileObject("Border", Vector3Int.zero, 0, board.transform);
        
            var border = borderObject.AddComponent<Border>();
            border.board = board;
            border.borderPieces = board.GameData.borderPieces;
            border.DrawBorder();

            return border;
        }

        private void Awake() {
            Tilemap = GetComponentInChildren<Tilemap>();
        }

        private void DrawBorder() {
            var bounds = board.WorldBounds;

            // Draw top/bottom border
            for (var col = bounds.xMin; col < bounds.xMax; col++) {
                var top = new Vector3Int(col, (bounds.yMax), 0);
                var bottom = new Vector3Int(col, (bounds.yMin - 1), 0);
                Set(top, BorderPieceType.Top);
                Set(bottom, BorderPieceType.Bottom);
            }

            // Draw sides border
            for (var row = bounds.yMin; row < bounds.yMax; row++) {
                var left = new Vector3Int((bounds.xMin - 1), row, 0);
                var right = new Vector3Int((bounds.xMax), row, 0);
                Set(left, BorderPieceType.Left);
                Set(right, BorderPieceType.Right);
            }

            DrawCorners(bounds);
        }

        private void DrawCorners(RectInt bounds) {
            var topLeft = new Vector3Int(bounds.xMin - 1, bounds.yMax, 0);
            var topRight = new Vector3Int(bounds.xMax, bounds.yMax, 0);
            var bottomLeft = new Vector3Int(bounds.xMin - 1, bounds.yMin - 1, 0);
            var bottomRight = new Vector3Int(bounds.xMax, bounds.yMin - 1, 0);

            Set(topLeft, BorderPieceType.TopLeft);
            Set(topRight, BorderPieceType.TopRight);
            Set(bottomLeft, BorderPieceType.BottomLeft);
            Set(bottomRight, BorderPieceType.BottomRight);
        }

        private void Set(Vector3Int position, BorderPieceType borderPieceType) {
            for (var i = 0; i < borderPieces.Length; i++) {
                if (borderPieceType == borderPieces[i].borderPiece) {
                    Tilemap.SetTile(position, borderPieces[i].tile);
                }
            }
        }

    }
}

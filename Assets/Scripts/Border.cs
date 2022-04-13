using UnityEngine;
using UnityEngine.Tilemaps;

public class Border : MonoBehaviour
{
    public Board board;
    public BorderPieceData[] borderPieces;
    public Vector3 thisPosition;
    public Tilemap tilemap { get; private set; }

    /// <summary>
    /// Create the Border gameobject and initialize
    /// </summary>
    public static void Initialize(Board board) {
        GameObject border = new GameObject("Border");
        border.transform.parent = board.transform;
        border.AddComponent<Tilemap>();
        border.AddComponent<TilemapRenderer>();
        Border borderComp = border.AddComponent<Border>();
        borderComp.board = board;
        borderComp.borderPieces = board.gameData.borderPieces;
        borderComp.DrawBorder();
    }

    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
    }

    public void DrawBorder() {
        RectInt bounds = this.board.WorldBounds;

        // Draw top/bottom border
        for (int col = bounds.xMin; col < bounds.xMax; col++) {
            Vector3Int top = new Vector3Int(col, (bounds.yMax), 0);
            Vector3Int bottom = new Vector3Int(col, (bounds.yMin - 1), 0);
            Set(top, BorderPieceType.Top);
            Set(bottom, BorderPieceType.Bottom);
        }

        // Draw sides border
        for (int row = bounds.yMin; row < bounds.yMax; row++) {
            Vector3Int left = new Vector3Int((bounds.xMin - 1), row, 0);
            Vector3Int right = new Vector3Int((bounds.xMax), row, 0);
            Set(left, BorderPieceType.Left);
            Set(right, BorderPieceType.Right);
        }

        DrawCorners(bounds);
    }

    private void DrawCorners(RectInt bounds) {
        Vector3Int topLeft = new Vector3Int(bounds.xMin - 1, bounds.yMax, 0);
        Vector3Int topRight = new Vector3Int(bounds.xMax, bounds.yMax, 0);
        Vector3Int bottomLeft = new Vector3Int(bounds.xMin - 1, bounds.yMin - 1, 0);
        Vector3Int bottomRight = new Vector3Int(bounds.xMax, bounds.yMin - 1, 0);

        Set(topLeft, BorderPieceType.TopLeft);
        Set(topRight, BorderPieceType.TopRight);
        Set(bottomLeft, BorderPieceType.BottomLeft);
        Set(bottomRight, BorderPieceType.BottomRight);
    }

    public void Set(Vector3Int position, BorderPieceType borderPieceType) {
        for (int i = 0; i < borderPieces.Length; i++) {
            if (borderPieceType == borderPieces[i].borderPiece) {
                this.tilemap.SetTile(position, borderPieces[i].tile);
            }
        }
    }

}

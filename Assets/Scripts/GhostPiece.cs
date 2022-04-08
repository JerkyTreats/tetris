using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Tracks the Active Piece and shows the position that piece would be if it were dropped.
/// </summary>
public class GhostPiece : MonoBehaviour
{
    public Board board;
    public Tile tile;
    public ActivePiece trackingPiece { get; private set; }
    public Tilemap tilemap { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }

    public void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector3Int[4];
    }

    public void Initialize() {
        this.board = gameObject.GetComponentInParent<Board>();
        this.trackingPiece = board.activePiece;
        this.tile = board.ghost;
    }

    // Do late update to track when the real piece moves
    private void LateUpdate() {
        Clear();
        Copy();
        Drop();
        Set();
    }

    /// <summary>
    /// Remove the ghost from the board
    /// </summary>
    private void Clear() {
        for (int i = 0; i < this.cells.Length; i++) {
            Vector3Int tilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    /// <summary>
    /// Clone the tracking piece shape as the ghosts shape
    /// </summary>
    private void Copy() {
        for (int i = 0; i < this.cells.Length; i++) {
            this.cells[i] = this.trackingPiece.cells[i];
        }
    }

    /// <summary>
    /// Drops the piece to the correct row
    /// </summary>
    private void Drop() {
        RectInt bounds = this.board.Bounds;
        int row = this.trackingPiece.position.y - 1;

        // Wrap logic in Clear/Set as IsValidPosition will be false
        // As Ghost will be in same position as tracking piece
        this.board.Clear(this.trackingPiece);

        while ( row >= bounds.yMin - 1 ) {
            Vector3Int pos = this.trackingPiece.position;
            pos.y = row;

            if(this.board.IsValidPosition(this.trackingPiece, pos)) {
                row--;
            } else {
                pos.y = row + 1;
                this.position = pos;
                break;
            }
        }

        this.board.Set(this.trackingPiece);
    }

    private void Set() {
        for (int i = 0; i < this.cells.Length; i++) {
            Vector3Int tilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, this.tile);
        }
    }
}

using UnityEngine;
using UnityEngine.Tilemaps;
public class Ghost : MonoBehaviour
{
    public Tile tile;
    public Board board;
    public Piece trackingPiece;

    public Tilemap tilemap { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }

    public void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector3Int[4];
    }

    // Do late update to track when the real piece moves
    private void LateUpdate() {
        Clear();
        Copy();
        Drop();
        Set();
    }

    private void Clear() {
        for (int i = 0; i < this.cells.Length; i++) {
            Vector3Int tilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    private void Copy() {
        for (int i = 0; i < this.cells.Length; i++) {
            this.cells[i] = this.trackingPiece.cells[i];
        }
    }

    private void Drop() {
        RectInt bounds = this.board.Bounds;
        int row = bounds.yMin;

        // Wrap logic in Clear/Set as IsValidPosition will be false
        // As Ghost will be in same position as tracking piece
        this.board.Clear(this.trackingPiece);

        while ( row < bounds.yMax ) {
            Vector3Int pos = this.trackingPiece.position;
            pos.y = row - 1;

            if(this.board.IsValidPosition(this.trackingPiece, pos)) {
                this.position = pos;
                break;
            } else {
                row++;
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

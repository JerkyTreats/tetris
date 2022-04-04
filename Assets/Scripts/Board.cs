using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public TetrominoData[] tetrominos;
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10,20);

    public RectInt Bounds {
        get {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }
    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();

        for (int i = 0;  i < tetrominos.Length; i++) {
            this.tetrominos[i].Initialize();
        }
    }

    private void Start() {
        SpawnPiece();
    }

    public void SpawnPiece() {
        int rnd = Random.Range(0, this.tetrominos.Length);
        TetrominoData data = this.tetrominos[rnd];

        this.activePiece.Initialize(spawnPosition, data, this);

        if (IsValidPosition(this.activePiece, this.spawnPosition)) {
            Set(this.activePiece);
        } else {
            GameOver();
        }
    }

    private void GameOver() {
        this.tilemap.ClearAllTiles();

        //...
    }

    /// <summary>
    /// Set piece on board
    /// </summary>
    /// <param name="piece"></param>
    public void Set(Piece piece) {
        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    /// <summary>
    /// Remove peice from board
    /// </summary>
    /// <param name="piece"></param>
    public void Clear(Piece piece) {
        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    /// <summary>
    /// Determine if piece is within bounds
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="newPosition"></param>
    /// <returns>bool</returns>
    public bool IsValidPosition(Piece piece, Vector3Int newPosition) {
        RectInt bounds = this.Bounds;

        for (int i = 0; i < piece.cells.Length; i++ ) {
            Vector3Int tilePosition = piece.cells[i] + newPosition;

            if (!bounds.Contains((Vector2Int)tilePosition)) {
                return false;
            }

            if (this.tilemap.HasTile(tilePosition)) {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Clear a fully completed line
    /// </summary>
    public void CheckClearedLines() {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;
        while(row < bounds.yMax) {
            if (IsLineFull(row)) {
                LineClear(row);
            } else {
                row++;
            }
        }
    }

    /// <summary>
    /// Determine if line is full
    /// </summary>
    /// <param name="row">Row index to check</param>
    /// <returns>bool</returns>
    private bool IsLineFull(int row) {
        RectInt bounds = this.Bounds;

        // For each column in a row, determine if a Tile is missing
        for (int col = bounds.xMin; col < bounds.xMax; col++) {
            Vector3Int pos = new Vector3Int(col, row, 0);

            if (!this.tilemap.HasTile(pos)) {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Remove the tiles in a row and step remaining lines down
    /// </summary>
    /// <param name="row">Row index to clear</param>
    private void LineClear(int row) {
        RectInt bounds = this.Bounds;

        // For each column in a row, set the Tile to null
        for (int col = bounds.xMin; col < bounds.xMax; col++) {
            Vector3Int pos = new Vector3Int(col, row, 0);

            this.tilemap.SetTile(pos, null);
        }

        // Move each row down once cleared
        while (row < bounds.yMax) {
            for (int col = bounds.xMin; col < bounds.xMax; col++) {
                Vector3Int pos = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(pos);

                pos = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(pos, above);
            }

            row++;
        }
    }

}

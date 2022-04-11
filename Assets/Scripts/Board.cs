using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class Board : MonoBehaviour
{
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10,20);
    public Vector3Int cameraPosition = new Vector3Int(0,0,-10);
    public Vector3Int boardPosition;
    public GameData gameData { get; private set; }
    public Tilemap tilemap { get; private set; }
    public ActivePiece activePiece { get; private set; }
    public List<Tetromino> spawnBag { get; private set; }
    public BoardCamera boardCamera { get; private set; }

    public RectInt Bounds {
        get {
            Vector2Int position = new Vector2Int(
                this.boardPosition.x - this.boardSize.x / 2,
                this.boardPosition.y - this.boardSize.y / 2
            );

            Vector2Int size = new Vector2Int(
                this.boardPosition.x + this.boardSize.x,
                this.boardPosition.y + this.boardSize.y
            );
            return new RectInt(position, size);
        }
    }

    /// <summary>
    /// Creates a new board object
    /// </summary>
    /// <param name="spawnPosition">Spawn position for each Active Piece</param>
    /// <param name="cameraPosition">Camera position for the board</param>
    /// <param name="boardPosition">Position of the board</param>
    /// <param name="boardSize">Size of the board in tiles</param>
    /// <param name="sortOrder">Tilemap sort order, larger means most visible layer</param>
    /// <returns>Created Board</returns>
    public static Board Initialize(Vector3Int spawnPosition, Vector3Int cameraPosition, Vector3Int boardPosition, Vector2Int boardSize, int sortOrder) {
        GameObject boardGO = new GameObject("Board");
        boardGO.AddComponent<Grid>();
        boardGO.transform.position = boardPosition;

        boardGO.AddComponent<Tilemap>();
        TilemapRenderer renderer = boardGO.AddComponent<TilemapRenderer>();
        renderer.sortingOrder = sortOrder;

        Board board = boardGO.AddComponent<Board>();
        board.spawnPosition = spawnPosition;
        board.boardSize = boardSize;
        board.cameraPosition = cameraPosition;
        board.boardCamera = new BoardCamera(board.gameObject, cameraPosition);

        Border.Initialize(board);
        BoardBackground.Initialize(board);

        return board;
    }

    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();

        GameObject gameDataObject = GameObject.Find("GameData");
        this.gameData = gameDataObject.GetComponent<GameData>();

        // Initialize each Tetromino listed in Editor object
        for (int i = 0;  i < this.gameData.tetrominos.Length; i++) {
            this.gameData.tetrominos[i].Initialize();
        }
    }

    void Start() {
        // ActivateGameOnBoard();
    }

    /// <summary>
    /// Make this board the active one with the game playing.
    /// </summary>
    public void ActivateGameOnBoard() {
        this.activePiece = this.gameObject.AddComponent<ActivePiece>();
        boardCamera.ActivateCamera();
        SpawnPiece();
        GhostPiece.Initialize(this);
    }

    public void DeactivateGame() {
        Destroy(this.GetComponentInChildren<GhostPiece>().gameObject);
        Destroy(this.activePiece);
        boardCamera.DeactivateCamera();
        this.tilemap.ClearAllTiles();
    }

    /// <summary>
    /// Spawn a new tetromino. Trigger Game Over state if applicable.
    /// </summary>
    public void SpawnPiece() {
        Tetromino nextPiece = GetNextPeice();

        for (int i = 0; i < this.gameData.tetrominos.Length; i++){
            if (this.gameData.tetrominos[i].tetromino == nextPiece) {
                this.activePiece.Initialize(spawnPosition, this.gameData.tetrominos[i], this);
            }
        }

        // False if there is a piece colliding in spawn position.
        if (IsValidPosition(this.activePiece, this.spawnPosition)) {
            Set(this.activePiece);
        } else {
            GameOver();
        }
    }

    /// <summary>
    /// Randomly cycle through a bag of each type of Tetromino. Refill bag on empty.
    /// </summary>
    /// <returns>Tetromino Enum to spawn next</returns>
    private Tetromino GetNextPeice() {
        if (this.spawnBag == null || this.spawnBag.Count == 0){
            spawnBag = new List<Tetromino> {
                Tetromino.I,
                Tetromino.O,
                Tetromino.T,
                Tetromino.J,
                Tetromino.L,
                Tetromino.S,
                Tetromino.Z,
            };
        }

        int rnd = Random.Range(0, this.spawnBag.Count);
        Tetromino nextPiece = spawnBag[rnd];
        spawnBag.RemoveAt(rnd);
        return nextPiece;
    }



    private void GameOver() {
        this.tilemap.ClearAllTiles();

        // TODO : Something useful with this
    }

    /// <summary>
    /// Set piece on board
    /// </summary>
    /// <param name="piece"></param>
    public void Set(ActivePiece piece) {
        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    /// <summary>
    /// Remove peice from board
    /// </summary>
    /// <param name="piece"></param>
    public void Clear(ActivePiece piece) {
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
    public bool IsValidPosition(ActivePiece piece, Vector3Int newPosition) {
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

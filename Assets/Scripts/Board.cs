using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class Board : MonoBehaviour
{
    public Vector3Int spawnPosition;
    public Vector2Int boardSize;
    public Vector3Int cameraPosition;
    public Vector3Int boardPosition;
    public GameData gameData { get; private set; }
    public Tilemap tilemap { get; private set; }
    public ActivePiece activePiece { get; private set; }
    public List<Tetromino> spawnBag { get; private set; }
    public BoardCamera boardCamera { get; private set; }

    /// <summary>
    /// Rectangle representing the board in WorldSpace
    /// </summary>
    public RectInt WorldBounds {
        get {

            // Origin is middle of rect, position is bottom left
            Vector2Int position = new Vector2Int(
                this.boardPosition.x - this.boardSize.x / 2,
                this.boardPosition.y - this.boardSize.y / 2
            );

            return new RectInt(position, this.boardSize);
        }
    }

    /// <summary>
    /// Rectangle representing the board in TileSpace (Origin is always 0,0)
    /// </summary>
    public RectInt TileBounds {
        get {
            // bottom left
            Vector2Int position = new Vector2Int(
                -this.boardSize.x / 2,
                -this.boardSize.y / 2
            );

            return new RectInt(position, this.boardSize);
        }
    }

    /// <summary>
    /// EDITOR: Draw the WorldBounds so inspect while game is paused
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Draw a yellow cube at the transform position
        Gizmos.color = Color.yellow;
        // Gizmos.DrawWireCube(transform.position, new Vector3(WorldBounds.size.x, WorldBounds.size.y, 0));
        Gizmos.DrawWireCube(this.transform.position, new Vector3(WorldBounds.size.x, WorldBounds.size.y, 0));
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
        board.boardPosition = boardPosition;
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
        for (int i = 0; i < piece.cells.Length; i++ ) {
            Vector3Int tilePosition = piece.cells[i] + newPosition;

            if (!this.TileBounds.Contains((Vector2Int)tilePosition)) {
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
        RectInt bounds = this.TileBounds;
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
        RectInt bounds = this.TileBounds;

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
        RectInt bounds = this.TileBounds;

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

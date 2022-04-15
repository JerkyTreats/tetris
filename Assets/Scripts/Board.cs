using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Vector3Int spawnPosition;
    public Vector2Int boardSize;
    public Vector3Int boardPosition;

    public GameData GameData { get; private set; }
    public ActivePiece ActivePiece { get; private set; }
    public BoardCamera boardCamera;
    
    private Tilemap Tilemap { get; set; }
    private List<Tetromino> SpawnBag { get; set; }

    /// <summary>
    /// Rectangle representing the board in WorldSpace
    /// </summary>
    public RectInt WorldBounds {
        get {
            // Origin is middle of rect, position is bottom left
            var position = new Vector2Int(
                boardPosition.x - boardSize.x / 2,
                boardPosition.y - boardSize.y / 2
            );

            return new RectInt(position, boardSize);
        }
    }

    /// <summary>
    /// Rectangle representing the board in TileSpace (Origin is always 0,0)
    /// </summary>
    private RectInt TileBounds {
        get {
            var bottomLeft = new Vector2Int(
                -boardSize.x / 2,
                -boardSize.y / 2
            );

            return new RectInt(bottomLeft, boardSize);
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
        Gizmos.DrawWireCube(transform.position, new Vector3(WorldBounds.size.x, WorldBounds.size.y, 0));
    }

    private void Awake() {
        Tilemap = GetComponentInChildren<Tilemap>();

        var gameDataObject = GameObject.Find("GameData");
        GameData = gameDataObject.GetComponent<GameData>();

        // Initialize each Tetromino listed in Editor object
        for (var i = 0;  i < GameData.tetrominos.Length; i++) {
            GameData.tetrominos[i].Initialize();
        }
    }

    /// <summary>
    /// Make this board the active one with the game playing.
    /// </summary>
    public void ActivateGame() {
        ActivePiece = gameObject.AddComponent<ActivePiece>();
        boardCamera.ActivateCamera();
        SpawnPiece();
        GhostPiece.Initialize(this);
    }

    public void DeactivateGame() {
        Destroy(GetComponentInChildren<GhostPiece>().gameObject);
        Destroy(ActivePiece);
        boardCamera.DeactivateCamera();
        Tilemap.ClearAllTiles();
    }

    /// <summary>
    /// Spawn a new Tetromino. Trigger Game Over state if applicable.
    /// </summary>
    public void SpawnPiece() {
        var nextPiece = GetNextPiece();

        for (var i = 0; i < GameData.tetrominos.Length; i++){
            if (GameData.tetrominos[i].tetromino == nextPiece) {
                ActivePiece.Initialize(spawnPosition, GameData.tetrominos[i], this);
            }
        }

        // False if there is a piece colliding in spawn position.
        if (IsValidPosition(ActivePiece, spawnPosition)) {
            Set(ActivePiece);
        } else {
            GameOver();
        }
    }

    /// <summary>
    /// Randomly cycle through a bag of each type of Tetromino. Refill bag on empty.
    /// </summary>
    /// <returns>Tetromino Enum to spawn next</returns>
    private Tetromino GetNextPiece() {
        if (SpawnBag == null || SpawnBag.Count == 0){
            SpawnBag = new List<Tetromino> {
                Tetromino.I,
                Tetromino.O,
                Tetromino.T,
                Tetromino.J,
                Tetromino.L,
                Tetromino.S,
                Tetromino.Z,
            };
        }

        var rnd = Random.Range(0, SpawnBag.Count);
        var nextPiece = SpawnBag[rnd];
        SpawnBag.RemoveAt(rnd);
        return nextPiece;
    }



    private void GameOver() {
        Tilemap.ClearAllTiles();

        // TODO : Something useful with this
    }

    /// <summary>
    /// Set piece on board
    /// </summary>
    /// <param name="piece"></param>
    public void Set(ActivePiece piece)
    {
        foreach (var t in piece.Cells)
        {
            var tilePosition = t + piece.Position;
            Tilemap.SetTile(tilePosition, piece.Data.tile);
        }
    }

    /// <summary>
    /// Remove piece from board
    /// </summary>
    /// <param name="piece"></param>
    public void Clear(ActivePiece piece)
    {
        foreach (var t in piece.Cells)
        {
            var tilePosition = t + piece.Position;
            Tilemap.SetTile(tilePosition, null);
        }
    }

    /// <summary>
    /// Determine if piece is within bounds
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="newPosition"></param>
    /// <returns>bool</returns>
    public bool IsValidPosition(ActivePiece piece, Vector3Int newPosition)
    {
        // Loop through each tile in the Tetromino
        foreach (var tilePosition in piece.Cells)
        {
            var newTilePosition = tilePosition + newPosition;

            if (!TileBounds.Contains((Vector2Int)newTilePosition)) {
                return false;
            }

            if (Tilemap.HasTile(newTilePosition)) {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Clear a fully completed line
    /// </summary>
    public void CheckClearedLines() {
        var bounds = TileBounds;
        var row = bounds.yMin;
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
        var bounds = TileBounds;

        // For each column in a row, determine if a Tile is missing
        for (var col = bounds.xMin; col < bounds.xMax; col++) {
            var pos = new Vector3Int(col, row, 0);

            if (!Tilemap.HasTile(pos)) {
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
        var bounds = TileBounds;

        // For each column in a row, set the Tile to null
        for (var col = bounds.xMin; col < bounds.xMax; col++) {
            var pos = new Vector3Int(col, row, 0);

            Tilemap.SetTile(pos, null);
        }

        // Move each row down once cleared
        while (row < bounds.yMax) {
            for (var col = bounds.xMin; col < bounds.xMax; col++) {
                var pos = new Vector3Int(col, row + 1, 0);
                var above = Tilemap.GetTile(pos);

                pos = new Vector3Int(col, row, 0);
                Tilemap.SetTile(pos, above);
            }

            row++;
        }
    }
}

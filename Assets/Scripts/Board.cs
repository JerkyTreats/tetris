using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Vector2Int boardSize;
    public Vector3Int boardPosition;

    public GameData GameData { get; private set; }
    public BoardCamera boardCamera;
    public GameController gameController;

    public Tilemap Tilemap { get; private set; }

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
    public RectInt TileBounds {
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

    public void OnActivate() {
        boardCamera.ActivateCamera();
        gameController.OnActivate();
    }

    public void OnDeactivate() {
        boardCamera.DeactivateCamera();
        gameController.OnDeactivate();
        Tilemap.ClearAllTiles();
    }

    /// <summary>
    /// Set piece on board
    /// </summary>
    /// <param name="piece"></param>
    public void Set(ActivePiece piece)
    {
        foreach (var t in piece.Cells)
        {
            var tilePosition = t + piece.position;
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
            var tilePosition = t + piece.position;
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
}

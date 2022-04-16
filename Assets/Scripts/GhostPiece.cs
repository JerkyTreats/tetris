using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Tracks the Active Piece and shows the position that piece would be if it were dropped.
/// </summary>
public class GhostPiece : MonoBehaviour
{
    public Board board;
    public Tile tile;
    private ActivePiece TrackingPiece { get; set; }
    private Tilemap Tilemap { get; set; }
    private Vector3Int[] Cells { get; set; }
    private Vector3Int Position { get; set; }

    /// <summary>
    /// Create the Ghost Piece GameObject and initialize
    /// </summary>
    public static GhostPiece CreateNewGhostPiece(Board board, ActivePiece activePiece, Tile tile)
    {
        var ghostObject = TileGameObjectFactory.CreateNewTileObject("Ghost", Vector3Int.zero, 1, board.transform);

        var ghostPiece = ghostObject.AddComponent<GhostPiece>();
        ghostPiece.board = board;
        ghostPiece.TrackingPiece = activePiece;
        ghostPiece.transform.localPosition = Vector3.zero;
        ghostPiece.tile = tile;

        return ghostPiece;
    }

    public void Awake() {
        Tilemap = GetComponentInChildren<Tilemap>();
        Cells = new Vector3Int[4];
    }

    // Do late update to track when the real piece moves
    private void LateUpdate()
    {
        Clear();
        Copy();
        Drop();
        Set();
    }

    /// <summary>
    /// Remove the ghost from the board
    /// </summary>
    private void Clear()
    {
        foreach (var tilePosition in Cells)
        {
            var newTilePosition = tilePosition + Position;
            Tilemap.SetTile(newTilePosition, null);
        }
    }

    /// <summary>
    /// Clone the tracking piece shape as the ghosts shape
    /// </summary>
    private void Copy() {
        for (var i = 0; i < Cells.Length; i++) {
            Cells[i] = TrackingPiece.Cells[i];
        }
    }

    /// <summary>
    /// Drops the piece to the correct row
    /// </summary>
    private void Drop() {
        var bounds = board.WorldBounds;
        var row = TrackingPiece.position.y - 1;

        // Wrap logic in Clear/Set as IsValidPosition will be false as Ghost will be in same position as tracking piece
        board.Clear(TrackingPiece);

        while ( row >= bounds.yMin - 1 ) {
            var pos = TrackingPiece.position;
            pos.y = row;

            if(board.IsValidPosition(TrackingPiece, pos)) {
                row--;
            } else {
                pos.y = row + 1;
                Position = pos;
                break;
            }
        }

        board.Set(TrackingPiece);
    }

    private void Set()
    {
        foreach (var tilePosition in Cells)
        {
            var newTilePosition = tilePosition + Position;
            Tilemap.SetTile(newTilePosition, tile);
        }
    }
}

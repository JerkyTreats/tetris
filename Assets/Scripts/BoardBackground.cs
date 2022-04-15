using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardBackground : MonoBehaviour
{
    public Board board;
    public Tile gridTile;
    private Tilemap TileMap { get; set; }

    /// <summary>
    /// Create the BoardBackground GameObject and initialize
    /// </summary>
    public static void CreateBoardBackground(Board board) {
        var backgroundGo = new GameObject("BoardBackground")
        {
            transform =
            {
                parent = board.transform
            }
        };
        backgroundGo.AddComponent<Tilemap>();
        backgroundGo.AddComponent<TilemapRenderer>();
        var boardBackground = backgroundGo.AddComponent<BoardBackground>();
        boardBackground.board = board;
        boardBackground.gridTile = board.GameData.gridTile;
    }


    public void Awake() {
        TileMap = GetComponentInChildren<Tilemap>();
    }

    public void Start() {
        DrawGrid();
    }

    private void DrawGrid() {
        var bounds = board.WorldBounds;

        for (var row = bounds.yMin; row < bounds.yMax; row++) {
            for (var col = bounds.xMin; col < bounds.xMax; col++) {
                var pos = new Vector3Int(col, row, 0);
                TileMap.SetTile(pos, gridTile);
            }
        }
    }

}

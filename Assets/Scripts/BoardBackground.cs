using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardBackground : MonoBehaviour
{
    public Board board;
    public Tile gridTile;
    public Tilemap tileMap { get; private set; }

    /// <summary>
    /// Create the BoardBackground gameobject and initialize
    /// </summary>
    public static void Initialize(Board board) {
        GameObject backgroundGO = new GameObject("BoardBackground");
        backgroundGO.transform.parent = board.transform;
        backgroundGO.AddComponent<Tilemap>();
        backgroundGO.AddComponent<TilemapRenderer>();
        BoardBackground borderComp = backgroundGO.AddComponent<BoardBackground>();
        borderComp.board = board;
        borderComp.gridTile = board.gameData.gridTile;
    }


    public void Awake() {
        this.tileMap = GetComponentInChildren<Tilemap>();
    }

    public void Start() {
        DrawGrid();
    }

    public void DrawGrid() {
        RectInt bounds = this.board.Bounds;

        for (int row = bounds.yMin; row < bounds.yMax; row++) {
            for (int col = bounds.xMin; col < bounds.xMax; col++) {
                Vector3Int pos = new Vector3Int(col, row, 0);
                this.tileMap.SetTile(pos, this.gridTile);
            }
        }
    }

}

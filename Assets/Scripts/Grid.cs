using UnityEngine;
using UnityEngine.Tilemaps;


public class BoardGrid : MonoBehaviour
{
    public Board board;
    public Tile gridTile;
    public Tilemap tileMap { get; private set; }

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
                Debug.Log(pos);
                this.tileMap.SetTile(pos, this.gridTile);
            }
        }
    }

}

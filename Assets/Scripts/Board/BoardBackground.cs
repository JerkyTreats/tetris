using Common;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Board
{
    public class BoardBackground : MonoBehaviour
    {
        public Board board;
        public Tile gridTile;
        private Tilemap TileMap { get; set; }

        /// <summary>
        /// Create the BoardBackground GameObject and initialize
        /// </summary>
        public static BoardBackground CreateBoardBackground(Board board)
        {
            var backgroundObject = TileGameObjectFactory.CreateNewTileObject("BoardBackground", Vector3Int.zero, 0, board.transform);
            var boardBackground = backgroundObject.AddComponent<BoardBackground>();
            boardBackground.board = board;
            boardBackground.gridTile = board.GameData.gridTile;

            return boardBackground;
        }


        private void Awake() {
            TileMap = GetComponentInChildren<Tilemap>();
        }

        private void Start() {
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
}

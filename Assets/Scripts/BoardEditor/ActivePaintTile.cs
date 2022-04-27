using Board.Persistence;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BoardEditor
{
    /// <summary>
    /// Tile selected to paint on a board
    /// </summary>
    public class ActivePaintTile :MonoBehaviour
    {
        private Tile Tile { get; set; }
        private Tilemap TileMap { get; set; }
        private Block Block { get; set; } 
        
        public Board.Board board;

        /// <summary>
        /// Create ActivePainter GameObject with setup components
        /// </summary>
        /// <param name="paintButton"></param>
        /// <param name="tilemap"></param>
        /// <param name="block"></param>
        public static void CreateNewActivePainter(PaintButton paintButton, Tilemap tilemap, Block block)
        {
            var activePaintTileObject = new GameObject("ActivePainter");
            var activePaintTile = activePaintTileObject.AddComponent<ActivePaintTile>();
            activePaintTile.Tile = paintButton.tile;
            activePaintTile.TileMap = tilemap;
            activePaintTile.Block = block;
        }
        
        private void Start()
        {
            board = FindObjectOfType<Board.Board>();
        }

        // TODO ActivePaintTile Input System overhaul
        private void Update()
        {
            // Left click
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.A))
                PaintTile();
        
            // Right click
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.L))
                Destroy(gameObject);
        }

        // Convert user input on screen to coordinates on Board Grid
        // Place a Tile at the location pressed
        // TODO ActivePaintTile paints tile on Mouse position alone
        // Should support mobile click 
        // See issues for Input System overhaul
        private void PaintTile()
        {
            if (Camera.main == null) return;
            
            // Screen space to camera space
            var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = board.BoardPosition.z;
            
            // Camera space to tilemap coord
            var tilePosition = TileMap.WorldToCell(worldPos);

            if (board.IsValidPosition(tilePosition))
                board.SetTile(Block, tilePosition, Tile);
        }
    }
}
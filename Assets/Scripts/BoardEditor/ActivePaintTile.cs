using UnityEngine;
using UnityEngine.Tilemaps;

namespace BoardEditor
{
    public class ActivePaintTile :MonoBehaviour
    {
        private Tile Tile { get; set; }
        private Tilemap TileMap { get; set; }
        public Board board;

        /// <summary>
        /// Create ActivePainter GameObject with setup components
        /// </summary>
        /// <param name="paintButton"></param>
        /// <param name="tilemap"></param>
        public static void CreateNewActivePainter(PaintButton paintButton, Tilemap tilemap)
        {
            var activePaintTileObject = new GameObject("ActivePainter");
            var activePaintTile = activePaintTileObject.AddComponent<ActivePaintTile>();
            activePaintTile.Tile = paintButton.tile;
            activePaintTile.TileMap = tilemap;
        }
        
        private void Start()
        {
            board = FindObjectOfType<Board>();
        }

        private void Update()
        {
            // Left click
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.A))
                PaintTile();
        
            // Right click
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.L))
                Destroy(gameObject);
        }

        private void PaintTile()
        {
            if (Camera.main == null) return;
            
            // Screen space to camera space
            var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = board.boardPosition.z;
            
            // Camera space to tilemap coord
            var tilePosition = TileMap.WorldToCell(worldPos);

            if (board.IsValidPosition(tilePosition)) 
                TileMap.SetTile(tilePosition, Tile);
        }
    }
}
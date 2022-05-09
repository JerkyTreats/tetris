using System;
using Common;
using GameManagement;
using Initialization;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tetris
{
    /// <summary>
    /// Tracks the Active Piece and shows the position that piece would be if it were dropped.
    /// </summary>
    public class GhostPiece : ActivePiece
    {
        private Tilemap _tilemap;
        private Tile _tile;

        public static GhostPiece CreateNewGhostPiece(Vector3Int spawnPosition, TetrominoData data, Transform parent, Board.Board board)
        {
            var ghostPieceObject = TileGameObjectFactory.CreateNewTileObject("GhostPiece", board.transform.position, 1, parent);
        
            var ghostPiece = ghostPieceObject.AddComponent<GhostPiece>();
            ghostPiece.Data = data;
            ghostPiece.position = spawnPosition;
            ghostPiece._tilemap = ghostPiece.GetComponent<Tilemap>();
            ghostPiece.Board = board;

            ghostPiece.Cells ??= new Vector3Int[data.Cells.Length];

            for (var i = 0; i < data.Cells.Length; i++) {
                ghostPiece.Cells[i] = (Vector3Int)data.Cells[i];
            }

            ghostPiece.Drop(spawnPosition);

            return ghostPiece;
        }

        private void Awake()
        {
            _tile = Resources.Load<GameData>("GameData").ghostTile;
        }

        // Drops the piece to the correct row
        private void Drop(Vector3Int activePiecePosition) {
            var bounds = Board.WorldBounds;
            position = activePiecePosition;
            var row = activePiecePosition.y - 1;
            
            while ( row >= bounds.yMin - 1 ) {
                var pos = position;
                pos.y = row;

                if(IsValidPiecePosition(pos)) {
                    row--;
                } else {
                    pos.y = row + 1;
                    position = pos;
                    break;
                }
            }

            Set(_tilemap, _tile);
        }
        
        private void Copy(ActivePiece activePiece) {
            for (var i = 0; i < Cells.Length; i++) {
                Cells[i] = activePiece.Cells[i];
            }
        }
        
        public void Step(ActivePiece activePiece)
        {
            Copy(activePiece);
            Drop(activePiece.position);
        }

        public void Clear()
        {
            _tilemap.ClearAllTiles();
        }
    }
}

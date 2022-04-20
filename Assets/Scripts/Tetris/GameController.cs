using System.Collections.Generic;
using Common;
using Initialization;
using Unity.VisualScripting;
using UnityEngine;

// Controls the gameplay of Tetris 
// Where Board is the visual manifestation of the components of Tetris,
// And ActivePiece, Ghost, etc. control each individual component,
// This controls how the game operates- beginning, while active, and end.

// Short term - give Active Piece back its control. 
// Next - Implement Observer/Observable 
//          - GameController runs PreStep() 
//          - GameController issues Step() 
//          - Ghost/ActivePiece onStep()
//          - Ghost/ActivePiece announce() stepResults
//          - GameController runs PostStep()
namespace Tetris
{
    public class GameController : MonoBehaviour
    {
        private Board.Board _board;
        private GameData _gameData;
        private Vector3Int _spawnPosition;

        private float stepDelay = 1f;
        private float lockDelay = 0.5f;
        private float _stepTime;
        private float _lockTime;
        private int _rotationIndex;

    
        private List<Tetromino> SpawnBag { get; set; }

        private ActivePiece ActivePiece { get; set; }
        private GhostPiece GhostPiece { get; set; }

        public static GameController CreateNewGameLogic(Board.Board board, Vector3Int spawnPosition, float stepDelay = 1f, float lockDelay = 0.5f)
        {
            var gameLogic = board.AddComponent<GameController>();
            gameLogic._board = board;
            gameLogic._spawnPosition = spawnPosition;
            gameLogic._gameData = board.GameData;

            gameLogic._stepTime = Time.time + stepDelay;
            gameLogic._lockTime = 0f;

            return gameLogic;
        }
    
        public void OnActivate()
        {
            SpawnPiece();
        }
    
        public void OnDeactivate()
        {
            Destroy(GhostPiece.gameObject);
            Destroy(ActivePiece.gameObject);
        }

        public void Update()
        {
            if (!ActivePiece) return;
        
        
            _board.Clear(ActivePiece);

            _lockTime += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Q)) {
                Rotate(-1);
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                Rotate(1);
            }

            if (Input.GetKeyDown(KeyCode.A)) {
                Move(Vector2Int.left);
            } else if (Input.GetKeyDown(KeyCode.D)) {
                Move(Vector2Int.right);
            }

            if (Input.GetKeyDown(KeyCode.S)) {
                Move(Vector2Int.down);
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                HardDrop();
            }

            if (Time.time >= _stepTime) {
                Step();
            }

            _board.Set(ActivePiece);
        }

        /// <summary>
        /// Spawn a new Tetromino. Trigger Game Over state if applicable.
        /// </summary>
        private void SpawnPiece() {
            var nextPiece = GetNextPiece();

            for (var i = 0; i < _board.GameData.tetrominos.Length; i++)
            {
                if (_board.GameData.tetrominos[i].tetromino != nextPiece) continue;
            
                ActivePiece = ActivePiece.CreateNewActivePiece(_spawnPosition, _board.GameData.tetrominos[i], _board);
                GhostPiece = GhostPiece.CreateNewGhostPiece(_board, ActivePiece, _gameData.ghostTile);
            }

            // False if there is a piece colliding in spawn position.
            if (_board.IsValidPiecePosition(ActivePiece, _spawnPosition)) {
                _board.Set(ActivePiece);
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
            _board.Tilemap.ClearAllTiles();

            // TODO : Something useful with this
        }
    
    
        private void Step() {
            _stepTime = Time.time + stepDelay;

            Move(Vector2Int.down);

            if (_lockTime >= lockDelay) {
                Lock();
            }
        }

        private bool Move(Vector2Int translation) {
            var newPosition = ActivePiece.position;
            newPosition.x += translation.x;
            newPosition.y += translation.y;
            var valid = _board.IsValidPiecePosition(ActivePiece, newPosition);

            if (!valid) return false;
        
            ActivePiece.position = newPosition;
            _lockTime = 0f;

            return true;
        }

        private void HardDrop() {
            while ( Move(Vector2Int.down) ) {
            }
        }
    
        private void Lock() {
            _board.Set(ActivePiece);
            Destroy(ActivePiece.gameObject);
            Destroy(GhostPiece.gameObject);
            CheckClearedLines();
            SpawnPiece();
        }
    
        /// <summary>
        /// Clear a fully completed line
        /// </summary>
        private void CheckClearedLines() {
            var bounds = _board.TileBounds;
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
            var bounds = _board.TileBounds;

            // For each column in a row, determine if a Tile is missing
            for (var col = bounds.xMin; col < bounds.xMax; col++) {
                var pos = new Vector3Int(col, row, 0);

                if (!_board.Tilemap.HasTile(pos)) {
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
            var bounds = _board.TileBounds;

            // For each column in a row, set the Tile to null
            for (var col = bounds.xMin; col < bounds.xMax; col++) {
                var pos = new Vector3Int(col, row, 0);

                _board.Tilemap.SetTile(pos, null);
            }

            // Move each row down once cleared
            while (row < bounds.yMax) {
                for (var col = bounds.xMin; col < bounds.xMax; col++) {
                    var pos = new Vector3Int(col, row + 1, 0);
                    var above = _board.Tilemap.GetTile(pos);

                    pos = new Vector3Int(col, row, 0);
                    _board.Tilemap.SetTile(pos, above);
                }

                row++;
            }
        }
    
        /// <summary>
        /// Rotation is quite complex, especially to achieve the Super Rotation System players don't realize they expect: https://tetris.fandom.com/wiki/SRS
        /// </summary>
        /// <param name="direction">Represents index to move to. -1 or 1 expected</param>
        private void Rotate(int direction) {
            var originRotation = _rotationIndex;
            _rotationIndex = Helpers.Wrap(_rotationIndex + direction, 0, 4);

            // Rotate the piece
            ActivePiece.ApplyRotationMatrix(direction);

            // Test if rotation would put piece out of bounds
            // Revert if tests failed
            if (TestWallKicks(_rotationIndex, direction)) return;
      
            _rotationIndex = originRotation;
            ActivePiece.ApplyRotationMatrix(-direction);
        }
    
        private bool TestWallKicks(int rotationIndex, int rotationDirection) {
            var wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        
            for (var i = 0; i < ActivePiece.Data.WallKicks.GetLength(1); i++){
                var translation = ActivePiece.Data.WallKicks[wallKickIndex, i];

                if (Move(translation)){
                    return true;
                }
            }

            return false;
        }

        private int GetWallKickIndex(int rotationIndex, int rotationDirection) {
            var wallKickIndex = rotationIndex * 2;

            if (rotationDirection < 0){
                wallKickIndex--;
            }

            return Helpers.Wrap(wallKickIndex, 0, ActivePiece.Data.WallKicks.GetLength(0));
        }
    }
}
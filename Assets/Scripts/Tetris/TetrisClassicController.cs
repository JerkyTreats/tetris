using System.Collections.Generic;
using GameManagement;
using Initialization;
using UnityEngine;

// Controls the gameplay of Tetris 
// Where Board is the visual manifestation of the components of Tetris,
// And ActivePiece, Ghost, etc. control each individual component,
// This controls how the game operates- beginning, while active, and end.
namespace Tetris
{
    public class TetrisClassicController : GameController, IGameController
    {
        public new TetrisClassicData Data
        {
            get => _data;
            set
            {
                _data = value;
                base.Data = value;
            }
        }
        
        

        private Board.Board _board;
        private GameData _gameData;

        private bool _isGameActive;
        [SerializeField] private TetrisClassicData _data;

        private List<Tetromino> SpawnBag { get; set; }

        private ActivePiece ActivePiece { get; set; }
        private GhostPiece GhostPiece { get; set; }

        public static TetrisClassicController CreateNewGameController(TetrisClassicData data)
        {
            var controllerObj = new GameObject("GameController"); 
            var controller = controllerObj.AddComponent<TetrisClassicController>();
            controller.Data = data;

            return controller;
        }

        public new void Initialize(GameManager manager)
        {
            _board = Board.Board.CreateNewBoard(Data.BoardData, transform);
            
            base.Initialize(manager);
        }

        private void Awake()
        {
            _gameData = Resources.Load<GameData>("GameData");
        }
        
        public new void GameStart(IGameController controller)
        {
            if (controller.Data.Guid != Data.Guid ) return;
            Debug.Log(controller.Data.Guid + " " + Data.Guid);
            
            Debug.Log("OVERLOAD START");
            _board.Activate();
            SpawnPiece();
            
            _isGameActive = true;
            base.GameStart(controller);
        }

        /// <summary>
        /// Spawn a new Tetromino. Trigger Game Over state if applicable.
        /// </summary>
        private void SpawnPiece() {
            Debug.Log($"Spawning Piece for [{Data.Guid}]");
            var nextPiece = GetNextPiece();

            for (var i = 0; i < _board.GameData.tetrominos.Length; i++)
            {
                if (_board.GameData.tetrominos[i].tetromino != nextPiece) continue;
            
                GhostPiece = GhostPiece.CreateNewGhostPiece(Data.SpawnPosition, _gameData.tetrominos[i], transform, _board);
                ActivePiece = ActivePiece.CreateNewActivePiece(Data.SpawnPosition, _gameData.tetrominos[i], transform, _board, Data.StepDelay, Data.LockDelay );
                ActivePiece.LockEvent += Lock;
            }
            
            if (!ActivePiece.IsValidPiecePosition(Data.SpawnPosition))
                GameEnd(this);
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

        // While game is active
        // Remove Active piece tiles from the board 
        // Step the active piece 
        // Determine if piece is locked
        // Step the GhostPiece
        // Set the Active piece in its new location
        private void Update()
        {
            if (!_isGameActive) return;
            
            ActivePiece.LockTime += Time.deltaTime;
        
            ActivePiece.Clear(_board.Tilemap);
            GhostPiece.Clear();
            
            ActivePiece.Step();

            GhostPiece.Step(ActivePiece);
            ActivePiece.Set(_board.Tilemap, ActivePiece.Data.tile);
        }
        
        private void Lock()
        {
            ActivePiece.Set(_board.Tilemap, ActivePiece.Data.tile);
            ActivePiece.Terminate();
            GhostPiece.Terminate();
            
            CheckClearedLines();
            SpawnPiece();
        }

        private new void GameEnd(IGameController controller)
        {
            if (controller.Data.Guid != Data.Guid) return;
            
            Debug.Log($"Ending game for [{Data.Guid}]");
            
            _board.Tilemap.ClearAllTiles();
            _board.Deactivate();
            
            GhostPiece.Terminate();
            ActivePiece.Terminate();
            
            _isGameActive = false;
            
            base.GameEnd(controller);
        }
    
        // Clear a fully completed line
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

        // Determine if line is full
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

        // Remove the tiles in a row and step remaining lines down
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
    }
}
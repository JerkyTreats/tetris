using Common;
using GameManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tetris
{
    public class ActivePiece : MonoBehaviour, IManagedGameObject
    {
        public TetrominoData Data { get; set; }
        public Vector3Int position;// { get; private set; }
        public Vector3Int[] Cells { get; set; }
        
        public Board.Board Board { get; set; }
        
        public float LockTime { get; set; }
        private float _lockDelay;

        private float stepDelay = 1f;
        private float _stepTime;
        private int _rotationIndex;
        
        public delegate void ActivePieceDelegate();

        public event ActivePieceDelegate LockEvent;

        public static ActivePiece CreateNewActivePiece(Vector3Int spawnPosition, TetrominoData data, Transform parent, Board.Board board, float stepDelay = 1f, float lockDelay = 0.5f) {
            var activePieceObject = new GameObject("ActivePiece")
            {
                transform =
                {
                    parent = parent
                }
            };
        
            var activePiece = activePieceObject.AddComponent<ActivePiece>();
            activePiece.Data = data;
            activePiece.position = spawnPosition;

            activePiece.Cells ??= new Vector3Int[data.Cells.Length];

            activePiece.Board = board;
            
            activePiece._stepTime = Time.time + stepDelay;
            activePiece.LockTime = 0f;
            activePiece._lockDelay = lockDelay;

            for (var i = 0; i < data.Cells.Length; i++) {
                activePiece.Cells[i] = (Vector3Int)data.Cells[i];
            }

            return activePiece;
        }
        
        public void Step()
        {
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
                DefaultStep();
            }   
        }
        
            
        private void DefaultStep() {
            _stepTime = Time.time + stepDelay;

            Move(Vector2Int.down);

            if (LockTime >= _lockDelay)
            {
                LockEvent?.Invoke();
            }
        }

        private bool Move(Vector2Int translation) {
            var newPosition = position;
            newPosition.x += translation.x;
            newPosition.y += translation.y;
            var valid = IsValidPiecePosition(newPosition);

            if (!valid) return false;
        
            position = newPosition;
            LockTime = 0f;

            return true;
        }

        /// <summary>
        /// Determine if piece is within bounds
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="newPosition"></param>
        /// <returns>bool</returns>
        public bool IsValidPiecePosition(Vector3Int newPosition)
        {
            // Loop through each tile in the Tetromino
            foreach (var tilePosition in Cells)
            {
                var newTilePosition = tilePosition + newPosition;
                if (!Board.IsValidPosition(newTilePosition))
                    return false;
            }

            return true;
        }
        

        private void HardDrop() {
            while ( Move(Vector2Int.down) ) {
            }
        }
        public void Activate()
        {
            throw new System.NotImplementedException();
        }

        public void Deactivate()
        {
            throw new System.NotImplementedException();
        }

        public void Terminate()
        {
            Destroy(gameObject);
        }

        public void Set(Tilemap tilemap, Tile toSet)
        {
            foreach (var cell in Cells)
            {
                var tilePosition = cell + position;
                tilemap.SetTile(tilePosition, toSet);
            }
        }

        public void Clear(Tilemap tilemap)
        {
            Set(tilemap, null);
        }
        
             
        // Rotation is quite complex, especially to achieve the Super Rotation System players don't realize they expect: https://tetris.fandom.com/wiki/SRS
        private void Rotate(int direction) {
            var originRotation = _rotationIndex;
            _rotationIndex = Helpers.Wrap(_rotationIndex + direction, 0, 4);

            // Rotate the piece
            ApplyRotationMatrix(direction);

            // Test if rotation would put piece out of bounds
            // Revert if tests failed
            if (TestWallKicks(_rotationIndex, direction)) return;
      
            _rotationIndex = originRotation;
            ApplyRotationMatrix(-direction);
        }
        
        // See https://en.wikipedia.org/wiki/Rotation_matrix
        // I and O piece have unique rotation requirements as per SRS
        private void ApplyRotationMatrix(int direction) {
            for (var i = 0; i < Cells.Length; i++) {
                Vector3 cell = Cells[i];
                int x, y;

                switch(Data.tetromino) {
                    case Tetromino.I:
                    case Tetromino.O:
                        cell.x -= 0.5f;
                        cell.y -= 0.5f;
                        x = Mathf.CeilToInt((cell.x * global::Tetris.Data.RotationMatrix[0] * direction) + (cell.y * global::Tetris.Data.RotationMatrix[1] * direction));
                        y = Mathf.CeilToInt((cell.x * global::Tetris.Data.RotationMatrix[2] * direction) + (cell.y * global::Tetris.Data.RotationMatrix[3] * direction));
                        break;
                    default:
                        x = Mathf.RoundToInt((cell.x * global::Tetris.Data.RotationMatrix[0] * direction) + (cell.y * global::Tetris.Data.RotationMatrix[1] * direction));
                        y = Mathf.RoundToInt((cell.x * global::Tetris.Data.RotationMatrix[2] * direction) + (cell.y * global::Tetris.Data.RotationMatrix[3] * direction));
                        break;
                }

                Cells[i] = new Vector3Int(x, y, 0);
            }
        }

        private bool TestWallKicks(int rotationIndex, int rotationDirection) {
            var wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        
            for (var i = 0; i < Data.WallKicks.GetLength(1); i++){
                var translation = Data.WallKicks[wallKickIndex, i];

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

            return Helpers.Wrap(wallKickIndex, 0, Data.WallKicks.GetLength(0));
        }
    }
}

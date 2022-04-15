using UnityEngine;

public class ActivePiece : MonoBehaviour
{
    public Board Board { get; private set; }
    public TetrominoData Data { get; private set; }

    public Vector3Int Position { get; private set; }
    public Vector3Int[] Cells { get; private set; }
    public int RotationIndex { get; private set; }

    public float stepDelay = 1f;
    public float lockDelay = 0.5f;

    private float _stepTime;
    private float _lockTime;
    private bool _isInitialized = false;

    public void Initialize(Vector3Int spawnPosition, TetrominoData data, Board board) {
        Board = board;
        Data = data;
        Position = spawnPosition;
        RotationIndex = 0;
        _stepTime = Time.time + stepDelay;
        _lockTime = 0f;

        if (Cells == null)
            Cells = new Vector3Int[data.Cells.Length];

        for (var i = 0; i < data.Cells.Length; i++) {
            Cells[i] = (Vector3Int)data.Cells[i];
        }

        _isInitialized = true;
    }

    public void Update() {
        if (_isInitialized) {
            Board.Clear(this);

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

            Board.Set(this);
        }
    }

    private void Step() {
        _stepTime = Time.time + stepDelay;

        Move(Vector2Int.down);

        if (_lockTime >= lockDelay) {
            Lock();
        }
    }

    private void Lock() {
        Board.Set(this);
        Board.CheckClearedLines();
        Board.SpawnPiece();
    }

    public void HardDrop() {
        while ( Move(Vector2Int.down) ) {
        continue;
        }
    }


    public bool Move(Vector2Int translation) {
        var newPosition = Position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;
        var valid = Board.IsValidPosition(this, newPosition);

        if ( valid ) {
            Position = newPosition;
            _lockTime = 0f;
        }

        return valid;
    }

  /// <summary>
  ///
  /// <para>
  /// Rotation is quite complex, especially to achieve the Super Rotation System players don't realize they expect: https://tetris.fandom.com/wiki/SRS
  /// </summary>
  /// <param name="direction">Represents index to move to. -1 or 1 expected</param>
    public void Rotate(int direction) {
      var originRotation = RotationIndex;
      RotationIndex = Helpers.Wrap(RotationIndex + direction, 0, 4);

      // Rotate the piece
      ApplyRotationMatrix(direction);

      // Test if rotation would put piece out of bounds
      // Revert if tests failed
      if (!TestWallKicks(RotationIndex, direction)) {
            RotationIndex = originRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    /// <summary>
    /// See https://en.wikipedia.org/wiki/Rotation_matrix
    /// <para>
    /// I and O piece have unique rotation requirements as per SRS
    /// </summary>
    /// <param name="direction">Represents index to move to. -1 or 1 expected</param>
    private void ApplyRotationMatrix(int direction) {
      for (var i = 0; i < Cells.Length; i++) {
        Vector3 cell = Cells[i];
        int x, y;

        switch(Data.tetromino) {
          case Tetromino.I:
          case Tetromino.O:
            cell.x -= 0.5f;
            cell.y -= 0.5f;
            x = Mathf.CeilToInt((cell.x * global::Data.RotationMatrix[0] * direction) + (cell.y * global::Data.RotationMatrix[1] * direction));
            y = Mathf.CeilToInt((cell.x * global::Data.RotationMatrix[2] * direction) + (cell.y * global::Data.RotationMatrix[3] * direction));
            break;
          default:
            x = Mathf.RoundToInt((cell.x * global::Data.RotationMatrix[0] * direction) + (cell.y * global::Data.RotationMatrix[1] * direction));
            y = Mathf.RoundToInt((cell.x * global::Data.RotationMatrix[2] * direction) + (cell.y * global::Data.RotationMatrix[3] * direction));
            break;
        }

        Cells[i] = new Vector3Int(x, y, 0);
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="rotationIndex"></param>
    /// <param name="rotationDirection"></param>
    /// <returns></returns>
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

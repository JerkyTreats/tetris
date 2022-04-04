using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }

    public Vector3Int position { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public int rotationIndex { get; private set; }

    public float stepDelay = 1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float lockTime;


    public void Initialize(Vector3Int spawnPosition, TetrominoData data, Board board) {
        this.board = board;
        this.data = data;
        this.position = spawnPosition;
        this.rotationIndex = 0;
        this.stepTime = Time.time + this.stepDelay;
        this.lockTime = 0f;

        if (this.cells == null)
            this.cells = new Vector3Int[data.cells.Length];

        for (int i = 0; i < data.cells.Length; i++) {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    public void Update() {
        this.board.Clear(this);

        this.lockTime += Time.deltaTime;

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

        if (Time.time >= this.stepTime) {
            Step();
        }

        this.board.Set(this);
    }

    private void Step() {
        this.stepTime = Time.time + this.stepDelay;

        Move(Vector2Int.down);

        if (this.lockTime >= this.lockDelay) {
            Lock();
        }
    }

    private void Lock() {
        this.board.Set(this);
        this.board.CheckClearedLines();
        this.board.SpawnPiece();
    }

    public void HardDrop() {
        while ( Move(Vector2Int.down) ) {
        continue;
        }
    }


    public bool Move(Vector2Int translation) {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;
        bool valid = this.board.IsValidPosition(this, newPosition);

        if ( valid ) {
            this.position = newPosition;
            this.lockTime = 0f;
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
      int originRotation = rotationIndex;
      rotationIndex = Helpers.Wrap(this.rotationIndex + direction, 0, 4);

      // Rotate the piece
      ApplyRotationMatrix(direction);

      // Test if rotation would put piece out of bounds
      // Revert if tests failed
      if (!TestWallKicks(this.rotationIndex, direction)) {
            this.rotationIndex = originRotation;
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
      for (int i = 0; i < this.cells.Length; i++) {
        Vector3 cell = this.cells[i];
        int x, y;

        switch(this.data.tetromino) {
          case Tetromino.I:
          case Tetromino.O:
            cell.x -= 0.5f;
            cell.y -= 0.5f;
            x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
            y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
            break;
          default:
            x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
            y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
            break;
        }

        this.cells[i] = new Vector3Int(x, y, 0);
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="rotationIndex"></param>
    /// <param name="rotationDirection"></param>
    /// <returns></returns>
    private bool TestWallKicks(int rotationIndex, int rotationDirection) {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < this.data.wallKicks.GetLength(1); i++){
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i];

            if (Move(translation)){
                return true;
            }
        }

        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection) {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0){
            wallKickIndex--;
        }

        return Helpers.Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0));
    }
}

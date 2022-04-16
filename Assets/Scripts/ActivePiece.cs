using UnityEngine;
using UnityEngine.Serialization;

public class ActivePiece : MonoBehaviour
{
    private Board Board { get; set; }
    public TetrominoData Data { get; private set; }
    public Vector3Int position;// { get; private set; }
    public Vector3Int[] Cells { get; private set; }

    public static ActivePiece CreateNewActivePiece(Vector3Int spawnPosition, TetrominoData data, Board board, float stepDelay = 1f, float lockDelay = 0.5f) {
        var activePieceObject = new GameObject("ActivePiece")
        {
            transform =
            {
                parent = board.transform
            }
        };
        
        var activePiece = activePieceObject.AddComponent<ActivePiece>();
        activePiece.Board = board;
        activePiece.Data = data;
        activePiece.position = spawnPosition;

        activePiece.Cells ??= new Vector3Int[data.Cells.Length];

        for (var i = 0; i < data.Cells.Length; i++) {
            activePiece.Cells[i] = (Vector3Int)data.Cells[i];
        }

        return activePiece;
    }

    /// <summary>
    /// See https://en.wikipedia.org/wiki/Rotation_matrix
    /// I and O piece have unique rotation requirements as per SRS
    /// </summary>
    /// <param name="direction">Represents index to move to. -1 or 1 expected</param>
    public void ApplyRotationMatrix(int direction) {
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
}

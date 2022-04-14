using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Originally pulled from: https://docs.unity3d.com/2019.1/Documentation/ScriptReference/UI.Button-onClick.html
/// </summary>
public class NewBoard : MonoBehaviour
{
    //Make sure to attach these Buttons in the Inspector
    public Button newBoardButton, saveBoardButton, loadBoardButton;

    // TODO Make this some serialized file.
    // Duplicated from BoardManager
    public BoardData board1 = new BoardData {
        spawnPosition = new Vector3Int(-1, 7, 0),
        cameraPosition = new Vector3Int(0, 0, -10),
        boardPosition = new Vector3Int(0, 0, 0),
        boardSize = new Vector2Int(10,20),
        sortOrder = 2,
    };

    void Start()
    {
        newBoardButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Board board = Board.Initialize(board1);
        board.boardCamera.ActivateCamera();
    }
}

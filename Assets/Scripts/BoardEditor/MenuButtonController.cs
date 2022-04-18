using Board;
using UnityEngine;
using UnityEngine.UI;

namespace BoardEditor
{
    /// <summary>
    /// Originally pulled from: https://docs.unity3d.com/2019.1/Documentation/ScriptReference/UI.Button-onClick.html
    /// </summary>
    public class MenuButtonController : MonoBehaviour
    {
        //Make sure to attach these Buttons in the Inspector
        public Button newBoardButton, saveBoardButton, loadBoardButton;

        // TODO Serial to file nephew.
        // Duplicated from BoardManager
        public BoardData board1 = new BoardData {
            spawnPosition = new Vector3Int(-1, 7, 0),
            cameraPosition = new Vector3Int(0, 0, -10),
            boardPosition = new Vector3Int(0, 0, 0),
            boardSize = new Vector2Int(10,20),
            sortOrder = 2,
        };

        private void Start()
        {
            newBoardButton.onClick.AddListener(TaskOnClick);
        }

        private void TaskOnClick()
        {
            BoardFactory.CreateNewBoard(board1);
        }
    }
}
using Board;
using Board.Persistence;
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

        // TODO Serialize to file nephew.
        // Duplicated from BoardManager
        public BoardData board1 = new BoardData(
            new Vector3Int(-1, 7, 0),
            new Vector3Int(0, 0, -10),
            new Vector3Int(0, 0, 0),
            new Vector2Int(10,20),
            2
        );

        private void Start()
        {
            newBoardButton.onClick.AddListener(TaskOnClick);
        }

        private void TaskOnClick()
        {
            BoardFactory.CreateNewBoard(board1);
        }

        private void CreateBoard()
        {
            Board.Board.CreateNewBoard(board1);
        }
        
        
    }
}
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
        private Board.Board _board;
        private BoardRepository _boardRepo;
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
            newBoardButton.onClick.AddListener(CreateBoard);
            saveBoardButton.onClick.AddListener(SaveBoard);
        }

        private void SaveBoard()
        {
            if (!_boardRepo)
                _boardRepo = ScriptableObject.CreateInstance<BoardRepository>();
            
            _boardRepo.Create(_board.data);
            
        }

        private void CreateBoard()
        {
            _board = Board.Board.CreateNewBoard(board1);
        }
        
        
    }
}
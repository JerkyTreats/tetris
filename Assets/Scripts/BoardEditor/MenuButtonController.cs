using System.ComponentModel;
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
        
        public delegate void MenuButtonControllerDelegate(MenuButtonController menuButtonController);
        public event MenuButtonControllerDelegate CreateBoardEvent;

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
            loadBoardButton.onClick.AddListener(LoadBoard);
        }

        private void CreateBoard()
        {
            Clear();
            _board = Board.Board.CreateNewBoard(board1);
            CreateBoardEvent?.Invoke(this);
        }

        private void SaveBoard()
        {
            Clear();
            if (!_boardRepo)
                _boardRepo = ScriptableObject.CreateInstance<BoardRepository>();
            
            _boardRepo.Create(_board.data);
            
        }

        private void LoadBoard()
        {
            Clear();
            if (!_boardRepo)
                _boardRepo = ScriptableObject.CreateInstance<BoardRepository>();

            var data = _boardRepo.Read();
            _board = Board.Board.CreateNewBoard(data);
        }
        
        
        private static void Clear()
        {
            var boards = FindObjectsOfType<Board.Board>();
            foreach (var board in boards)
            {
                Destroy(board.gameObject);
            }
        }
    }
}
using System;
using Board.Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoardEditor
{
    public class NewBoardContextController : MonoBehaviour, IContextMenu<MenuButtonController>
    {
        private Board.Board _board;
        private CanvasGroup _canvasGroup;
        private static Camera _mainCam;

        // TODO Serialize to file nephew.
        // Duplicated from BoardManager
        private BoardData _defaultBoard = new BoardData(
            new Vector3Int(-1, 7, 0),
            new Vector3Int(0, 0, -10),
            new Vector3Int(0, 0, 0),
            new Vector2Int(10,20),
            2
        );

        public Button createNewBoardButton;
        public Button cancelNewBoardButton;
        public TMP_InputField boardHeight;
        public TMP_InputField boardWidth;

        public delegate void NewBoardContextControllerDelegate(NewBoardContextController newBoardContextController);
        public event NewBoardContextControllerDelegate CreateBoardEvent;
        
        private void Awake()
        {
            _mainCam = FindObjectOfType<Camera>();
            _canvasGroup = GetComponent<CanvasGroup>();
            createNewBoardButton.onClick.AddListener(CreateBoard);
            cancelNewBoardButton.onClick.AddListener(Disable);
            
            // Register the menu button event then disable 
            var menuButtonController = FindObjectOfType<MenuButtonController>();
            menuButtonController.NewBoardEvent += Enable;
            Disable(); 
        }

        public void Enable(MenuButtonController _)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
        }

        //
        private void CreateBoard()
        {
            Clear();
            
            var boardSize = GetBoardSize();
            _defaultBoard.boardSize = boardSize;
            
            _board = Board.Board.CreateNewBoard(_defaultBoard);
            _board.boardCamera.ActivateCamera();
            
            CreateBoardEvent?.Invoke(this);
            Disable();
        }

        // TODO Magic numbers
        // Parse the input boxes for width/height or use default
        private Vector2Int GetBoardSize()
        {
            var defaultWidth = 10;
            var defaultHeight = 20;

            var widthSuccess = Int32.TryParse(boardWidth.text, out var width);
            var heightSuccess = Int32.TryParse(boardHeight.text, out var height);
            
            width = widthSuccess ? width : defaultWidth;
            height = heightSuccess ? height : defaultHeight;
            
            return new Vector2Int(width , height);
            
        }

        private void Disable()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }

        private static void Clear()
        {
            var boards = FindObjectsOfType<Board.Board>();
            foreach (var board in boards)
            {
                Destroy(board.gameObject);
            }

            _mainCam.enabled = true;
            _mainCam.gameObject.SetActive(true);
        }
    }
}

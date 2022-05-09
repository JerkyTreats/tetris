using System;
using System.Collections.Generic;
using Board.Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoardEditor
{
    /// <summary>
    /// Controller for the NewBoardContext menu, controlling settings for creating a new Board
    /// </summary>
    public class NewBoardController : ModalController
    {
        private BoardEditorBoardFactory _boardEditorBoardFactory;

        [SerializeField] private Button createNewBoardButton;
        [SerializeField] private Button cancelNewBoardButton;
        [SerializeField] private TMP_InputField boardHeight;
        [SerializeField] private TMP_InputField boardWidth;
        
        // TODO NewBoardContextController - Fix default BoardData location 
        // A case could be made this should be right up into GameData.asset
        // This was duplicated from BoardManager
        // Can be considered a "default Tetris board"
        private BoardData _defaultBoard = new BoardData(
            new Vector3Int(0, 0, -10),
            new Vector3Int(0, 0, 0),
            new Vector2Int(10,20),
            2,
            new List<BoardTileData>()
        );

        public delegate void NewBoardControllerDelegate(Board.Board board);
        public event NewBoardControllerDelegate NewBoardEvent;
        
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            createNewBoardButton.onClick.AddListener(CreateBoard);
            cancelNewBoardButton.onClick.AddListener(Disable);

            // Register the menu button event then disable 
            var menuButtonController = FindObjectOfType<MenuButtonController>();
            menuButtonController.NewBoardMenuClickEvent += Enable;
            Disable(); 
        }
        
        // Clear any existing board
        // Grab boardsize from input
        // Spawn a new Board
        // Trigger NewBoard Event
        // Dismiss this menu
        private void CreateBoard()
        {
            UIHelpers.Clear();
            
            var boardSize = GetBoardSize();
            _defaultBoard.boardSize = boardSize;
            
            var canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            _boardEditorBoardFactory = new BoardEditorBoardFactory(canvasRectTransform.rect);
            var board = _boardEditorBoardFactory.CreateNewBoard(_defaultBoard);

            NewBoardEvent?.Invoke(board);
            Disable();
        }

        // TODO NewBoardContextController - Fix BoardSize Magic numbers
        // Parse the input boxes for width/height or use default
        private Vector2Int GetBoardSize()
        {
            const int defaultWidth = 10;
            const int defaultHeight = 20;

            var widthSuccess = Int32.TryParse(boardWidth.text, out var width);
            var heightSuccess = Int32.TryParse(boardHeight.text, out var height);

            // Valid int in input field else default 
            width = widthSuccess ? width : defaultWidth;
            height = heightSuccess ? height : defaultHeight;

            return new Vector2Int(width, height);
        }
    }
}

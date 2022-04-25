using System;
using Board.Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoardEditor
{
    /// <summary>
    /// Controller for the NewBoardContext menu, controlling settings for creating a new Board
    /// </summary>
    public class NewBoardContextController : MonoBehaviour, IContextMenu<MenuButtonController>
    {
        private BoardEditorBoardFactory _boardEditorBoardFactory;
        public Board.Board ActiveBoard { get; private set; }

        private CanvasGroup _canvasGroup;

        [SerializeField] private Button createNewBoardButton;
        [SerializeField] private Button cancelNewBoardButton;
        [SerializeField] private TMP_InputField boardHeight;
        [SerializeField] private TMP_InputField boardWidth;
        
        // TODO NewBoardContextController - Fix default BoardData location 
        // A case could be made this should be right up into GameData.asset
        // This was duplicated from BoardManager
        // Can be considered a "default Tetris board"
        private BoardData _defaultBoard = new BoardData(
            new Vector3Int(-1, 7, 0),
            new Vector3Int(0, 0, -10),
            new Vector3Int(0, 0, 0),
            new Vector2Int(10,20),
            2
        );

        public delegate void NewBoardContextControllerDelegate(NewBoardContextController newBoardContextController);
        public event NewBoardContextControllerDelegate CreateBoardEvent;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            createNewBoardButton.onClick.AddListener(CreateBoard);
            cancelNewBoardButton.onClick.AddListener(Disable);

            // Register the menu button event then disable 
            var menuButtonController = FindObjectOfType<MenuButtonController>();
            menuButtonController.NewBoardEvent += Enable;
            Disable(); 
        }

        /// <summary>
        /// Makes the UI element appear and functional
        /// </summary>
        public void Enable(MenuButtonController _)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
        }
        
        // Make board invisible + nonfunctional
        private void Disable()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }

        private void CreateBoard()
        {
            UIHelpers.Clear(); // Remove any existing board
            
            var boardSize = GetBoardSize(); // Boardsize by input
            _defaultBoard.boardSize = boardSize;
            
            var canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            _boardEditorBoardFactory = new BoardEditorBoardFactory(canvasRectTransform.rect);
            ActiveBoard = _boardEditorBoardFactory.CreateNewBoard(_defaultBoard);

            CreateBoardEvent?.Invoke(this);
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

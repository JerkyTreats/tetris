using Board.Persistence;
using UnityEngine;
using UnityEngine.UI;

namespace BoardEditor
{
    /// <summary>
    /// Controller for functionality available for the main BoardEditor Menu
    /// </summary>
    public class MenuButtonController : MonoBehaviour
    {
        private BoardLocalFileRepository _boardLocalFileRepo;
        private NewBoardController _newBoardController;
        private LoadBoardController _loadBoardController;
        private SaveBoardController _saveBoardController;

        [SerializeField] private Button newBoardButton;
        [SerializeField] private Button saveBoardButton;
        [SerializeField] private Button loadBoardButton;

        public Board.Board ActiveBoard { get; set; }
        
        // Fire events when menu item clicked
        public delegate void MenuButtonControllerDelegate();
        public event MenuButtonControllerDelegate NewBoardMenuClickEvent, LoadBoardMenuClickEvent, SaveBoardMenuClickEvent;
        
        // Fire events if the "active" board is changed
        public delegate void ActiveBoardDelegate(Board.Board board);
        public event ActiveBoardDelegate ActivateBoardEvent;

        private void Awake()
        {
            newBoardButton.onClick.AddListener(NewBoard);
            saveBoardButton.onClick.AddListener(SaveBoard);
            loadBoardButton.onClick.AddListener(LoadBoard);
        }

        private void Start()
        {
            
            _newBoardController = FindObjectOfType<NewBoardController>();
            _newBoardController.NewBoardEvent += ActivateBoard;
            
            _loadBoardController = FindObjectOfType<LoadBoardController>();
            _loadBoardController.LoadBoardEvent += ActivateBoard;

            saveBoardButton.interactable = false;
        }

        private void NewBoard()
        {
            NewBoardMenuClickEvent?.Invoke();
        }

        private void SaveBoard()
        {
            SaveBoardMenuClickEvent?.Invoke();
        }

        private void LoadBoard()
        {
            LoadBoardMenuClickEvent?.Invoke();
        }

        private void ActivateBoard(Board.Board board)
        {
            ActiveBoard = board;
            ActivateBoardEvent?.Invoke(board);
            
            saveBoardButton.interactable = true;
        }

    }
}
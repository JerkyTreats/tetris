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

        // New/Load modals will serve up a new board
        // Capture that event and push a new event
        // Acts as a central source for any resource dependent on a board 
        // TODO MenuButtonController: ActivateBoard - Consider separate class 
        // Have MenuButtonController be just another subscriber to the event
        // It will reduce scope of the menu controller to only be concerned with the menu
        private void ActivateBoard(Board.Board board)
        {
            ActivateBoardEvent?.Invoke(board);
            saveBoardButton.interactable = true;
        }

    }
}
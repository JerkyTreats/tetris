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
        private BoardRepository _boardRepo;
        private NewBoardContextController _newBoardContextController;

        private Board.Board ActiveBoard { get; set; }

        //Make sure to attach these Buttons in the Inspector
        [SerializeField] private Button newBoardButton;
        [SerializeField] private Button saveBoardButton;
        [SerializeField] private Button loadBoardButton;
        
        public delegate void MenuButtonControllerDelegate(MenuButtonController menuButtonController);
        public event MenuButtonControllerDelegate NewBoardEvent, LoadBoardEvent;

        private void Start()
        {
            newBoardButton.onClick.AddListener(NewBoard);
            saveBoardButton.onClick.AddListener(SaveBoard);
            loadBoardButton.onClick.AddListener(LoadBoard);
            
            var newBoardContextController = FindObjectOfType<NewBoardContextController>();
            newBoardContextController.CreateBoardEvent += SetBoard;
        }

        private void NewBoard()
        {
            NewBoardEvent?.Invoke(this);
        }

        private void SaveBoard()
        {
            if (!_boardRepo)
                _boardRepo = ScriptableObject.CreateInstance<BoardRepository>();

            _boardRepo.Create(ActiveBoard.data);
        }

        private void LoadBoard()
        {
            LoadBoardEvent?.Invoke(this);
        }

        private void SetBoard(NewBoardContextController newBoardContextController)
        {
            ActiveBoard = newBoardContextController.ActiveBoard;
        }
    }
}
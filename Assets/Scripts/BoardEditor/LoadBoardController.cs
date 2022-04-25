using Board.Persistence;
using UnityEngine;
using UnityEngine.UI;

namespace BoardEditor
{
    [RequireComponent(typeof(CanvasGroup)) ]
    public class LoadBoardController : MonoBehaviour, IContextMenu<MenuButtonController>
    {
        private CanvasGroup _canvasGroup;
        private BoardRepository _boardRepo;
        private Board.Board _board;

        [SerializeField] private Button loadBoardButton;
        [SerializeField] private Button cancelButton;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            loadBoardButton.onClick.AddListener(LoadBoard);
            cancelButton.onClick.AddListener(Disable);

            // Register the menu button event then disable 
            var menuButtonController = FindObjectOfType<MenuButtonController>();
            menuButtonController.LoadBoardEvent += Enable;
            Disable(); 
        }

        private void LoadBoard()
        {
            UIHelpers.Clear();
            if (!_boardRepo)
                _boardRepo = ScriptableObject.CreateInstance<BoardRepository>();

            var data = _boardRepo.Read();
            _board = Board.Board.CreateNewBoard(data);
            _board.BoardCamera.ActivateCamera();
            
            Disable();
        }

        public void Enable(MenuButtonController eventController)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
        }

        private void Disable()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}

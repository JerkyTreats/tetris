using System;
using Board.Persistence;
using Persistence;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace BoardEditor
{
    [RequireComponent(typeof(CanvasGroup)) ]
    public class LoadBoardController : MonoBehaviour, IContextMenu
    {
        private CanvasGroup _canvasGroup;
        private BoardRepository _boardRepo;

        private BoardRepository BoardRepo
        {
            get
            {
                if (!_boardRepo)
                    _boardRepo = ScriptableObject.CreateInstance<BoardRepository>();
                return _boardRepo;
            }
        }
        private Board.Board _board;

        [SerializeField] private GameObject loadListPanel;
        [SerializeField] private GameObject loadSelectionPanel;
        [SerializeField] private Button loadBoardButton;
        [SerializeField] private Button cancelButton;
        
        // public event Action<NewBoardContextController> LoadBoardEvent;
        public delegate void LoadBoardControllerDelegate();
        public event LoadBoardControllerDelegate LoadBoardEvent;

        public string SelectedFile { get; set; }

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

            var data = BoardRepo.Read(SelectedFile);
            var factory = new BoardEditorBoardFactory(GetComponentInParent<RectTransform>().rect);
            _board = factory.CreateNewBoard(data);
            _board.BoardCamera.ActivateCamera();
            
            LoadBoardEvent?.Invoke();
            
            Disable();
        }

        public void Enable()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;

            PopulateLoadList();
        }

        private void PopulateLoadList()
        {
            foreach (var path in BoardRepo.GetSavedFiles())
            {
                var selectionObject = Instantiate(loadSelectionPanel, loadListPanel.transform);
                
                var selection = selectionObject.GetComponent<LoadBoardSelectionController>();
                selection.FileName = path;

                selection.SelectEvent += LoadBoardSelected;
            }
        }

        private void LoadBoardSelected(LoadBoardSelectionController loadBoardSelectionController)
        {
            SelectedFile = loadBoardSelectionController.FileName;
        }


        private void Disable()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
            
            DestroyAllChildren();
        }

        private void DestroyAllChildren()
        {
            var list = GetComponentsInChildren<LoadBoardSelectionController>();

            foreach (var selector in list)
            {
                Destroy(selector.gameObject);
            }
        }
    }
}

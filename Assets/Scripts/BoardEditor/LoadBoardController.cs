using Board.Persistence;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace BoardEditor
{
    [RequireComponent(typeof(CanvasGroup)) ]
    public class LoadBoardController : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private BoardLocalFileRepository _boardLocalFileRepo;

        private BoardLocalFileRepository BoardLocalFileRepo
        {
            get
            {
                if (!_boardLocalFileRepo)
                    _boardLocalFileRepo = ScriptableObject.CreateInstance<BoardLocalFileRepository>();
                return _boardLocalFileRepo;
            }
        }

        [SerializeField] private GameObject loadListPanel;
        [SerializeField] private GameObject loadSelectionPanel;
        [SerializeField] private Button loadBoardButton;
        [SerializeField] private Button cancelButton;
        
        // public event Action<NewBoardContextController> LoadBoardEvent;
        public delegate void LoadBoardControllerDelegate(Board.Board board);
        public event LoadBoardControllerDelegate LoadBoardEvent;

        private string SelectedFile { get; set; }

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            loadBoardButton.onClick.AddListener(LoadBoard);
            cancelButton.onClick.AddListener(Disable);

            // Register the menu button event then disable 
            var menuButtonController = FindObjectOfType<MenuButtonController>();
            menuButtonController.LoadBoardMenuClickEvent += Enable;
            
            Disable(); 
        }

        private void LoadBoard()
        {
            UIHelpers.Clear();

            var data = BoardLocalFileRepo.Read(SelectedFile);
            var factory = new BoardEditorBoardFactory(GetComponentInParent<RectTransform>().rect);
            var board = factory.CreateNewBoard(data);
            board.BoardCamera.ActivateCamera();
            
            LoadBoardEvent?.Invoke(board);
            
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
            foreach (var path in BoardLocalFileRepo.GetSavedFiles())
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

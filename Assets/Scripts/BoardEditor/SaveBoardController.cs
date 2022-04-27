using System;
using System.IO;
using Board.Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoardEditor
{
    /// <summary>
    ///  Modal menu controller for saving an active Board
    /// </summary>
    public class SaveBoardController : ModalController
    {        
        private BoardLocalFileRepository _boardLocalFileRepo;
        private Board.Board _activeBoard;

        // TODO : BoardEditor LocalFileRepo - Consider just making it a singleton
        private BoardLocalFileRepository BoardLocalFileRepo
        {
            get
            {
                if (!_boardLocalFileRepo)
                    _boardLocalFileRepo = ScriptableObject.CreateInstance<BoardLocalFileRepository>();
                return _boardLocalFileRepo;
            }
        }

        [SerializeField] private TMP_InputField saveBoardInputBox;
        [SerializeField] private Button saveBoardButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private TMP_Text currentSaveNameField;
        [SerializeField] private GameObject currentSaveNamePanel;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>(); // See base class
            
            saveBoardButton.onClick.AddListener(SaveBoard);
            cancelButton.onClick.AddListener(Disable);
            saveBoardInputBox.onValueChanged.AddListener(UpdateBoardName);

            var menuButtonController = FindObjectOfType<MenuButtonController>();
            menuButtonController.SaveBoardMenuClickEvent += Enable;
            menuButtonController.ActivateBoardEvent += ActivateBoard;

            saveBoardButton.interactable = false;
            currentSaveNamePanel.SetActive(false);
            
            Disable(); 
        }
        
        // Save the ActiveBoard to disk 
        // Dismiss this menu
        private void SaveBoard()
        {
            // Not sure I love this guarding, but ended up needing it whiling dev-ing this feature 
            if (!_activeBoard) { Debug.LogError("Missing ActiveBoard on BoardEditor Save"); return; }
            
            BoardLocalFileRepo.Create(_activeBoard.data);
            
            Disable();
        }
        
        // Don't let the user save the board if there isn't a Board loaded
        private void ActivateBoard(Board.Board board)
        {
            _activeBoard = board;
            
            if (board.data.userSavedBoardName == "") return;
            
            saveBoardButton.interactable = true;
            saveBoardInputBox.text = board.data.userSavedBoardName;
            ActivateCurrentSaveNamePanel(board.data.userSavedBoardName);
        }

        // User defined Board name. Update UI elements to provide visual feedback for the changing name
        private void UpdateBoardName(string newBoardName)
        {
            newBoardName = ValidateAndFixBoardName(newBoardName);
            _activeBoard.data.userSavedBoardName = newBoardName;
            saveBoardButton.interactable = true;

            ActivateCurrentSaveNamePanel(newBoardName);
        }

        // The panel that describes the current save name should only appear if there is a name
        // The panel should update the current save name as the name is updated
        private void ActivateCurrentSaveNamePanel(string saveName)
        {
            currentSaveNamePanel.SetActive(true);
            currentSaveNameField.text = saveName;
        }
        
        // TODO : SaveBoardController : InputField should be validating Save filenames
        // This is just a hack because I dont want to learn about TMP_InputField validators.
        // They exist and are documented, but I am lazy on this one.
        // I dont think this even works
        private string ValidateAndFixBoardName(string newBoardName)
        {
            var badChars = Path.GetInvalidFileNameChars();
            foreach (var badChar in badChars)
            {
                newBoardName = newBoardName.Replace(badChar.ToString(), "");
            }

            if (newBoardName == "")
                newBoardName = "default"; 

            return newBoardName;
        }
    }
}

using System;
using System.IO;
using Board.Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoardEditor
{
    [RequireComponent(typeof(CanvasGroup)) ]
    public class SaveBoardController : MonoBehaviour
    {        
        private CanvasGroup _canvasGroup;
        private BoardLocalFileRepository _boardLocalFileRepo;
        private Board.Board _activeBoard;

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
            _canvasGroup = GetComponent<CanvasGroup>();
            saveBoardButton.onClick.AddListener(SaveBoard);
            cancelButton.onClick.AddListener(Disable);
            saveBoardInputBox.onValueChanged.AddListener(UpdateBoardName);

            // Register the menu button event then disable 
            var menuButtonController = FindObjectOfType<MenuButtonController>();
            menuButtonController.SaveBoardMenuClickEvent += Enable;
            menuButtonController.ActivateBoardEvent += ActivateBoard;

            saveBoardButton.interactable = false;
            currentSaveNamePanel.SetActive(false);
            
            Disable(); 
        }
        
        private void SaveBoard()
        {
            if (!_activeBoard) { Debug.LogError("Missing ActiveBoard on BoardEditor Save"); return; }
            
            BoardLocalFileRepo.Create(_activeBoard.data);
            
            Disable();
        }
        
        private void ActivateBoard(Board.Board board)
        {
            _activeBoard = board;
            
            if (board.data.userSavedBoardName == "") return;
            
            saveBoardButton.interactable = true;
            saveBoardInputBox.text = board.data.userSavedBoardName;
            ActivateCurrentSaveNamePanel(board.data.userSavedBoardName);
        }

        private void UpdateBoardName(string newBoardName)
        {
            newBoardName = ValidateAndFixBoardName(newBoardName);
            _activeBoard.data.userSavedBoardName = newBoardName;
            saveBoardButton.interactable = true;

            ActivateCurrentSaveNamePanel(newBoardName);
        }
        
        // TODO : SaveBoardController : InputField should be validating Save filenames
        // This is just a hack because I dont want to learn about TMP_InputField validators.
        // They exist and are documented, but I am lazy on this one.
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

        private void ActivateCurrentSaveNamePanel(string saveName)
        {
            currentSaveNamePanel.SetActive(true);
            currentSaveNameField.text = saveName;
        }

        private void Enable()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
        }

        private void Disable()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
            saveBoardButton.interactable = false;
            currentSaveNamePanel.SetActive(false);
        }
    }
}

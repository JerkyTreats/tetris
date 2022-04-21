using System.ComponentModel;
using Board.Persistence;
using UnityEngine;
using UnityEngine.UI;

namespace BoardEditor
{
    /// <summary>
    /// Originally pulled from: https://docs.unity3d.com/2019.1/Documentation/ScriptReference/UI.Button-onClick.html
    /// </summary>
    public class MenuButtonController : MonoBehaviour
    {
        private static Camera _mainCam;
        private BoardRepository _boardRepo;
        //Make sure to attach these Buttons in the Inspector
        public Button newBoardButton, saveBoardButton, loadBoardButton;
        
        public delegate void MenuButtonControllerDelegate(MenuButtonController menuButtonController);
        public event MenuButtonControllerDelegate NewBoardEvent;

        private void Start()
        {
            _mainCam = FindObjectOfType<Camera>();
            newBoardButton.onClick.AddListener(NewBoard);
            saveBoardButton.onClick.AddListener(SaveBoard);
            loadBoardButton.onClick.AddListener(LoadBoard);
        }

        private void NewBoard()
        {
            NewBoardEvent?.Invoke(this);
        }

        // TODO review if findobject can be improved here
        private void SaveBoard()
        {
            Clear();
            if (!_boardRepo)
                _boardRepo = ScriptableObject.CreateInstance<BoardRepository>();

            var board = FindObjectOfType<Board.Board>();
            _boardRepo.Create(board.data);
            
        }

        // TODO cycle back here and fix this 
        private void LoadBoard()
        {
            Clear();
            if (!_boardRepo)
                _boardRepo = ScriptableObject.CreateInstance<BoardRepository>();

            var data = _boardRepo.Read();
            Board.Board.CreateNewBoard(data);
        }

        // TODO Fix with NewBoardContext
        private static void Clear()
        {
            var boards = FindObjectsOfType<Board.Board>();
            foreach (var board in boards)
            {
                Destroy(board.gameObject);
            }
            
            _mainCam.enabled = true;
            _mainCam.gameObject.SetActive(true);
        }
    }
}
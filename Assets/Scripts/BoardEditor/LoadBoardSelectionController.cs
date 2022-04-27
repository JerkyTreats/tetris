using TMPro;
using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BoardEditor
{
    /// <summary>
    /// Controller for an individual element in the Load Board list of saves
    /// </summary>
    public class LoadBoardSelectionController : MonoBehaviour, ISelectHandler
    {
        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set => SetFileName(value);
        }

        public delegate void LoadBoardSelectionDelegate(LoadBoardSelectionController loadBoardSelectionController);
        public event LoadBoardSelectionDelegate SelectEvent;

        private Selectable _selectable;
        private TMP_Text _fileTextBox;
        
        private void Awake()
        {
            _fileTextBox = GetComponentInChildren<TMP_Text>();
            _selectable = GetComponent<Selectable>();
        }

        // When this element is selected, fire an event 
        public void OnSelect(BaseEventData eventData)
        {
            SelectEvent?.Invoke(this);
        }
        
        private void SetFileName(string value)
        {
            _fileName = value;
            _fileTextBox.text = value;
        }
    }
}

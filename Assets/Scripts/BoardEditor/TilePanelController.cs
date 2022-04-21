using System;
using UnityEditor;
using UnityEngine;

namespace BoardEditor
{
    /// <summary>
    /// Controller for the BoardEditor UI element "TilePanel"
    /// </summary>
    public class TilePanelController : MonoBehaviour, IContextMenu<NewBoardContextController>
    {
        private CanvasGroup _canvasGroup;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            // Register the menu button event then disable 
            var newBoardContextController = FindObjectOfType<NewBoardContextController>();
            newBoardContextController.CreateBoardEvent += Enable;
            Disable(); 
        }

        // Enable the TilePanel by event
        public void Enable(NewBoardContextController _)
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
using System;
using UnityEditor;
using UnityEngine;

namespace BoardEditor
{
    /// <summary>
    /// Controller for the BoardEditor UI element "TilePanel"
    /// </summary>
    public class TilePanelController : ModalController
    {
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            // Register the menu button event then disable 
            var newBoardContextController = FindObjectOfType<NewBoardController>();
            newBoardContextController.NewBoardEvent += Enable;
            
            var loadBoardContextController = FindObjectOfType<LoadBoardController>();
            loadBoardContextController.LoadBoardEvent += Enable;
            Disable(); 
        }

        // Enable the tilepanel when a Board is active on screen
        private void Enable(Board.Board _)
        {
            base.Enable();
        }
    }
}
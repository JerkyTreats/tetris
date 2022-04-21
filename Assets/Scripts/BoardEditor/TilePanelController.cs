using System;
using UnityEditor;
using UnityEngine;

namespace BoardEditor
{
    /// <summary>
    /// Controller for the BoardEditor UI element "TilePanel"
    /// </summary>
    public class TilePanelController : MonoBehaviour
    {
        private void Awake()
        {
            // Register the menu button event then disable 
            var menuButtonController = FindObjectOfType<MenuButtonController>();
            menuButtonController.CreateBoardEvent += Enable;
            Disable(); 
        }

        // Enable the TilePanel by event
        private void Enable(MenuButtonController _)
        {
            gameObject.SetActive(true);
        }

        private void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
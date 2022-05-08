using System;
using Tetris;
using UnityEngine;

namespace GameManagement
{
    /// <summary>
    /// Creates and Initializes GameControllers 
    /// </summary>
    public static class GameControllerInitializer
    {

        /// <summary>
        /// Create and Initialize specific GameController type. 
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IGameController InitializeGameController(GameManager manager, IGameControllerData data)
        {
            // Create the GameObject
            var controllerObj = new GameObject(data.GameObjectName)
            {
                transform =
                {
                    parent = manager.transform
                }
            };

            // There's probably a way to do this with reflection but I dont like it
            // I don't like this implementation either but I need a place to convert IGameController into its true types. 
            switch (data.GameControllerType)
            {
                case GameControllerType.TetrisClassic:
                    var controller = controllerObj.AddComponent<TetrisClassicController>();
                    controller.Data = (TetrisClassicData)data;
                    InitializeController(controller, manager);
                    return controller;
                default:
                    Debug.LogError($"Unknown GameController type: [{data.GameControllerType}]");
                    return null;
            }
        }

        // Initialize the Controller
        private static void InitializeController(IGameController controller, GameManager manager)
        {
            if (controller.Data.Guid == Guid.Empty)
                controller.Data.Guid = Guid.NewGuid();
            
            controller.Initialize(manager);
        }
        
    }
}
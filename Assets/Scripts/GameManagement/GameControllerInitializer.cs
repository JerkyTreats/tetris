using System;
using Tetris;
using UnityEngine;

namespace GameManagement
{
    public static class GameControllerInitializer
    {

        public static IGameController InitializeGameController(GameManager manager, IGameControllerData data)
        {
            var controllerObj = new GameObject(data.GameObjectName)
            {
                transform =
                {
                    parent = manager.transform
                }
            };

            switch (data.GameControllerType)
            {
                case GameControllerType.TetrisClassic:
                    var controller = controllerObj.AddComponent<TetrisClassicController>();
                    controller.Data = (TetrisClassicData)data;
                    controller.Data.Guid = Guid.NewGuid();
                    controller.Initialize(manager);
                    return controller;
                default:
                    Debug.LogError("Unknown GameController type: " + data.GameControllerType);
                    return null;
            }

        }
        
    }
}
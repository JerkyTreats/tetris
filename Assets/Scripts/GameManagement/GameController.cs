using System;
using UnityEngine;

namespace GameManagement
{
    public class GameController : MonoBehaviour, IGameController
    {
        public bool IsInitialized { get; private set; }
        
        public delegate void GameControllerDelegate(GameController controller);
        public event GameControllerDelegate GameStarted, GameEnded, GameUpdate;

        public static GameController CreateNewGameController(GameControllerData data, GameManager manager)
        {
            // Create the game controller object
            var gameControllerObj = new GameObject(data.ControllerName)
            {
                transform =
                {
                    parent = manager.transform
                }
            };
                
            // Initialize the controller
            var gameController = gameControllerObj.AddComponent<GameController>();
            gameController.Initialize(data, manager);

            return gameController;
        }
        
        public void Initialize(GameControllerData gameControllerData, GameManager manager)
        {
            // Register manager events 
            manager.GameStart += GameStart;
            manager.GameEnd += GameEnd;
            manager.Terminate += Terminate;
            manager.Interrupt += Interrupt;

            IsInitialized = true;
        }

        public void GameStart()
        {
            GameStarted?.Invoke(this);
        }

        public void GameEnd()
        {
            GameEnded?.Invoke(this);
        }

        public void Terminate() { }

        public void Interrupt() { }
    }
}
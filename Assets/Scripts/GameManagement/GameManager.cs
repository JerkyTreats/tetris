using System;
using System.Collections;
using UnityEngine;

namespace GameManagement
{
    public class GameManager : MonoBehaviour
    {
        public GameManagerData Data { get; private set; }

        private IGameController[] _gameControllers;
        
        private int _numGameControllers;

        private int _activeGame;

        public delegate void GameManagerDelegate(IGameController controller);

        public event GameManagerDelegate GameStart, GameEnd;//, Interrupt, Terminate;
        
        public void Initialize(GameManagerData managerData)
        {
            Data = managerData;
            _numGameControllers = ((ICollection) Data.GameControllerData).Count;
            _gameControllers = new IGameController[_numGameControllers];

            // Create all GameControllers
            for (var i = 0; i< _numGameControllers; i++)
            {
                var gameController = GameControllerInitializer.InitializeGameController(this,
                    managerData.GameControllerData[i]);

                _gameControllers[i] = gameController;

                // Instrument listeners
                gameController.GameStarted += ControllerGameStarted;
                gameController.GameEnded += ControllerGameEnded;
                gameController.GameUpdated += ControllerGameUpdated;
                
                if (!gameController.IsInitialized)
                    Debug.LogError("Game controller not initialized: [" + gameController.Data.GameObjectName + "]");

            }

            _activeGame = 0;
        }

        private void StartNextGame()
        {
            _activeGame += 1;
            
            Debug.Log($"ActiveGame [{_activeGame}]");
            
            if (_activeGame == _gameControllers.Length) return;
            
            StartGame();
        }
        
        public void StartGame()
        {
            Debug.Log($"Triggering StartGame event for [{_gameControllers[_activeGame].Data.Guid}] ");

            GameStart?.Invoke(_gameControllers[_activeGame]);
        }

        public void EndGame()
        {
            GameEnd?.Invoke(_gameControllers[_activeGame]);
        }

        private void ControllerGameUpdated(IGameController controller)
        {
            Debug.Log("ControllerGameUpdated");

        }

        private void ControllerGameEnded(IGameController controller)
        {
            Debug.Log($"Controller [{controller.Data.Guid}] Endgame Event");
            
            StartNextGame();
        }

        private void ControllerGameStarted(IGameController controller)
        {
            Debug.Log($"Controller [{controller.Data.Guid}] StartGame Event");

        }
    }
}
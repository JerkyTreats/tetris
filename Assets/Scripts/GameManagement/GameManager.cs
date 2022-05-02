using System.Collections.Generic;
using UnityEngine;

namespace GameManagement
{
    public class GameManager : MonoBehaviour
    {
        public GameManagerData Data { get; private set; }

        private GameController[] _gameControllers;
        
        private int _numGameControllers;

        public delegate void GameManagerDelegate();
        public event GameManagerDelegate GameStart, GameEnd, Interrupt, Terminate;
        
        public void Initialize(GameManagerData managerData)
        {
            Data = managerData;
            _numGameControllers = Data.GameControllers.Count;
            _gameControllers = new GameController[_numGameControllers];

            // Create all GameControllers
            for (var i = 0; i< _numGameControllers; i++)
            {
                var gameController = GameController.CreateNewGameController(Data.GameControllers[i], this);
                _gameControllers[i] = gameController;

                // Instrument listeners
                gameController.GameStarted += ControllerGameStarted;
                gameController.GameEnded += ControllerGameEnded;
                gameController.GameUpdate += ControllerGameUpdated;
                
                if (!gameController.IsInitialized)
                    Debug.LogError("Game controller not initialized: [" + gameController.name + "]");

            }
        }

        private void ControllerGameUpdated(GameController controller)
        {
            throw new System.NotImplementedException();
        }

        private void ControllerGameEnded(GameController controller)
        {
            throw new System.NotImplementedException();
        }

        private void ControllerGameStarted(GameController controller)
        {
            throw new System.NotImplementedException();
        }
    }
}
using System.Collections;
using UnityEngine;

namespace GameManagement
{
    /// <summary>
    /// Manages the lifecycle of a set of GameControllers. 
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private GameManagerData Data { get; set; }

        private IGameController[] _gameControllers;
        
        private int _numGameControllers;
        private int _activeGame;

        private bool _isSessionStarted;

        public delegate void GameManagerDelegate(IGameController controller);

        public event GameManagerDelegate GameStart, GameEnd;//, Interrupt, Terminate;
        
        /// <summary>
        /// Initialize the GameManager from Data and all child objects
        /// </summary>
        /// <param name="managerData">GameManagerData to initialize from</param>
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
        
        private void ControllerGameStarted(IGameController controller)
        {
            Debug.Log($"Controller [{controller.Data.Guid}] StartGame Event");
        }
        
        private void ControllerGameUpdated(IGameController controller)
        {
            Debug.Log($"Controller [{controller.Data.Guid}] UpdateGame Event");
        }

        private void ControllerGameEnded(IGameController controller)
        {
            Debug.Log($"Controller [{controller.Data.Guid}] Endgame Event");

            ContinueOrEndSession();
        }
        
        /// <summary>
        /// Start the Managed Game Session.
        /// </summary>
        public void StartSession()
        {
            if (_isSessionStarted)
            {
                Debug.LogWarning("GameManager attempted StartSession called after session was already started");
                return;
            }

            _isSessionStarted = true;
            StartNextGame();
        }
              
        /// <summary>
        /// End the Managed Game Session.
        /// </summary>
        public void EndSession() { } // Invoke EndSession event
        
        // Increment the active game and begin the next game in the queue.
        private void StartNextGame()
        {
            Debug.Log($"Triggering StartGame event for [{_gameControllers[_activeGame].Data.Guid}]");
            GameStart?.Invoke(_gameControllers[_activeGame]);
        }
        
        // Step the session, starting the next game or ending the session.
        private void ContinueOrEndSession()
        {
            _activeGame += 1;

            if (_activeGame == _gameControllers.Length)
            {
                EndSession();
            }
            else
            {
                StartNextGame();
            }
        }
    }
}
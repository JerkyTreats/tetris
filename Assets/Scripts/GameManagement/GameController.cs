using System;
using UnityEngine;

namespace GameManagement
{
    public class GameController : MonoBehaviour, IGameController
    {
        public IGameControllerData Data { get; set; }
        public bool IsInitialized { get; private set; }
        
        public event IGameController.GameControllerDelegate GameStarted, GameEnded, GameUpdated;

        public void Initialize(GameManager manager)
        {
            // Register manager events 
            manager.GameStart += GameStart;
            manager.GameEnd += GameEnd;
            // manager.Terminate += Terminate;
            // manager.Interrupt += Interrupt;

            IsInitialized = true;
        }

        public void GameStart(IGameController controller)
        {
            Debug.Log("BASE START");
            GameStarted?.Invoke(this);
        }

        public void GameEnd(IGameController controller)
        {
            GameEnded?.Invoke(this);
        }

        public void GameUpdate(IGameController controller)
        {
            GameUpdated?.Invoke(this);
        }

        public void Terminate(IGameController controller) { }

        public void Interrupt(IGameController controller) { }
    }
}
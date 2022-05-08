namespace GameManagement
{
    /// <summary>
    /// Contract defining responsibility for a GameController object managed by a GameManager.
    /// </summary>
    public interface IGameController
    {
        public bool IsInitialized { get; }
        
        public IGameControllerData Data { get; set; }
        
        public delegate void GameControllerDelegate(IGameController controller);
        public event GameControllerDelegate GameStarted, GameEnded, GameUpdated;
        
        public void Initialize(GameManager manager);
        public void GameStart(IGameController controller);
        
        public void GameEnd(IGameController controller);
        public void GameUpdate(IGameController controller);
        
        public void Terminate(IGameController controller);
        public void Interrupt(IGameController controller);
    }
}
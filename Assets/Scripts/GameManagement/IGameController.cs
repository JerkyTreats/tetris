namespace GameManagement
{
    public interface IGameController
    {
        void Initialize(GameControllerData gcData, GameManager manager);
        void GameStart();
        void GameEnd();
        void Terminate();
        void Interrupt();
    }
}
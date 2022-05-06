using System.Collections.Generic;

namespace GameManagement
{
    public struct GameManagerData
    {
        public IGameControllerData[] GameControllerData { get; set; }

        public GameManagerData(IGameControllerData[] controllersData)
        {
            GameControllerData = controllersData;
        }
    }
}
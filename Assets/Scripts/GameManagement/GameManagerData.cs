using System.Collections.Generic;

namespace GameManagement
{
    // TODO Serializable GameManagerData
    /// <summary>
    /// Data structure for a GameManager
    /// </summary>
    public struct GameManagerData
    {
        public IGameControllerData[] GameControllerData { get; set; }

        public GameManagerData(IGameControllerData[] controllersData)
        {
            GameControllerData = controllersData;
        }
    }
}
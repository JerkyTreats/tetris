using System.Collections.Generic;
using Board.Persistence;
using GameManagement;
using Unity.VisualScripting;
using UnityEngine;

namespace Tetris.Test
{
    public class TetrisClassicManagerTest : MonoBehaviour
    {
        private void Start()
        {
            BoardData board1 = new BoardData(
                new Vector3Int(0, 0, -10),
                new Vector3Int(0, 0, 0),
                new Vector2Int(10, 10),
                2
            );
            
            BoardData board2 = new BoardData(
                new Vector3Int(0, 0, -10),
                new Vector3Int(0, -5, 0),
                new Vector2Int(15, 20),
                2
            );

            TetrisClassicData controllerData = new TetrisClassicData(
                new Vector3Int(-1, 3, 0),
                1f,
                0.5f,
                board1
            );
            
            TetrisClassicData controllerData1 = new TetrisClassicData(
                new Vector3Int(-1, 7, 0),
                0.5f,
                0.5f,
                board2
            );
            

            var gameControllers = new IGameControllerData[2];
            gameControllers[0] = controllerData;
            gameControllers[1] = controllerData1;
                

            GameManagerData managerData = new GameManagerData(
                gameControllers
            );

            var managerObj = new GameObject("GameManager");
            var manager = managerObj.AddComponent<GameManager>();
            
            manager.Initialize(managerData);
            manager.StartGame();
        }
        
        
    }
}
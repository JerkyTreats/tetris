using Board.Persistence;
using Unity.VisualScripting;
using UnityEngine;

namespace Tetris.Test
{
    public class TetrisClassicControllerTest : MonoBehaviour
    {
        private void Start()
        {
            BoardData board1 = new BoardData(
                new Vector3Int(0, 0, -10),
                new Vector3Int(0, 0, 0),
                new Vector2Int(10, 10),
                2
            );

            TetrisClassicData data = new TetrisClassicData(
                new Vector3Int(-1, 3, 0),
                1f,
                0.5f,
                board1
            );

            var controllerObj = new GameObject("Controller");
            var controller = controllerObj.AddComponent<TetrisClassicController>();

            controller.Data = data;
            controller.Initialize();
            controller.GameStart(controller);
        }
    }
}
using Board.Persistence;
using Unity.VisualScripting;
using UnityEngine;

namespace Tetris.Test
{
    public class TetrisClassicControllerTest : MonoBehaviour
    {

        
    //     [ProtoMember(1)] public Vector3Int SpawnPosition { get; set; }
    //     [ProtoMember(2)] public int StepDelay { get; set; }
    //     [ProtoMember(3)] public float LockDelay { get; set; }
    //     
    //     [ProtoMember(4)] public BoardData BoardData { get; set; }
    // }
        
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



            var controller = TetrisClassicController.CreateNewGameController(data);
            
            controller.GameStart(controller);
        }
    }
}
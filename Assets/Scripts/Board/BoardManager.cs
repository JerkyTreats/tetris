using System.Collections.Generic;
using Board.Persistence;
using Common;
using UnityEngine;

namespace Board
{
    /// <summary>
    /// Test Class thing to spin up multiple boards to ensure dynamic board generation is mostly working as intended.
    /// </summary>
    public class BoardManager : MonoBehaviour
    {
        public List<Board> boardObjects;
        public int currentBoard = 1;

        public BoardData board1 = new BoardData(
            new Vector3Int(-1, 7, 0),
            new Vector3Int(0, 0, -10),
            new Vector3Int(0, 0, 0),
            new Vector2Int(10, 20),
            2
        );

        /// <summary>
        /// Create 3 boards. First is the standard Tetris board as you would expect, the other two randomize the size.
        /// </summary>
        void Populate() {

            var boards = new List<BoardData>();
            boards.Add(board1);
            var b = Board.CreateNewBoard(board1);
            boardObjects.Add(b);

            // List<BoardTileData> board2Tiles = new List<BoardTileData>();


            // BoardData board2 = new BoardData
            // {
            //     spawnPosition = new Vector3Int(-1, 7, 0),
            //     cameraPosition = new Vector3Int(0, 0, -10),
            //     boardPosition = new Vector3Int(0, 0, 0),
            //     boardSize = new Vector2Int(10,20),
            //     sortOrder = 2,
            //     tiles = board2Tiles
            // };

            for (var i = 0; i < 2; i++) {
                var nextBoard = GetNextBoard(boards[boards.Count-1]);
                boards.Add(nextBoard);
                b = Board.CreateNewBoard(nextBoard);
                boardObjects.Add(b);
            }
        }

        /// <summary>
        /// Create a board off a previous board. Adds some buffer, builds the struct for the next board
        /// </summary>
        /// <param name="previousBoard"></param>
        /// <returns></returns>
        BoardData GetNextBoard(BoardData previousBoard) {
            var nextBoardSize = new Vector2Int((Random.Range(5, 10) * 2), (Random.Range(5, 10) * 2));
            var nextBoardSizeXOrigin = nextBoardSize.x / 2;
            var buffer = 1;
            var previousBoardXOrigin = previousBoard.boardPosition.x + (previousBoard.boardSize.x / 2);
            var nextX = previousBoardXOrigin + buffer + nextBoardSizeXOrigin;

            var nextBoardPos = new Vector3Int(
                nextX,
                0,
                0
            );

            var nextSpawn = new Vector3Int(
                - 1,
                (nextBoardSize.y / 2) - 3,
                0
            );

            var nextCam = new Vector3Int(
                nextX,
                previousBoard.boardPosition.y,
                -10
            );

            return new BoardData(
                nextSpawn,
                nextCam,
                nextBoardPos,
                nextBoardSize,
                2
            );
        }

        void Start() {
            Populate();
            // Board b = Board.Initialize(board1.spawnPosition, board1.cameraPosition, board1.boardPosition, board1.boardSize, board1.sortOrder);
            // b.ActivateGameOnBoard();
            boardObjects[currentBoard].OnActivate();
            InvokeRepeating("SwitchBoard", 2.0f, 212.0f);
        }

        /// <summary>
        /// Cycle through each board and see what happens
        /// </summary>
        void SwitchBoard(){
            boardObjects[currentBoard].OnDeactivate();
            currentBoard = Helpers.Wrap(currentBoard + 1, 0, boardObjects.Count);
            boardObjects[currentBoard].OnActivate();

        }
    }
}

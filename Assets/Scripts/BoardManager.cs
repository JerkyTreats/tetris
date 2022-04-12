using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Test Class thing to spin up multiple boards to ensure dynamic board generation is mostly working as intended.
/// </summary>
public class BoardManager : MonoBehaviour
{
    public List<Board> boardObjects;
    public int currentBoard = 1;

    public BoardData board1 = new BoardData
    {
        spawnPosition = new Vector3Int(-1, 7, 0),
        cameraPosition = new Vector3Int(0, 0, -10),
        boardPosition = new Vector3Int(0, 0, 0),
        boardSize = new Vector2Int(10,20),
        sortOrder = 2,
    };

    /// <summary>
    /// Create 3 boards. First is the standard Tetris board as you would expect, the other two randomize the size.
    /// </summary>
    void Populate() {
        List<BoardData> boards = new List<BoardData>();
        boards.Add(board1);
        Board b = Board.Initialize(board1.spawnPosition, board1.cameraPosition, board1.boardPosition, board1.boardSize, board1.sortOrder);
        boardObjects.Add(b);

        for (int i = 0; i < 2; i++) {
            BoardData nextBoard = GetNextBoard(boards[boards.Count-1]);
            boards.Add(nextBoard);
            b = Board.Initialize(nextBoard.spawnPosition, nextBoard.cameraPosition, nextBoard.boardPosition, nextBoard.boardSize, 2);
            boardObjects.Add(b);
        }
    }

    /// <summary>
    /// Create a board off a previous board. Adds some buffer, builds the struct for the next board
    /// </summary>
    /// <param name="previousBoard"></param>
    /// <returns></returns>
    BoardData GetNextBoard(BoardData previousBoard) {
        Vector2Int nextBoardSize = new Vector2Int((Random.Range(5, 10) * 2), (Random.Range(5, 10) * 2));
        int nextBoardSizeXOrigin = nextBoardSize.x / 2;
        int buffer = 1;
        int previousBoardXOrigin = previousBoard.boardPosition.x + (previousBoard.boardSize.x / 2);
        int nextX = previousBoardXOrigin + buffer + nextBoardSizeXOrigin;

        Vector3Int nextBoardPos = new Vector3Int(
            nextX,
            0,
            0
        );

        Vector3Int nextSpawn = new Vector3Int(
            - 1,
            (nextBoardSize.y / 2) - 3,
            0
        );

        Vector3Int nextCam = new Vector3Int(
            nextX,
            previousBoard.boardPosition.y,
            -10
        );

        return new BoardData {
            spawnPosition = nextSpawn,
            cameraPosition = nextCam,
            boardPosition = nextBoardPos,
            boardSize = nextBoardSize,
            sortOrder = 2,
        };
    }

    void Start() {
        Populate();
        // Board b = Board.Initialize(board1.spawnPosition, board1.cameraPosition, board1.boardPosition, board1.boardSize, board1.sortOrder);
        // b.ActivateGameOnBoard();
        boardObjects[currentBoard].ActivateGameOnBoard();
        InvokeRepeating("SwitchBoard", 2.0f, 212.0f);
    }

    /// <summary>
    /// Cycle through each board and see what happens
    /// </summary>
    void SwitchBoard(){
        boardObjects[currentBoard].DeactivateGame();
        currentBoard = Helpers.Wrap(currentBoard + 1, 0, boardObjects.Count);
        boardObjects[currentBoard].ActivateGameOnBoard();

    }
}

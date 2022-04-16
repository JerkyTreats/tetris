using UnityEngine;
using UnityEngine.Tilemaps;

public static class BoardFactory
{ 
    public static Board CreateNewBoard(Vector3Int spawnPosition, Vector3Int cameraPosition, Vector3Int boardPosition, Vector2Int boardSize, int sortOrder) {
        var boardGo = TileGameObjectFactory.CreateNewTileObject("Board", boardPosition, sortOrder);

        var board = boardGo.AddComponent<Board>();
        board.boardPosition = boardPosition;
        board.boardSize = boardSize;
        board.boardCamera = new BoardCamera(board.gameObject, cameraPosition);

        board.tetrisGameLogic = TetrisGameLogic.CreateNewGameLogic(board, spawnPosition);

        Border.CreateBoardBorder(board);
        BoardBackground.CreateBoardBackground(board);

        return board;
    }

    public static Board CreateNewBoard(BoardData boardData) {
        return CreateNewBoard(boardData.spawnPosition, boardData.cameraPosition, boardData.boardPosition, boardData.boardSize, boardData.sortOrder);
    }
}
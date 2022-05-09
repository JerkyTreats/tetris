using Board.Persistence;
using Common;
using GameManagement;
using Initialization;
using Tetris;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Board
{
    public class Board : MonoBehaviour, IManagedGameObject
    {
        #region Properties

        public BoardData data;

        public Vector3Int BoardPosition => data.boardPosition;

        // Will likely be using these in the future but YAGNI for the moment
        public Border border;
        public BoardBackground background;

        public GameData GameData { get; private set; }
        public BoardCamera BoardCamera { get; private set; }

        public Tilemap Tilemap { get; private set; }

        /// <summary>
        /// Rectangle representing the board in WorldSpace
        /// </summary>
        public RectInt WorldBounds
        {
            get
            {
                // Origin is middle of rect, position is bottom left
                var position = new Vector2Int(
                    data.boardPosition.x - data.boardSize.x / 2,
                    data.boardPosition.y - data.boardSize.y / 2
                );

                return new RectInt(position, data.boardSize);
            }
        }

        /// <summary>
        /// Rectangle representing the board in TileSpace (Origin is always 0,0)
        /// </summary>
        public RectInt TileBounds
        {
            get
            {
                var bottomLeft = new Vector2Int(
                    -data.boardSize.x / 2,
                    -data.boardSize.y / 2
                );

                return new RectInt(bottomLeft, data.boardSize);
            }
        }
        
        #endregion

        #region Initialization

        /// <summary>
        /// Create a new Board GameObject with configured components
        /// </summary>
        /// <param name="boardData"></param>
        /// <returns></returns>
        public static Board CreateNewBoard(BoardData boardData, Transform parent = null)
        {
            var boardGo =
                TileGameObjectFactory.CreateNewTileObject("Board", boardData.boardPosition, boardData.sortOrder);
            boardGo.transform.parent = parent;
            
            var board = boardGo.AddComponent<Board>();
            board.data = boardData;
            board.BoardCamera = BoardCamera.CreateNewBoardCamera(board);

            return board;
        }

        private void Awake()
        {
            Tilemap = GetComponentInChildren<Tilemap>();

            GameData = Resources.Load<GameData>("GameData");

            border = Border.CreateBoardBorder(this);
            background = BoardBackground.CreateBoardBackground(this);

            // Initialize each Tetromino listed in Editor object
            for (var i = 0; i < GameData.tetrominos.Length; i++)
            {
                GameData.tetrominos[i].Initialize();
            }
        }

        private void Start()
        {
            foreach (var tileData in data.tiles)
            {
                Tilemap.SetTile(tileData.position, GameData.GetTileFromBlock(tileData.block));
            }
        }

        #endregion
        
        // TODO These two functions should probably be an interface for all Activatable objects
        public void Activate()
        {
            BoardCamera.ActivateCamera();
        }

        public void Deactivate()
        {
            BoardCamera.DeactivateCamera();
            Tilemap.ClearAllTiles();
        }

        public void Terminate()
        {
            Destroy(gameObject);
        }


        /// <summary>
        /// Determines if the input position is within the bounds of the Board size.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsValidPosition(Vector3Int position)
        {
            if (!TileBounds.Contains((Vector2Int) position))
                return false;

            if (Tilemap.HasTile(position))
                return false;

            return true;
        }
        
        
        // EDITOR: Draw the WorldBounds to inspect while game is paused
        // TODO Disable on real builds? 
        private void OnDrawGizmosSelected()
        {
            // Draw a yellow cube at the transform position
            Gizmos.color = Color.yellow;
            // Gizmos.DrawWireCube(transform.position, new Vector3(WorldBounds.size.x, WorldBounds.size.y, 0));
            Gizmos.DrawWireCube(transform.position, new Vector3(WorldBounds.size.x, WorldBounds.size.y, 0));
        }

        // TODO - I'm not sure I love this. This sets blocks, but Tetrominos are sets of blocks... Overload? Separate GameEditorBoard?
        public void SetTile(Block block, Vector3Int tilePosition, Tile tile)
        {
            data.tiles.Add(new BoardTileData()
            {
                block = block,
                position = tilePosition
            });
            
            
            Tilemap.SetTile(tilePosition, tile);
        }
    }
}
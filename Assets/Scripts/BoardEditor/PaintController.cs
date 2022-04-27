using Common;
using Initialization;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BoardEditor
{
    /// <summary>
    /// Controller for Painter 
    /// </summary>
    public class PaintController : MonoBehaviour
    {
        private GameData _gameData;
        private Tilemap Tilemap { get; set; }

        private void Start()
        {
            _gameData = Resources.Load<GameData>("GameData");
            // Register all paintbutton selection events 
            foreach (var paintButton in FindObjectsOfType<PaintButton>())
            {
                paintButton.PainterSelectEvent += SelectTile;
            }

            var newBoardController = FindObjectOfType<NewBoardController>();
            newBoardController.NewBoardEvent += CleanUp;

            var menuController = FindObjectOfType<MenuButtonController>();
            menuController.SaveBoardMenuClickEvent += ActivePainterCleanUp;
            menuController.LoadBoardMenuClickEvent += ActivePainterCleanUp;
            menuController.ActivateBoardEvent += SetTileMap;
        }

        private void SetTileMap(Board.Board board)
        {
            Tilemap = board.Tilemap;
        }


        // on PaintButton delegate event fire
        private void SelectTile(PaintButton paintButton)
        {
            ActivePainterCleanUp();
            ActivePaintTile.CreateNewActivePainter(paintButton, Tilemap, _gameData.GetBlockFromTile(paintButton.tile));
        }

        // Seek and destroy all ActivePainter
        // Argued with putting this in ActivePainter, but this operation runs on a set of painters
        // Therefore I feel its more appropriate here.
        private static void ActivePainterCleanUp()
        {
            var existingPainter = FindObjectsOfType<ActivePaintTile>();
            foreach (var activePainter in existingPainter)
            {
                Destroy(activePainter.gameObject);
            }
        }
        
        private void CleanUp(Board.Board _)
        {
            ActivePainterCleanUp();
            Tilemap.ClearAllTiles();
        }
    }
}

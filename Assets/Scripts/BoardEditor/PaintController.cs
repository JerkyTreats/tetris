using UnityEngine;
using UnityEngine.Tilemaps;

namespace BoardEditor
{
    /// <summary>
    /// Controller for Painter 
    /// </summary>
    public class PaintController : MonoBehaviour
    {
        private Tilemap Tilemap { get; set; }

        private void Start()
        {
            // Register all paintbutton selection events 
            foreach (var paintButton in FindObjectsOfType<PaintButton>())
            {
                paintButton.PainterSelectEvent += SelectTile;
            }
            var paintTileMapObject = TileGameObjectFactory.CreateNewTileObject("PaintGrid", Vector3Int.zero, 3);
            Tilemap = paintTileMapObject.GetComponent<Tilemap>();
        }

        // on PaintButton delegate event fire
        private void SelectTile(PaintButton paintButton)
        {
            ActivePainterCleanUp();
            ActivePaintTile.CreateNewActivePainter(paintButton, Tilemap);
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
    }
}

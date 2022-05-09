using Board;
using Board.Persistence;
using UnityEngine;

namespace BoardEditor
{
    /// <summary>
    /// Sets up a Board bespoke to the BoardEditor. 
    /// </summary>
    public class BoardEditorBoardFactory
    {
        // TODO: NewBoardContextController: UI Padding to top-level UI ScriptableObject Asset
        // This field represents the padding for the UI whilst in Portrait mode. 
        // Not only is it magic numbers, it also will likely need to be extended to support landscape.
        private readonly Vector2 _uiPadding = new Vector2(100, 500f);

        private readonly Rect _canvasRect;
        private Board.Board _board;

        public BoardEditorBoardFactory(Rect canvasRect)
        {
            _canvasRect = canvasRect;
        }

        public Board.Board CreateNewBoard(BoardData data)
        {
            _board = Board.Board.CreateNewBoard(data);
            SetupBoardCamera();

            return _board;
        }

        // Default camera behaviour is to fit whole screen, we must change to fit UI 
        // Setup Camera and Activate
        private void SetupBoardCamera()
        {
            SetBoardCameraSize();
            SetBoardCameraPosition();
            _board.Activate();
        }

        // Determine + Set how big a board should be so there's no overlap with UI
        private void SetBoardCameraSize()
        {
            // Get percentage of padding to Canvas, send as input for BoardCamera size 
            var paddingX = _uiPadding.x / _canvasRect.width;
            var paddingY = _uiPadding.y / _canvasRect.height;

            var fitCamSize = BoardCamera.OrthoCameraSizeFitToBoard(_board, new Vector2(paddingX, paddingY));
            _board.BoardCamera.Cam.orthographicSize = fitCamSize;
        }
        
        // Move Camera so the board fits the screen with no overlap with UI 
        private void SetBoardCameraPosition()
        {
            // Offset Camera position to avoid UI overlap
            var cam = _board.BoardCamera.Cam;

            var paddingX = _uiPadding.x / _canvasRect.width;
            var paddingY = _uiPadding.y / _canvasRect.height;
            var cameraTopRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight));
            var paddingWorldSpaceAmount = new Vector3( cameraTopRight.x * paddingX, cameraTopRight.y * paddingY);

            var cameraBottomLeft = cam.ScreenToWorldPoint(new Vector3()); // Always 0,0 in pixel space
            var boardBottomLeft = new Vector3(_board.WorldBounds.min.x, _board.WorldBounds.min.y);
            var cornerDelta = boardBottomLeft - cameraBottomLeft;
            var remainingDelta = paddingWorldSpaceAmount - cornerDelta;

            cam.transform.position -= cornerDelta + remainingDelta;
        }


    }
}
using Common;
using UnityEngine;
using static UnityEngine.Screen;

namespace Board
{
    [RequireComponent(typeof(Camera))]
    public class BoardCamera : MonoBehaviour
    {
        public Camera Cam { get; private set; }
        
        public static BoardCamera CreateNewBoardCamera(Board board){
            var boardCameraObject = new GameObject("Camera")
            {
                transform =
                {
                    position = board.data.cameraPosition,
                    parent = board.transform,
                }
            };
            boardCameraObject.SetActive(false);
            

            var camera = boardCameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = OrthoCameraSizeFitToBoard(board);
        
            // TODO : Pull magic number into static variable, probably in gameData or something
            camera.clearFlags = CameraClearFlags.SolidColor;
            var color = Helpers.HexStringToColor("171B1F");
            camera.backgroundColor = color;

            var boardCamera = boardCameraObject.AddComponent<BoardCamera>();
            boardCamera.Cam = camera;
    
            return boardCamera;
        }

        public static float OrthoCameraSizeFitToBoard(Board board, Vector2 padding)
        {
            var paddedHalfBoard = board.WorldBounds.size.y / (2 - (2 * padding.y));
            var screenRatio = (float)width / height;
            var targetRatio = (float)board.WorldBounds.size.x / board.WorldBounds.size.y;

            if(screenRatio >= targetRatio)
                return paddedHalfBoard;

            var differenceInSize = targetRatio / screenRatio;
            return paddedHalfBoard * differenceInSize;
        }

        public static float OrthoCameraSizeFitToBoard(Board board)
        {
            return OrthoCameraSizeFitToBoard(board, new Vector2());
        }

        private void Awake()
        {
            Cam = GetComponent<Camera>();
        }
    
        /// <summary>
        /// Makes this camera the one the user will be viewing.
        /// </summary>
        public void ActivateCamera() {
            DisableAllOtherCameras();
            gameObject.tag = "MainCamera"; // sets Camera.main property
            gameObject.SetActive(true);
            Cam.enabled = true;

        }

        public void DeactivateCamera() {
            Cam.enabled = false;
            gameObject.SetActive(false);
        }

        private static void DisableAllOtherCameras()
        {
            var cams = FindObjectsOfType<Camera>();
            foreach (var cam in cams)
            {
                cam.enabled = false;
                cam.gameObject.SetActive(false);
            }
        }

    }
}

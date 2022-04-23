using Common;
using UnityEngine;
using static UnityEngine.Screen;

namespace Board
{
    [RequireComponent(typeof(Camera))]
    public class BoardCamera : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        
        public static BoardCamera CreateNewBoardCamera(Board board){
            var boardCameraObject = new GameObject("Camera")
            {
                transform =
                {
                    position = board.data.cameraPosition
                }
            };
            boardCameraObject.SetActive(false);
            

            var camera = boardCameraObject.AddComponent<Camera>();
            boardCameraObject.transform.parent = board.transform;

            camera.orthographic = true;
            camera.orthographicSize = OrthoCameraSizeFitToBoard(board);
        
            // TODO : Pull magic number into static variable, probably in gameData or something
            camera.clearFlags = CameraClearFlags.SolidColor;
            var color = Helpers.HexStringToColor("171B1F");
            camera.backgroundColor = color;

            var boardCamera = boardCameraObject.AddComponent<BoardCamera>();
            boardCamera.cam = camera;
    
            return boardCamera;
        }

        public static float OrthoCameraSizeFitToBoard(Board board)
        {
            var screenRatio = width / (float)height;
            var targetRatio = board.WorldBounds.size.x / (float)board.WorldBounds.size.y;
            
            if(screenRatio >= targetRatio)
                return (float)board.WorldBounds.size.y / 2;

            var differenceInSize = targetRatio / screenRatio;
            return (float)board.WorldBounds.size.y / 2 * differenceInSize;
        }

        private void Awake()
        {
            cam = GetComponent<Camera>();
        }
    
        /// <summary>
        /// Makes this camera the one the user will be viewing.
        /// </summary>
        public void ActivateCamera() {
            DisableAllOtherCameras();
            gameObject.tag = "MainCamera"; // sets Camera.main property
            gameObject.SetActive(true);
            cam.enabled = true;

        }

        public void DeactivateCamera() {
            cam.enabled = false;
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

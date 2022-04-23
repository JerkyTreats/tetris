using Board;
using UnityEngine;

namespace BoardEditor
{
    public class UIHelpers : MonoBehaviour
    {
        public static void Clear()
        {
            var cameras = FindObjectsOfType<Camera>();
            foreach (var camera in cameras)
            {
                camera.enabled = false;
            }
            
            BoardManager.ClearBoards();
        }
    }
}
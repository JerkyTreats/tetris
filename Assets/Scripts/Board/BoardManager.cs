using UnityEngine;

namespace Board
{
    public class BoardManager : MonoBehaviour
    {
        /// <summary>
        /// Destroy all board Game Objects
        /// </summary>
        public static void ClearBoards()
        {
            var boards = FindObjectsOfType<Board>();
            foreach (var board in boards)
            {
                board.Deactivate();
                board.Terminate();
            }
        }
    }
}
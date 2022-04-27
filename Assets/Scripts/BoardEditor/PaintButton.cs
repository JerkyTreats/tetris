using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace BoardEditor
{
    /// <summary>
    /// Functionality for each button in the TilePanel
    /// </summary>
    public class PaintButton : MonoBehaviour
    {
        public Tile tile;
        [SerializeField] private Button button;

        public delegate void PainterDelegate(PaintButton paintButton);
        public event PainterDelegate PainterSelectEvent;

        private void Start()
        {
            button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(TaskOnClick);
        }

        private void TaskOnClick()
        {
            PainterSelectEvent?.Invoke(this);
        }
        
    }
}

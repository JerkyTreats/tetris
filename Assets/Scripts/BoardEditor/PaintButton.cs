using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace BoardEditor
{
    /// <summary>
    /// Button logic for Tile Painter. Fires events on click
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

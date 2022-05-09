using UnityEngine;
using UnityEngine.Tilemaps;

namespace Common
{
    public static class TileGameObjectFactory
    {
        /// <summary>
        /// Creates a generic Tilemap Game Object with necessary components.  
        /// </summary>
        public static GameObject CreateNewTileObject(string name, Vector3 position, int sortOrder = 0, Transform parent = null)
        {
            var gameObject = new GameObject(name)
            {
                transform =
                {
                    position = position
                }
            };
            if (parent != null)
                gameObject.transform.parent = parent;

            gameObject.AddComponent<Grid>();

            gameObject.AddComponent<Tilemap>();
            var renderer = gameObject.AddComponent<TilemapRenderer>();
            renderer.sortingOrder = sortOrder;
        
            return gameObject;
        }
    }
}
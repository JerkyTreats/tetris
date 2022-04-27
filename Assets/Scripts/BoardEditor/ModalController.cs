using UnityEngine;

namespace BoardEditor
{
    /// <summary>
    /// Parent class for shared functionality of Modal menus
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class ModalController : MonoBehaviour
    {
        protected CanvasGroup canvasGroup;

        // Trick from the internet to make a UI element visible/interactable
        protected void Enable()
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        protected void Disable()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
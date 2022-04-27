using UnityEngine;

namespace BoardEditor
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ModalController : MonoBehaviour
    {
        protected CanvasGroup canvasGroup;

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
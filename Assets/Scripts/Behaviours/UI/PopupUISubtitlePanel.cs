using UnityEngine;
namespace PixelCrushers.DialogueSystem
{
    [AddComponentMenu("")] // Use wrapper.
    public class PopupUISubtitlePanel : StandardUISubtitlePanel
    {

        [Header("Prompt Settings")]
        [Tooltip("Layer mask for kill zones. If the subtitle panel enters a kill zone, it will be destroyed.")]
        public LayerMask killZoneLayerMask;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & killZoneLayerMask) != 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

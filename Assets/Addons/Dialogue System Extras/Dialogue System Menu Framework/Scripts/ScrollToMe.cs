// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace PixelCrushers.DialogueSystem.MenuSystem
{

    /// <summary>
    /// Add this script to items inside a ScrollRect. When selected by joystick or keyboard,
    /// it will scroll the ScrollRect to that item.
    /// </summary>
    public class ScrollToMe : MonoBehaviour, ISelectHandler
    {

        public void OnSelect(BaseEventData eventData)
        {
            if (!InputDeviceManager.autoFocus) return;
            StartCoroutine(ScrollToMeAfterUIUpdate());
        }

        private IEnumerator ScrollToMeAfterUIUpdate()
        {
            yield return null; // If UI is just appearing, need to set in next frame.
            var scrollRect = GetComponentInParent<UnityEngine.UI.ScrollRect>();
            if (scrollRect == null) yield break;
            int numActiveChildren = 0;
            int myIndex = 0;
            foreach (Transform t in transform.parent)
            {
                if (t.gameObject.activeSelf)
                {
                    if (t == transform) myIndex = numActiveChildren;
                    numActiveChildren++;
                }
            }
            float pos = (myIndex < 2) ? 1 : (myIndex >= (numActiveChildren - 1)) ? 0 : Mathf.Clamp01(1 - (float)myIndex / (float)numActiveChildren);
            scrollRect.verticalNormalizedPosition = pos;
        }
    }
}
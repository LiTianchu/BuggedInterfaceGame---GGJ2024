#if USE_ENHANCEDSCROLLER
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using System;

namespace PixelCrushers.DialogueSystem.Extras
{

    public class TextlineCellView : EnhancedScrollerCellView
    {
        public GameObject iconPanelPrefab;
        public StandardUISubtitlePanel npcSubtitlePanelPrefab;
        public StandardUISubtitlePanel pcSubtitlePanelPrefab;
        public StandardUIMenuPanel menuPanelPrefab;

        private GameObject instantiatedChild = null;

        [NonSerialized] public GameObject prefab = null;

        public void SetIconData()
        {
            if (prefab != iconPanelPrefab)
            {
                Destroy(instantiatedChild);
                prefab = iconPanelPrefab;
                instantiatedChild = Instantiate(prefab, transform, false);
            }
        }

        public void SetSubtitleData(Subtitle subtitle, bool isLoadingGame)
        {
            var newPrefab = subtitle.speakerInfo.isNPC ? npcSubtitlePanelPrefab.gameObject : pcSubtitlePanelPrefab.gameObject;
            if (prefab != newPrefab)
            {
                Destroy(instantiatedChild);
                prefab = newPrefab;
                instantiatedChild = Instantiate(prefab, transform, false);
            }
            var panel = instantiatedChild.GetComponent<StandardUISubtitlePanel>();
            panel.ShowSubtitle(subtitle);
            var typewriter = panel.GetTypewriter();
            if (typewriter != null) typewriter.Stop();
        }

        public void SetMenuData(Subtitle subtitle, Response[] responses, Transform target)
        {
            if (prefab != menuPanelPrefab)
            {
                Destroy(instantiatedChild);
                prefab = menuPanelPrefab.gameObject;
                instantiatedChild = Instantiate(prefab, transform, false);
            }
            var panel = instantiatedChild.GetComponent<StandardUIMenuPanel>();
            panel.gameObject.SetActive(true);
            panel.ShowResponses(subtitle, responses, target);
        }
    }
}
#endif
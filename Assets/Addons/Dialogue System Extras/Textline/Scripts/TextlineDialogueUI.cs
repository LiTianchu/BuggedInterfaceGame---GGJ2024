using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrushers.DialogueSystem.Extras
{

    /// <summary>
    /// Wrapper for SMSDialogueUI.
    /// </summary>
    public class TextlineDialogueUI : SMSDialogueUI
    {
        [SerializeField] private bool showMultipleAlertsInSequence = true;
        [SerializeField] private float closeDelay = 5f;

        private List<UIPanel> _alertPanelQueue = new();
        public override void ShowAlert(string message, float duration)
        {
            if (!showMultipleAlertsInSequence)
            {
                base.ShowAlert(message, duration);
                return;
            }

            if (string.IsNullOrEmpty(message)) { return; }

            foreach (UIPanel alertPanel in _alertPanelQueue)
            {
                RectTransform rect = alertPanel.GetComponent<RectTransform>();
                float rectHeight = rect.rect.height;
                float newY = rect.anchoredPosition.y + rectHeight;
                alertPanel.GetComponent<RectTransform>().DOAnchorPosY(
                    newY,
                    0.5f
                ).SetEase(Ease.OutBack);
            }

            UIPanel newAlertPanel = Instantiate(alertUIElements.panel, transform);

            newAlertPanel.transform.SetAsFirstSibling(); // prevent blocking the main panel
            TMP_Text txt = Instantiate(alertUIElements.alertText.textMeshProUGUI, newAlertPanel.transform);
            txt.text = FormattedText.Parse(message).text;

            _alertPanelQueue.Add(newAlertPanel);

            newAlertPanel.SetOpen(true);
            txt.gameObject.SetActive(true);

            StartCoroutine(CloseAlertAfterDelay(newAlertPanel));

        }

        protected override void AddMessage(Subtitle subtitle)
        {
            // check if the subtitle is from NPC
            if (subtitle.speakerInfo.isNPC)
            {
                DialogueManager.ShowAlert(subtitle.formattedText.text);
            }
            
            base.AddMessage(subtitle);
        }

        private IEnumerator CloseAlertAfterDelay(UIPanel alertPanel)
        {
            yield return new WaitForSeconds(closeDelay);
            alertPanel.SetOpen(false);

            // remove the alert panel from the queue
            if (_alertPanelQueue[_alertPanelQueue.Count - 1] == alertPanel)
            {
                _alertPanelQueue.RemoveAt(_alertPanelQueue.Count - 1);
            }
            else
            {
                foreach (UIPanel queuedPanel in _alertPanelQueue)
                {
                    if (queuedPanel == alertPanel)
                    {
                        _alertPanelQueue.Remove(queuedPanel);
                        break;
                    }
                }
            }
            
            yield return new WaitForSeconds(2f); // Wait for the close animation to finish
            Destroy(alertPanel.gameObject);
        }
    }
}
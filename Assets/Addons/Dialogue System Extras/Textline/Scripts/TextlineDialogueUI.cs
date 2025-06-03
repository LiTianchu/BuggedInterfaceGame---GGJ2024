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

        private Queue<UIPanel> _alertPanelQueue = new();
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
            TMP_Text txt = Instantiate(alertUIElements.alertText.textMeshProUGUI, newAlertPanel.transform);
            txt.text = FormattedText.Parse(message).text;

            _alertPanelQueue.Enqueue(newAlertPanel);

            newAlertPanel.SetOpen(true);
            txt.gameObject.SetActive(true);

            StartCoroutine(CloseAlertAfterDelay(newAlertPanel));

        }
        
        private IEnumerator CloseAlertAfterDelay(UIPanel alertPanel)
        {
            yield return new WaitForSeconds(closeDelay);
            alertPanel.SetOpen(false);
            yield return new WaitForSeconds(2f); // Wait for the close animation to finish
            Destroy(alertPanel.gameObject);
        }



        

    }


        // public override void SetActive(bool value)
        // {
        //     if (panel != null)
        //     {
        //         if (!m_initializedAnimator && value == false)
        //         {
        //             if (panel.deactivateOnHidden)
        //             {
        //                 panel.gameObject.SetActive(false);
        //             }
        //         }
        //         else
        //         {
        //             panel.SetOpen(value);
        //         }
        //     }
        //     m_initializedAnimator = true;
        //     if (value == true || panel == null) alertText.SetActive(true);
        // }
}
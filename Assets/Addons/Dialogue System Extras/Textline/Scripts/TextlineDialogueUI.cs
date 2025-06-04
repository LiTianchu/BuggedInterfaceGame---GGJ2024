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
        [SerializeField] private bool showDialogueAsNotificationSequence = true;
        [SerializeField] private FadeAfterShowTime notificationUI;
        [SerializeField] private TMP_Text notificationTextLabel;

        private List<FadeAfterShowTime> _notificationList = new();
        public override void ShowAlert(string message, float duration)
        {
            if (!showDialogueAsNotificationSequence)
            {
                base.ShowAlert(message, duration);
                return;
            }

            if (string.IsNullOrEmpty(message)) { return; }

            foreach (FadeAfterShowTime notification in _notificationList)
            {
                RectTransform rect = notification.GetComponent<RectTransform>();
                float rectHeight = rect.rect.height;
                float newY = rect.anchoredPosition.y + rectHeight;
                notification.GetComponent<RectTransform>().DOAnchorPosY(
                    newY,
                    0.5f
                ).SetEase(Ease.OutBack);
            }

            FadeAfterShowTime newNotification = Instantiate(notificationUI, transform);

            newNotification.transform.SetAsFirstSibling(); // prevent blocking the main panel
            TMP_Text txt = Instantiate(notificationTextLabel, newNotification.transform);
            txt.text = FormattedText.Parse(message).text;

            _notificationList.Add(newNotification);

            newNotification.gameObject.SetActive(true);
            txt.gameObject.SetActive(true);

            newNotification.OnFadeOutCompleted += () =>
            {
                _notificationList.Remove(newNotification);
            };
            
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

    }
}
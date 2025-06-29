using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using static AnimationUtils;

public class Change : MonoBehaviour
{
    [Header("Authentication Settings(Optional)")]
    [SerializeField] string username;
    [SerializeField] string password;
    [SerializeField] TMP_InputField usernameField;
    [SerializeField] TMP_InputField passwordField;

    [Header("GameObjects to Activate/Deactivate upon changing displayed panel")]
    [SerializeField] GameObject deactivated;
    [SerializeField] GameObject activated;
    [SerializeField] bool hasTransition = true;

    [Header("Transition Settings")]
    [SerializeField] float transitionDuration = 0.5f;
    [SerializeField] TransitionType transitionType = TransitionType.Fade;
    [SerializeField] Ease easeType = Ease.OutQuad;
    [Header("Slide Transition Settings")]
    [SerializeField] Vector2 deactivatedSlideOutPosition = new Vector2(-1000, 0);
    [SerializeField] Vector2 activatedSlideInPosition = new Vector2(0, 0);



    private Vector2 _stayAnchorPosition;
    private RectTransform _deactivatedRect;
    private RectTransform _activatedRect;

    private void Start()
    {
        if (deactivated != null)
        {
            _deactivatedRect = deactivated.GetComponent<RectTransform>();
            _stayAnchorPosition = _deactivatedRect.anchoredPosition;
        }

        if (activated != null)
        {
            _activatedRect = activated.GetComponent<RectTransform>();
            _stayAnchorPosition = _activatedRect.anchoredPosition;
        }
    }

    public void Replace()
    {
        bool authenticated = true;

        // Check authentication if fields are provided
        if (usernameField != null && passwordField != null)
        {
            authenticated = usernameField.text == username && passwordField.text == password;
        }

        if (authenticated)
        {
            // if (hasTransition)
            // {
            //     StartCoroutine(ReplaceWithTransition());
            // }
            // else
            // {
            // Immediate replacement without transition
            if (deactivated != null)
            {
                UIManager.Instance.HideUI(deactivated);
                // UITransition transitionObject = deactivated.GetComponent<UITransition>();
                // if (transitionObject != null)
                // {
                //     transitionObject.TransitionOut();
                // }
                // else
                // {
                //     deactivated.SetActive(false);
                // }
            }

            if (activated != null)
            {
                UIManager.Instance.ShowUI(activated);
                // UITransition transitionObject = activated.GetComponent<UITransition>();
                // if (transitionObject != null)
                // {
                //     transitionObject.TransitionIn();
                // }
                // else
                // {
                //     activated.SetActive(true);
                // }

                //}
            }
        }

        //private IEnumerator ReplaceWithTransition()
        //{
        // RectTransform deactivatedRect = deactivated != null ? deactivated.GetComponent<RectTransform>() : null;
        // RectTransform activatedRect = activated != null ? activated.GetComponent<RectTransform>() : null;
        // switch (transitionType)
        // {
        //     case TransitionType.Fade:
        //         yield return StartCoroutine(UIFadeTransition(deactivatedRect,
        //                                                      activatedRect,
        //                                                      transitionDuration,
        //                                                      easeType));
        //         break;
        //     case TransitionType.Slide:
        //         yield return StartCoroutine(UISlideTransition(deactivatedRect,
        //                                                       activatedRect,
        //                                                       transitionDuration,
        //                                                       originalAnchorPositions[deactivated],
        //                                                       originalAnchorPositions[activated],
        //                                                       easeType));
        //         break;
        //     case TransitionType.Scale:
        //         yield return StartCoroutine(UIScaleTransition(deactivatedRect, activatedRect, transitionDuration, easeType));
        //         break;
        //     case
        //     default: // no transition
        //         if (deactivated != null) deactivated.SetActive(false);
        //         if (activated != null) activated.SetActive(true);
        //         break;
        // }

        //}
    }
}

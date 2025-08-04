using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserRegistrationPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private GridLayoutGroup avartarGridLayoutGroup;

    private List<UnityEngine.UI.Toggle> _avatarButtons = new List<UnityEngine.UI.Toggle>();
    private Sprite _selectedAvatarSprite;
    public void Show()
    {
        GetComponent<UITransition>().TransitionIn();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Find all buttons in the avatar grid layout group
        foreach (Transform child in avartarGridLayoutGroup.transform)
        {
            UnityEngine.UI.Toggle toggle = child.GetComponent<UnityEngine.UI.Toggle>();
            if (toggle != null)
            {
                _avatarButtons.Add(toggle);
            }
        }

        // Add listener to each button
        foreach (UnityEngine.UI.Toggle toggle in _avatarButtons)
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    OnAvatarSelected(toggle);
                }
            });
        }
        _avatarButtons[0].isOn = true; // Select the first avatar by default
        OnAvatarSelected(_avatarButtons[0]); // Initialize the selected avatar
    }

    private void OnAvatarSelected(UnityEngine.UI.Toggle toggle)
    {
        Image img = toggle.GetComponent<Image>();
        if (img != null)
        {
            foreach (UnityEngine.UI.Toggle t in _avatarButtons)
            {
                if (t != toggle)
                {
                    t.isOn = false; // Deselect other toggles
                    t.colors = new ColorBlock
                    {
                        normalColor = new Color(0.38f, 0.38f, 0.38f), // Dim the deselected avatar
                        highlightedColor = Color.white,
                        pressedColor = new Color(0.58f, 0.58f, 0.58f),
                        selectedColor = Color.white,
                        disabledColor = Color.gray,
                        colorMultiplier = 1f
                    };


                }
            }

            // Set the selected avatar sprite in GameManager
            _selectedAvatarSprite = img.sprite;
            toggle.isOn = true; // Ensure the selected toggle is on

            toggle.colors = new ColorBlock
            {
                normalColor = Color.white, // Highlight the selected avatar
                highlightedColor = Color.white,
                pressedColor = Color.white,
                selectedColor = Color.white,
                disabledColor = Color.gray,
                colorMultiplier = 1f
            };

            Debug.Log($"Avatar selected: {img.sprite.name}");
        }
        else
        {
            Debug.LogWarning("Toggle does not have an Image component.");
        }
    }

    public void OnSubmitButtonClicked()
    {
        string username = usernameInputField.text.Trim();
        if (string.IsNullOrEmpty(username))
        {
            // trigger special dialogue for empty username
            Debug.Log("Empty username submitted.");
        }

        // Set the username in GameManager
        GameManager.Instance.Username = username;
        GameManager.Instance.AvatarSprite = _selectedAvatarSprite;
        Debug.Log($"Username set: {username}");
        Debug.Log($"Avatar sprite set: {_selectedAvatarSprite.name}");

        GetComponent<UITransition>().TransitionOut();

      
        DialogueLua.SetVariable("Username", username); // Save to Lua variable   
       
        Debug.Log(DialogueLua.GetVariable("UsernameEmptyInputCount").asInt);
        Debug.Log(DialogueLua.GetVariable("Username").asString);
        StartCoroutine(SendMessageAfterDelay("NameEntered", 1f));
    }

    public IEnumerator SendMessageAfterDelay(string message, float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log($"Message ready to send: {message}");
        Sequencer.Message(message);
        Debug.Log($"Message sent: {message}");
    }

}

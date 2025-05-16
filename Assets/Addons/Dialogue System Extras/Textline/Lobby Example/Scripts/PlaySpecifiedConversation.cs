using UnityEngine;
using PixelCrushers;
using System;

namespace PixelCrushers.DialogueSystem.Extras
{

    public class PlaySpecifiedConversation : MonoBehaviour
    {

        private void Awake()
        {
            SaveSystem.saveDataApplied += OnSaveDataApplied;
        }        

        private void OnSaveDataApplied()
        {
            SaveSystem.saveDataApplied -= OnSaveDataApplied;

            if (DialogueManager.isConversationActive) return;
            var conversationTitle = !string.IsNullOrEmpty(SMSDialogueUI.conversationVariableOverride)
                ? SMSDialogueUI.conversationVariableOverride
                : DialogueLua.GetVariable("Conversation").asString;
            if (!string.IsNullOrEmpty(conversationTitle))
            {
                Debug.Log("Dialogue System: Starting conversation '" + conversationTitle + "' from beginning");
                DialogueManager.StartConversation(conversationTitle);
            }
        }

    }
}

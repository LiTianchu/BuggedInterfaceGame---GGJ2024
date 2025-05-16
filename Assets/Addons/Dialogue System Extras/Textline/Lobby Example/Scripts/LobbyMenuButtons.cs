using UnityEngine;
using PixelCrushers;

namespace PixelCrushers.DialogueSystem.Extras
{

    public class LobbyMenuButtons : MonoBehaviour
    {

        public string gameplaySceneName = "1 Gameplay";
        public int saveSlot = 1;
        public UnityEngine.UI.Button continueButton;
        public UnityEngine.UI.Button restartButton;

        private static bool hasShownContinueButton = false;

        private void Start()
        {
            if (continueButton == null) Debug.LogWarning(GetType().Name + ": Continue Button isn't assigned.", this);
            if (restartButton == null) Debug.LogWarning(GetType().Name + ": Restart Button isn't assigned.", this);
            var hasSavedGame = SaveSystem.HasSavedGameInSlot(saveSlot);
            if (continueButton != null) continueButton.gameObject.SetActive(hasSavedGame && !hasShownContinueButton);
            if (restartButton != null) restartButton.gameObject.SetActive(hasSavedGame);
            hasShownContinueButton = true;
        }

        public void StartOrResumeConversation(string conversationTitle)
        {
            SMSDialogueUI.conversationVariableOverride = conversationTitle;
            SaveSystem.LoadScene(gameplaySceneName);
        }
    }
}

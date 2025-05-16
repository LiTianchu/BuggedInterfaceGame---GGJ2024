using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem.Extras;

namespace PixelCrushers.DialogueSystem.MenuSystem
{

    public class MenuUtility : MonoBehaviour
    {

        [Tooltip("Open this URL to rate the app.")]
        public string rateURL;

        public void RateThisApp()
        {
            Application.OpenURL(rateURL);
        }

        public void OpenPauseMenu()
        {
            FindObjectOfType<Pause>().pausePanel.Open();
        }

        //public void StartConversation(string title)
        //{
        //    StartCoroutine(StartConversationAfterSaveSystem(title));
        //}

        //private IEnumerator StartConversationAfterSaveSystem(string title)
        //{
        //    for (int i = 0; i < (SaveSystem.framesToWaitBeforeApplyData + 1); i++)
        //    {
        //        yield return null;
        //    }
        //    if (!DialogueManager.isConversationActive)
        //    {
        //        DialogueLua.SetVariable("Conversation", title);
        //        var textlineDialogueUI = FindObjectOfType<TextlineDialogueUI>();
        //        if (DialogueLua.DoesVariableExist(textlineDialogueUI.currentDialogueEntryRecords))
        //        {
        //            var originalDontLoadScenes = textlineDialogueUI.dontLoadConversationInScenes;
        //            textlineDialogueUI.dontLoadConversationInScenes = new int[0] { }; // Make sure we load regardless of scene.
        //            textlineDialogueUI.OnApplyPersistentData();
        //            textlineDialogueUI.dontLoadConversationInScenes = originalDontLoadScenes;
        //        }
        //        else
        //        {
        //            DialogueManager.StartConversation(title);
        //        }
        //    }
        //}

        public void SaveAndReturnToTitleMenu()
        {
            StartCoroutine(ReturnToTitleWhenDoneSaving());
        }

        private IEnumerator ReturnToTitleWhenDoneSaving()
        {
            var textlineDialogueUI = FindObjectOfType<TextlineDialogueUI>();
            var originalDontLoadScenes = textlineDialogueUI.dontLoadConversationInScenes;
            textlineDialogueUI.dontLoadConversationInScenes = new int[0] { }; // Make sure we save regardless of scene.
            var saveHelper = FindObjectOfType<SaveHelper>();
            saveHelper.QuickSave();
            yield return null;
            yield return new WaitForEndOfFrame();
            textlineDialogueUI.dontLoadConversationInScenes = originalDontLoadScenes;
            DialogueManager.StopConversation();
            saveHelper.ReturnToTitleMenu();
        }

    }
}
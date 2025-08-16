using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Add this to the Dialogue Manager. It adds a Lua function that you can use in your
/// dialogue entries' Script fields:
/// 
/// ChangeActorName(actorName, newDisplayName)
/// </summary>
public class ChangeActorNameLua : MonoBehaviour
{

    void OnEnable()
    {
        // Make the function available to Lua:
        Lua.RegisterFunction("ChangeActorName", this, SymbolExtensions.GetMethodInfo(() => ChangeActorName(string.Empty, string.Empty)));
    }

    void OnDisable()
    {
        // Remove the function from Lua:
        Lua.UnregisterFunction("ChangeActorName");
    }

    public void ChangeActorName(string actorName, string newDisplayName)
    {
        if (DialogueDebug.LogInfo) Debug.Log("Dialogue System: Changing " + actorName + "'s Display Name to " + newDisplayName);
        DialogueLua.SetActorField(actorName, "Display Name", newDisplayName);
        if (DialogueManager.IsConversationActive)
        {
            var actor = DialogueManager.MasterDatabase.GetActor(actorName);
            if (actor != null)
            {
                var info = DialogueManager.ConversationModel.GetCharacterInfo(actor.id);
                if (info != null) info.Name = newDisplayName;
            }
        }
    }
}

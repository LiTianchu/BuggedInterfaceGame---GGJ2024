using UnityEngine;
using UnityEditor; //<-- Include this line in editor scripts.
using PixelCrushers.DialogueSystem;

//custom editor to create the serailize field https://www.pixelcrushers.com/phpbb/viewtopic.php?p=27142
[CustomEditor(typeof(PopupUISubtitlePanel), true)]
public class PopupUISubtitlePanelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // Draw the regular StandardUISubtitlePanel fields.
        
        // Draw fields that were added in MySubtitlePanel:
        serializedObject.Update();
        //EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(PopupUISubtitlePanel.killZoneLayerMask)));
        serializedObject.ApplyModifiedProperties();
    }

}

NOTE: This is a work in progress. Some edge cases may still need to be addressed.

This supplement adds support for Enhanced Scroller:
https://assetstore.unity.com/packages/tools/gui/enhancedscroller-36378

To use it:

1. Select Edit > Project Settings > Player. Add this Scripting Define Symbol:

   USE_ENHANCEDSCROLLER
   
2. To see it in action, in build settings replace  "0 Start" with 
   "0 Start EnhancedScroller" and play the scene. If it doesn't work, you
   may need to right-click on the Scripts folder and select Reimport.
   
3. The Dialogue Manager points to a new dialogue UI prefab named 
   "Textline Dialogue UI EnhancedScroller" with a TextlineDialogueUIEnhancedScroller script.
   - This script points to a TextlineCellView prefab.
   - The TextlineCellView prefab points to these UI templates that you can customize:
      - NPC subtitle
	  - PC subtitle
	  - Response menu
	  - "NPC is typing" pre-delay icon

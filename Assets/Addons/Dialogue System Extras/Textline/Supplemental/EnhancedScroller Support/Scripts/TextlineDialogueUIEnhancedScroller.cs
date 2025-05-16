#if USE_ENHANCEDSCROLLER
using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrushers.DialogueSystem.Extras
{

    public class TextlineDialogueUIEnhancedScroller : TextlineDialogueUI, IEnhancedScrollerDelegate
    {
        [Header("Enhanced Scroller")]
        public EnhancedScroller enhancedScroller;
        public TextlineCellView cellViewPrefab;

        [System.Serializable]
        public class CellData
        {
            public GameObject prefab;
            public Subtitle subtitle;
            public Response[] responses;
            public float timeout;
            public string info;
        }

        [System.NonSerialized]
        public List<CellData> cellDataList = new List<CellData>();

        protected override void CheckAssignments()
        {
            if (enhancedScroller == null) Debug.LogWarning("Textline: Enhanced Scroller is not assigned", this);
            if (cellViewPrefab == null)
            {
                Debug.LogWarning("Textline: Cell View Prefab is not assigned", this);
            }
            else
            {
                if (cellViewPrefab.npcSubtitlePanelPrefab == null) Debug.LogWarning("Textline: Cell View Prefab's NPC Panel is not assigned", this);
                if (cellViewPrefab.pcSubtitlePanelPrefab == null) Debug.LogWarning("Textline: Cell View Prefab's PC Panel is not assigned", this);
                if (cellViewPrefab.menuPanelPrefab == null) Debug.LogWarning("Textline: Cell View Prefab's Menu Panel is not assigned", this);
                if (cellViewPrefab.iconPanelPrefab == null) Debug.LogWarning("Textline: Cell View Prefab's Icon Panel is not assigned", this);
            }
        }

        public override void Open()
        {
            base.Open();
            enhancedScroller.Delegate = this;
            if (!isLoadingGame)
            {
                cellDataList.Clear();
            }
        }

        public override void ShowResponses(Subtitle subtitle, Response[] responses, float timeout)
        {
            if (isInPreDelay && !isLoadingGame)
            {
                StartCoroutine(ShowResponsesAfterPreDelay(subtitle, responses, timeout));
            }
            else
            {
                ShowResponsesNow(subtitle, responses, timeout);
            }
        }

        protected override void ShowResponsesNow(Subtitle subtitle, Response[] responses, float timeout)
        {
            cellDataList.RemoveAll(x => x.prefab == cellViewPrefab.iconPanelPrefab.gameObject);
            var cellData = new CellData();
            cellData.prefab = cellViewPrefab.menuPanelPrefab.gameObject;
            cellData.subtitle = subtitle;
            cellData.responses = responses;
            cellData.timeout = timeout;
            cellDataList.Add(cellData);
            enhancedScroller.ReloadData();
            ScrollToBottom();
        }

        protected override IEnumerator AddMessageWithPreDelay(float preDelay, Subtitle subtitle)
        {
            if (isLoadingGame)
            {
                AddMessage(subtitle);
                yield break;
            }
            isInPreDelay = true;
            yield return null;
            var cellData = new CellData();
            cellData.prefab = cellViewPrefab.iconPanelPrefab;
            cellDataList.Add(cellData);
            enhancedScroller.ReloadData();
            ScrollToBottom();
            yield return new WaitForSeconds(preDelay);
            Sequencer.Message(subtitle.speakerInfo.isNPC ? "Received" : "Sent");
            AddMessage(subtitle);
            isInPreDelay = false;
        }

        /// <summary>
        /// Adds the subtitle as a message in the UI.
        /// </summary>
        protected override void AddMessage(Subtitle subtitle)
        {
            cellDataList.RemoveAll(x => x.prefab == cellViewPrefab.menuPanelPrefab.gameObject || x.prefab == cellViewPrefab.iconPanelPrefab.gameObject);
            var cellData = new CellData();
            cellData.prefab = subtitle.speakerInfo.IsNPC ? cellViewPrefab.npcSubtitlePanelPrefab.gameObject : cellViewPrefab.pcSubtitlePanelPrefab.gameObject;
            cellData.subtitle = subtitle;
            cellData.responses = null;
            cellDataList.Add(cellData);
            if (!isLoadingGame)
            {
                enhancedScroller.ReloadData();
                ScrollToBottom();
            }
        }

        protected override IEnumerator ScrollToBottomCoroutine()
        {
            if (enhancedScroller == null) enhancedScroller = GetComponentInChildren<EnhancedScroller>();
            if (enhancedScroller == null)
            {
                Debug.LogWarning(GetType().Name + ": No Enhanced Scroller is assigned. Can't scroll to bottom.");
                yield break;
            }
            if (scrollRect == null) scrollRect = enhancedScroller.GetComponent<UnityEngine.UI.ScrollRect>();
            if (scrollRect == null)
            {
                Debug.LogWarning(GetType().Name + ": No ScrollRect found on the Enhanced Scroller component. Can't scroll to bottom.");
                yield break;
            }
            yield return null;
            if (contentPanel == null) contentPanel = enhancedScroller.transform.GetChild(0).GetComponent<RectTransform>();
            if (contentPanel == null)
            {
                Debug.LogWarning(GetType().Name + ": No content panel found under the Enhanced Scroller. Can't scroll to bottom.");
                yield break;
            }
            var contentHeight = contentPanel.rect.height;
            var scrollRectHeight = scrollRect.GetComponent<RectTransform>().rect.height;
            var needToScroll = contentHeight > scrollRectHeight;
            if (needToScroll)
            {
                var ratio = scrollRectHeight / contentHeight;
                var timeout = Time.time + 10f; // Avoid infinite loops by maxing out at 10 seconds.
                while (scrollRect.verticalNormalizedPosition > 0.01f && Time.time < timeout)
                {
                    var newPos = scrollRect.verticalNormalizedPosition - scrollSpeed * Time.deltaTime * ratio;
                    scrollRect.verticalNormalizedPosition = Mathf.Max(0, newPos);
                    yield return null;
                }
            }
            scrollRect.verticalNormalizedPosition = 0;
            scrollCoroutine = null;
        }

        /// <summary>
        /// When loading a game, load the dialogue entry records and resume the conversation.
        /// </summary>
        public override void OnApplyPersistentData()
        {
            if (DontLoadInThisScene()) Debug.Log("OnApplyPersistentData Dont Load in this scene: " + SceneManager.GetActiveScene().buildIndex);
            if (DontLoadInThisScene()) return;
            records.Clear();
            if (!DialogueLua.DoesVariableExist(currentDialogueEntryRecords)) return;
            StopAllCoroutines();

            var actorName = DialogueLua.GetVariable(currentConversationActor).AsString;
            var conversantName = DialogueLua.GetVariable(currentConversationConversant).AsString;
            var s = DialogueLua.GetVariable(currentDialogueEntryRecords).AsString;
            if (string.IsNullOrEmpty(s)) return;

            if (Debug.isDebugBuild) Debug.Log("Dialogue System: Restoring current conversation from " + currentDialogueEntryRecords + ": " + s);
            var ints = s.Split(';');
            var numRecords = Tools.StringToInt(ints[0]);
            for (int i = 0; i < numRecords; i++)
            {
                var conversationID = Tools.StringToInt(ints[1 + i * 2]);
                var entryID = Tools.StringToInt(ints[2 + i * 2]);
                records.Add(new DialogueEntryRecord(conversationID, entryID));
            }

            // If we have records, resume the conversation:
            if (records.Count == 0) return;
            var lastRecord = records[records.Count - 1];
            if (lastRecord.conversationID >= 0 && lastRecord.entryID > 0)
            {
                UnityEngine.UI.Button lastContinueButton = null;
                try
                {
                    // Resume conversation:
                    isLoadingGame = true;
                    cellDataList.Clear();
                    var conversation = DialogueManager.MasterDatabase.GetConversation(lastRecord.conversationID);
                    var actor = GameObject.Find(actorName);
                    var conversant = GameObject.Find(conversantName);
                    var actorTransform = (actor != null) ? actor.transform : null;
                    var conversantTransform = (conversant != null) ? conversant.transform : null;
                    if (Debug.isDebugBuild) Debug.Log("Resuming '" + conversation.Title + "' at entry " + lastRecord.entryID);
                    DialogueManager.StopConversation();
                    var lastEntry = DialogueManager.MasterDatabase.GetDialogueEntry(lastRecord.conversationID, lastRecord.entryID);
                    var originalSequence = lastEntry.Sequence; // Handle last entry's sequence differently if end entry.
                    npcPreDelaySettings.CopyTo(npcPreDelaySettingsCopy);
                    pcPreDelaySettings.CopyTo(pcPreDelaySettingsCopy);
                    npcPreDelaySettings.basedOnTextLength = false;
                    npcPreDelaySettings.additionalSeconds = 0;
                    pcPreDelaySettings.basedOnTextLength = false;
                    pcPreDelaySettings.additionalSeconds = 0;
                    var isEndEntry = lastEntry.Sequence.Contains("WaitForMessage(Forever)") || lastEntry.outgoingLinks.Count == 0;
                    if (isEndEntry)
                    {
                        if (!lastEntry.Sequence.Contains("WaitForMessage(Forever)"))
                        {
                            lastEntry.Sequence = "WaitForMessage(Forever); " + lastEntry.Sequence;
                        }
                    }
                    else if (dontRepeatLastSequence)
                    {
                        lastEntry.Sequence = "Continue()";
                    }
                    else
                    {
                        //--- Replay entire last sequence: lastEntry.Sequence = "Delay(0.1)";
                        //--- Will send Sent/Received messages later in method in case sequences wait for them.
                    }
                    skipNextRecord = true;
                    isInPreDelay = false;
                    Open();
                    var conversationModel = new ConversationModel(DialogueManager.masterDatabase, conversation.Title, actorTransform, conversantTransform, true, null, lastRecord.entryID);

                    lastContinueButton = continueButton;
                    npcPreDelaySettingsCopy.CopyTo(npcPreDelaySettings);
                    pcPreDelaySettingsCopy.CopyTo(pcPreDelaySettings);

                    // Populate UI with previous records:
                    var lastInstance = (instantiatedMessages.Count > 0) ? instantiatedMessages[instantiatedMessages.Count - 1] : null;
                    instantiatedMessages.Remove(lastInstance);
                    DestroyInstantiatedMessages();
                    for (int i = 0; i < records.Count - 1; i++)
                    {
                        var entry = DialogueManager.MasterDatabase.GetDialogueEntry(records[i].conversationID, records[i].entryID);
                        var speakerInfo = conversationModel.GetCharacterInfo(entry.ActorID);
                        var listenerInfo = conversationModel.GetCharacterInfo(entry.ConversantID);
                        var formattedText = FormattedText.Parse(entry.currentDialogueText);
                        var subtitle = new Subtitle(speakerInfo, listenerInfo, formattedText, "None()", entry.ResponseMenuSequence, entry);
                        AddMessage(subtitle);
                    }
                    if (lastInstance != null)
                    {
                        instantiatedMessages.Add(lastInstance);
                        lastInstance.transform.SetAsLastSibling();
                    }

                    // Start conversation:
                    skipNextRecord = true;
                    isInPreDelay = false;
                    DialogueManager.StartConversation(conversation.Title, actorTransform, conversantTransform, lastRecord.entryID);
                    lastEntry.Sequence = originalSequence;
                    npcPreDelaySettingsCopy.CopyTo(npcPreDelaySettings);
                    pcPreDelaySettingsCopy.CopyTo(pcPreDelaySettings);

                    // Advance conversation if playing last sequence and it's configured to wait for Sent/Received messages:
                    if (!dontRepeatLastSequence)
                    {
                        Sequencer.Message("Sent");
                        Sequencer.Message("Received");
                    }
                }
                finally
                {
                    StartCoroutine(SetIsLoadingGame(false));
                    enhancedScroller.ReloadData();
                    ScrollToBottom();
                    continueButton = lastContinueButton;
                    if (shouldShowContinueButton && lastContinueButton != null) lastContinueButton.gameObject.SetActive(true);
                }
            }
            ScrollToBottom();
        }

        IEnumerator SetIsLoadingGame(bool value)
        {
            yield return null;
            isLoadingGame = false;
        }

        #region EnhancedScroller Handlers

        /// <summary>
        /// This tells the scroller the number of cells that should have room allocated. 
        /// This should be the length of your data array.
        /// </summary>
        /// <param name="scroller">The scroller that is requesting the data size</param>
        /// <returns>The number of cells</returns>
        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return cellDataList.Count;
        }

        /// <summary>
        /// This tells the scroller what the size of a given cell will be. Cells can be any size and do not have
        /// to be uniform. For vertical scrollers the cell size will be the height. For horizontal scrollers the
        /// cell size will be the width.
        /// </summary>
        /// <param name="scroller">The scroller requesting the cell size</param>
        /// <param name="dataIndex">The index of the data that the scroller is requesting</param>
        /// <returns>The size of the cell</returns>
        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            // in this example, even numbered cells are 30 pixels tall, odd numbered cells are 100 pixels tall
            //return (dataIndex % 2 == 0 ? 30f : 100f);
            return 46;
        }

        /// <summary>
        /// Gets the cell to be displayed. You can have numerous cell types, allowing variety in your list.
        /// Some examples of this would be headers, footers, and other grouping cells.
        /// </summary>
        /// <param name="scroller">The scroller requesting the cell</param>
        /// <param name="dataIndex">The index of the data that the scroller is requesting</param>
        /// <param name="cellIndex">The index of the list. This will likely be different from the dataIndex if the scroller is looping</param>
        /// <returns>The cell for the scroller to use</returns>
        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cellData = cellDataList[dataIndex];

            // first, we get a cell from the scroller by passing a prefab.
            // if the scroller finds one it can recycle it will do so, otherwise
            // it will create a new cell.
            var cellView = scroller.GetCellView(cellViewPrefab) as TextlineCellView;

            // set the name of the game object to the cell's data index.
            // this is optional, but it helps up debug the objects in 
            // the scene hierarchy.
            var text = (cellData.prefab == cellViewPrefab.iconPanelPrefab) ? "..." : cellData.subtitle.formattedText.text;
            cellView.name = "Cell " + dataIndex + ": " + cellData.prefab.name + ": " + ((text.Length <= 20) ? text : text.Substring(0, Mathf.Min(20, text.Length)) + "...");

            // in this example, we just pass the data to our cell's view which will update its UI
            //cellView.SetData(_data[dataIndex]);
            if (cellData.prefab == cellViewPrefab.iconPanelPrefab)
            {
                cellView.SetIconData();
            }
            else if (cellData.prefab == cellViewPrefab.npcSubtitlePanelPrefab.gameObject || cellData.prefab == cellViewPrefab.pcSubtitlePanelPrefab.gameObject)
            {
                cellView.SetSubtitleData(cellData.subtitle, isLoadingGame);
            }
            else
            {
                cellView.SetMenuData(cellData.subtitle, cellData.responses, transform);
            }
            var adjustMinLayoutHeight = cellView.GetComponent<AdjustMinLayoutHeight>();
            if (adjustMinLayoutHeight != null) adjustMinLayoutHeight.Activate();

            // return the cell to the scroller
            return cellView;
        }

        #endregion

    }
}
#endif
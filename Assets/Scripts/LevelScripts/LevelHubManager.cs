using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelHubManager : Singleton<LevelHubManager>
{
    [SerializeField] private GameObject desktopGUI;
    [SerializeField] private GameObject fileSystem;
    [SerializeField] private List<GameObject> levels;

    [SerializeField] private List<CrumbleObject> crumbleObjects;

    private int _numOfCrumbleObjects;
    private bool _biosLoaded = false;
    public event System.Action OnStartLoadingBios;

    private void Start()
    {
        _numOfCrumbleObjects = crumbleObjects.Count;
        Actor user = DialogueManager.masterDatabase.GetActor("User");
        if (GameManager.Instance.AvatarSprite != null)
        {
            user.spritePortrait = GameManager.Instance.AvatarSprite;
            DialogueLua.SetActorField("User", "Avatar", GameManager.Instance.AvatarSprite);
        }
        Debug.Log("User portrait: " + user.spritePortrait.name);
        foreach (Field f in user.fields)
        {
            Debug.Log("User field: " + f.title + " = " + f.value);
        }
    }

    private void Update()
    {
        // if (!_biosLoaded && InventoryManager.Instance.KeyCount >= 3)
        // {
        //     _biosLoaded = true;
        //     StartCoroutine(LoadBios());
        // }
        // if(Keyboard.current.oKey.wasPressedThisFrame)
        // {
        //     StartCoroutine(LoadBios());
        // }
    }

    public void ShowLevelHubScreen()
    {
        foreach (GameObject level in levels)
        {

            if (level.CompareTag("LevelHubScreen"))
            {
                if (!level.activeSelf)
                {
                    level.SetActive(true);
                }
            }
            else
            {
                level.SetActive(false);
            }
        }
    }

    public void ShowFileSystemScreen()
    {
        foreach (GameObject level in levels)
        {
            if (level.CompareTag("FileSystemScreen"))
            {
                if (!level.activeSelf)
                {
                    level.SetActive(true);
                }
            }
            else
            {
                level.SetActive(false);
            }
        }
    }

    public void StartLoadingBios()
    {
        if (_biosLoaded)
        {
            Debug.LogWarning("BIOS is already loaded.");
            return;
        }

        StartCoroutine(LoadBios());
    }

    public IEnumerator LoadBios()
    {
        _biosLoaded = true;
        OnStartLoadingBios?.Invoke();
        desktopGUI.SetActive(true);
        fileSystem.SetActive(false);
        DialogueManager.StopAllConversations(); // Stop any ongoing conversations
        DialogueManager.StartConversation("Entering BIOS");

        foreach (CrumbleObject crumbleObject in crumbleObjects)
        {
            if (crumbleObject != null)
            {
                crumbleObject.Crumble();
            }

            yield return new WaitForSeconds(Random.Range(0.15f, 0.3f));
        }

        yield return new WaitForSeconds(2f);
        Debug.Log("Loading BIOS...");
        SceneManager.LoadScene("BIOS", LoadSceneMode.Single);
    }




}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelHubManager : Singleton<LevelHubManager>
{
    [SerializeField] private List<GameObject> levels;

    [SerializeField] private List<CrumbleObject> crumbleObjects;

    private int _numOfCrumbleObjects;
    private bool _biosLoaded = false;
    public event System.Action OnStartLoadingBios;

    private void Start()
    {
        _numOfCrumbleObjects = crumbleObjects.Count;
    }

    private void Update()
    {
        if (!_biosLoaded && InventoryManager.Instance.KeyCount >= 3)
        {
            _biosLoaded = true;
            StartCoroutine(LoadBios());
        }
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

    public IEnumerator LoadBios()
    {
        OnStartLoadingBios?.Invoke();

        foreach (CrumbleObject crumbleObject in crumbleObjects)
        {
            crumbleObject.Crumble();

            yield return new WaitForSeconds(Random.Range(0.01f, 0.03f));
        }

        yield return new WaitForSeconds(1.75f);
        Debug.Log("Loading BIOS...");
        SceneManager.LoadScene("BIOS", LoadSceneMode.Single);
    }




}

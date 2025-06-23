using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FileSystem : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private UnityEvent onFileSystemEnabled;
    [SerializeField] private UnityEvent onFileSystemDisabled;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        onFileSystemEnabled?.Invoke();
    }

    void OnDisable()
    {
        onFileSystemDisabled.Invoke();
    }
}

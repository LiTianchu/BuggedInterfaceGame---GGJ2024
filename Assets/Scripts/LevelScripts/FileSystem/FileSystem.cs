using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FileSystem : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private UnityEvent onFileSystemEnabled;
    [SerializeField] private UnityEvent onFileSystemDisabled;

    void OnEnable()
    {
        onFileSystemEnabled?.Invoke();
    }

    void OnDisable()
    {
        onFileSystemDisabled.Invoke();
    }
}

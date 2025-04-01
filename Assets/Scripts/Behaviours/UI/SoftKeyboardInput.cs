using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_InputField))]
public class SoftKeyboardInput : MonoBehaviour,IPointerDownHandler
{
    private TMP_InputField _inputField;
    // Start is called before the first frame update
    void Start()
    {
        _inputField = GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AppendCharacter(string character)
    {
        _inputField = GetComponent<TMP_InputField>();
        _inputField.text += character;
    }

    public void RemoveLastCharacter()
    {
        if(_inputField.text.Length == 0){
            return;
        }
        _inputField.text = _inputField.text.Substring(0, _inputField.text.Length - 1);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SoftKeyboard.Instance.ShowKeyboard(this);
    }
}
